using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.FormElement;
using Kingdee.BOS.JSON;
using Kingdee.BOS.Orm;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;


namespace CYSD.DDL.K3.BOS.Tools
{
    public class KDOpView
    {


        #region --单据视图--
        public static IBillView GetBillView(Context ctx, string Formid, long PkValue = 0)
        {
            // 读取单据的元数据
            FormMetadata meta = MetaDataServiceHelper.Load(ctx, Formid) as FormMetadata;
            Form form = meta.BusinessInfo.GetForm();
            // 创建用于引入数据的单据view
            Type type = Type.GetType("Kingdee.BOS.Web.Import.ImportBillView,Kingdee.BOS.Web");
            var billView = (IDynamicFormViewService)Activator.CreateInstance(type);
            // 开始初始化billView：
            // 创建视图加载参数对象，指定各种参数，如FormId, 视图(LayoutId)等

            BillOpenParameter openParam;
            openParam = CreateOpenParameter(ctx, meta, PkValue);
            // 动态领域模型服务提供类，通过此类，构建MVC实例
            var provider = form.GetFormServiceProvider();
            billView.Initialize(openParam, provider);

            ((IBillViewService)billView).LoadData();

            // 触发插件的OnLoad事件：
            // 组织控制基类插件，在OnLoad事件中，对主业务组织改变是否提示选项进行初始化。
            // 如果不触发OnLoad事件，会导致主业务组织赋值不成功
            DynamicFormViewPlugInProxy eventProxy = (billView as IBillView).GetService<DynamicFormViewPlugInProxy>();
            eventProxy.FireOnLoad();

            return billView as IBillView;
        }
        private static BillOpenParameter CreateOpenParameter(Context ctx, FormMetadata meta, long PkValue)
        {
            Form form = meta.BusinessInfo.GetForm();
            // 指定FormId, LayoutId
            BillOpenParameter openParam = new BillOpenParameter(form.Id, meta.GetLayoutInfo().Id);
            // 数据库上下文
            openParam.Context = ctx;
            // 本单据模型使用的MVC框架
            openParam.ServiceName = form.FormServiceName;
            // 随机产生一个不重复的PageId，作为视图的标识
            openParam.PageId = Guid.NewGuid().ToString();
            // 元数据
            openParam.FormMetaData = meta;
            // 界面状态：新增 (修改、查看)
            openParam.Status = PkValue == 0 ? OperationStatus.ADDNEW : OperationStatus.EDIT;
            // 单据主键：本案例演示新建物料，不需要设置主键
            if (openParam.Status == OperationStatus.ADDNEW)
            {
                openParam.PkValue = null;
            }
            else
            {
                openParam.PkValue = PkValue;
            }
            // 界面创建目的：普通无特殊目的 （为工作流、为下推、为复制等）
            openParam.CreateFrom = CreateFrom.Default;
            // 基础资料分组维度：基础资料允许添加多个分组字段，每个分组字段会有一个分组维度
            // 具体分组维度Id，请参阅 form.FormGroups 属性
            //openParam.GroupId = "";
            // 基础资料分组：如果需要为新建的基础资料指定所在分组，请设置此属性
            //openParam.ParentId = 0;
            // 单据类型
            openParam.DefaultBillTypeId = "";
            // 业务流程
            openParam.DefaultBusinessFlowId = "";
            // 主业务组织改变时，不用弹出提示界面
            openParam.SetCustomParameter("ShowConfirmDialogWhenChangeOrg", false);
            // 插件
            List<AbstractDynamicFormPlugIn> plugs = form.CreateFormPlugIns();
            openParam.SetCustomParameter(FormConst.PlugIns, plugs);
            PreOpenFormEventArgs args = new PreOpenFormEventArgs(ctx, openParam);
            foreach (var plug in plugs)
            {// 触发插件PreOpenForm事件，供插件确认是否允许打开界面
                plug.PreOpenForm(args);
            }
            if (args.Cancel == true)
            {// 插件不允许打开界面
             // 本案例不理会插件的诉求，继续....
            }
            // 返回
            return openParam;
        }
        #endregion


        public static bool Save(Context ctx, IBillView billView, ref string Msg)
        {
            OperateOption saveOption = OperateOption.Create();
            saveOption.SetIgnoreWarning(true);
            // 设置FormId
            Form form = billView.BillBusinessInfo.GetForm();
            if (form.FormIdDynamicProperty != null)
            {
                form.FormIdDynamicProperty.SetValue(billView.Model.DataObject, form.Id);
            }
            // 调用保存操作
            IOperationResult saveResult = BusinessDataServiceHelper.Save(
                        ctx,
                        billView.BillBusinessInfo,
                        billView.Model.DataObject,
                        saveOption,
                        "Save");

            Msg = GetOpMsg(saveResult);
            return saveResult.IsSuccess;
        }


        public static IOperationResult SaveAndAudit(Context ctx, IBillView billView)
        {
            IOperationResult result = null;
            using (var tran = new KDTransactionScope(TransactionScopeOption.Required))
            {
                OperateOption saveOption = OperateOption.Create();
                saveOption.SetIgnoreWarning(true);
                // 调用保存操作
                IOperationResult saveResult = BusinessDataServiceHelper.Save(
                            ctx,
                            billView.BillBusinessInfo,
                            billView.Model.DataObject,
                            saveOption,
                            "Save");
                if (saveResult.IsSuccess)
                {
                    object[] pk = saveResult.SuccessDataEnity.Select(a => a["Id"]).ToArray();
                    IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(ctx, billView.BillBusinessInfo, pk, "Submit", saveOption);
                    if (SubmitResult.IsSuccess)
                    {
                        IOperationResult AuditResult = BusinessDataServiceHelper.Audit(ctx, billView.BillBusinessInfo, pk, saveOption);
                        if (AuditResult.IsSuccess)
                            tran.Complete();
                        result = AuditResult;
                    }
                    else
                    {

                        result = SubmitResult;
                    }
                }
                else
                {
                    result = saveResult;
                }
            }
            return result;
        }

        public static bool SaveAndAudit(Context ctx, IBillView billView, ref string Msg)
        {
            IOperationResult result = null;
            using (var tran = new KDTransactionScope(TransactionScopeOption.Required))
            {
                OperateOption saveOption = OperateOption.Create();
                saveOption.SetIgnoreWarning(true);
                // 调用保存操作
                IOperationResult saveResult = BusinessDataServiceHelper.Save(
                            ctx,
                            billView.BillBusinessInfo,
                            billView.Model.DataObject,
                            saveOption,
                            "Save");
                if (saveResult.IsSuccess)
                {
                    object[] pk = saveResult.SuccessDataEnity.Select(a => a["Id"]).ToArray();
                    IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(ctx, billView.BillBusinessInfo, pk, "Submit", saveOption);
                    if (SubmitResult.IsSuccess)
                    {
                        IOperationResult AuditResult = BusinessDataServiceHelper.Audit(ctx, billView.BillBusinessInfo, pk, saveOption);
                        if (AuditResult.IsSuccess)
                            tran.Complete();
                        result = AuditResult;
                        Msg = GetOpMsg(result);
                    }
                    else
                    {

                        result = SubmitResult;
                        Msg = GetOpMsg(result);
                    }
                }
                else
                {
                    result = saveResult;
                    Msg = GetOpMsg(result);
                }
            }
            return result.IsSuccess;
        }

        //public class Result
        //{
        //    public bool IsSuccess;
        //    public string Msg;
        //    public string Data;
        //}
        //public  static string GetResultMsg(IOperationResult opResult)
        //{
        //    Result op = new Result();
        //    op.IsSuccess = opResult.IsSuccess;
        //    op.Msg = GetOpMsg(opResult);
        //    op.Data = GetData(opResult);
        //    return JsonConvert.SerializeObject(op);
        //}

        private static string GetOpMsg(IOperationResult result)
        {
            string Msg = string.Empty;
            if (result.ValidationErrors != null && result.ValidationErrors.Count > 0)
            {
                int i = 1;
                foreach (var item in result.ValidationErrors)
                {
                    Msg += i + "[" + item.DisplayToFieldKey + "]" + item.Message;
                    i++;
                }
            }
            else if (result.OperateResult != null && result.OperateResult.Any())
            {
                foreach (var item in result.OperateResult)
                {
                    if (!item.SuccessStatus)
                    {
                        Msg += item.Message;
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(result.InteractionContext.SimpleMessage))
            {
                Msg += result.InteractionContext.SimpleMessage;
            }
            return Msg;
        }


    }
}
