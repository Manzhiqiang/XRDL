using Kingdee.BOS;
using Kingdee.BOS.App.Core;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Core.Metadata.FormElement;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Kingdee.K3.MFG.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Tools
{
    public class CommonBillHelper
    {
       
        /// <summary>
        ///  创建单据（填充默认值、触发实体服务规则）
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="formId">单据元数据标识</param>
        /// <param name="action"> 1.调用IDynamicFormViewService.UpdateValue: 会执行字段的值更新事件；2.调用 dynamicFormView.SetItemValueByNumber ：不会执行值更新事件，需要继续调用： ((IDynamicFormView)dynamicFormView).InvokeFieldUpdateService(key, rowIndex);</param>
        ///<param name="pkId">pkId=null,表示新增；pkId有值，表示修改具体单据</param>
        /// <param name="billTypeId">单据类型</param>
        /// <returns></returns>
        public IBillView CreateBillView(Context ctx, string formId, Action<IDynamicFormViewService> action, object pkId = null, string billTypeId = "")
        {
            // 读取单据的元数据
            FormMetadata meta = AppServiceContext.MetadataService.Load(ctx, formId) as FormMetadata;
            Form form = meta.BusinessInfo.GetForm();
            IDynamicFormViewService dynamicFormViewService = (IDynamicFormViewService)Activator.CreateInstance(Type.GetType("Kingdee.BOS.Web.Import.ImportBillView,Kingdee.BOS.Web"));

            // 创建视图加载参数对象，指定各种参数，如FormId, 视图(LayoutId)等
            BillOpenParameter openParam = new BillOpenParameter(form.Id, meta.GetLayoutInfo().Id);
            openParam.Context = ctx;
            openParam.ServiceName = form.FormServiceName;
            openParam.PageId = Guid.NewGuid().ToString();
            openParam.FormMetaData = meta;
            openParam.Status = (pkId == null || Convert.ToInt64(pkId) == 0) ? OperationStatus.ADDNEW : OperationStatus.EDIT;
            openParam.PkValue = pkId;
            openParam.CreateFrom = CreateFrom.Default;
            // 单据类型
            openParam.DefaultBillTypeId = billTypeId;
            openParam.SetCustomParameter("ShowConfirmDialogWhenChangeOrg", false);
            // 插件
            List<AbstractDynamicFormPlugIn> plugs = form.CreateFormPlugIns();
            openParam.SetCustomParameter(FormConst.PlugIns, plugs);
            PreOpenFormEventArgs args = new PreOpenFormEventArgs(ctx, openParam);
            foreach (var plug in plugs)
            {
                plug.PreOpenForm(args);
            }
            // 动态领域模型服务提供类，通过此类，构建MVC实例
            IResourceServiceProvider provider = form.GetFormServiceProvider(false);

            dynamicFormViewService.Initialize(openParam, provider);
            //object bv = provider.GetService(typeof(IDynamicFormView));
            //IDynamicFormViewService dynamicFormViewService=bv as IDynamicFormViewService;
            //dynamicFormViewService.Initialize(openParam, provider);

            // 构建一个IBillView实例，通过此实例，可以方便的填写各属性
            IBillView billView = dynamicFormViewService as IBillView;
            ((IBillViewService)billView).LoadData();

            // 触发插件的OnLoad事件：
            // 组织控制基类插件，在OnLoad事件中，对主业务组织改变是否提示选项进行初始化。
            // 如果不触发OnLoad事件，会导致主业务组织赋值不成功
            DynamicFormViewPlugInProxy eventProxy = billView.GetService<DynamicFormViewPlugInProxy>();
            eventProxy.FireOnLoad();
            if (action != null)
            {
                //按需填充单据属性字段
                action(dynamicFormViewService);
            }

            // 设置FormId
            form = billView.BillBusinessInfo.GetForm();
            if (form.FormIdDynamicProperty != null)
            {
                form.FormIdDynamicProperty.SetValue(billView.Model.DataObject, form.Id);
            }
            return billView;
        }
        /// <summary>
        /// 将数据绑定到动态表单
        /// </summary>
        /// <param name="view">View对象</param>
        /// <param name="result">要展示的数据</param>
        /// <param name="entryId">分录标识</param>
        public void FillListEntity(IDynamicFormView view, IOperationResult result, string entryId)
        {
            List<DynamicObject> praDatas = (List<DynamicObject>)result.FuncResult;
            Entity entity = view.BusinessInfo.GetEntity(entryId);
            DynamicObjectCollection entrys = view.Model.GetEntityDataObject(entity);
            entrys.Clear();
            if (!result.IsSuccess || praDatas.IsEmpty())
            {
                view.UpdateView(entryId);
                return;
            }
            //开始初始化
            view.Model.BeginIniti();
            foreach (DynamicObject prdData in praDatas)
            {
                entrys.Add(prdData);
            }
            view.Model.EndIniti();
            view.UpdateView(entryId);
        }

        public IOperationResult SaveAndAuditNoTransaction(Context ctx, BusinessInfo info, DynamicObject[] dataEntities, OperateOption option, string operationNumber = "")
        {
            IOperationResult result;

            if (option == null)
            {
                option = OperateOption.Create();
            }
            IOperationResult saveResult = AppServiceContext.SaveService.Save(ctx, info, dataEntities, option.Copy(), operationNumber);
            IEnumerable<object> successPKs = from e in saveResult.OperateResult
                                             where e.SuccessStatus
                                             select e into x
                                             select x.PKValue;
            if (successPKs.Count<object>() == 0)
            {
                result = saveResult;
            }
            else
            {
                SubmitService submitService = new SubmitService();
                IOperationResult submitResult = submitService.Submit(ctx, info, successPKs.ToArray<object>(), FormOperationEnum.Submit.ToString(), option.Copy());
                bool isExit = this.MergeResult(saveResult, submitResult);
                if (isExit)
                {
                    result = saveResult;
                }
                else
                {
                    successPKs = from e in saveResult.OperateResult
                                 where e.SuccessStatus
                                 select e into x
                                 select x.PKValue;
                    if (successPKs.Count<object>() > 0)
                    {
                        List<object> paras = new List<object>
                    {
                        "1"
                    };
                        List<KeyValuePair<object, object>> auditPKs = (from x in successPKs
                                                                       select new KeyValuePair<object, object>(x, "")).ToList<KeyValuePair<object, object>>();
                        SetStatusService statusService = new SetStatusService();
                        IOperationResult auditResult = statusService.SetBillStatus(ctx, info, auditPKs, paras, FormOperationEnum.Audit.ToString(), option.Copy());
                        isExit = this.MergeResult(saveResult, auditResult);
                        if (isExit)
                        {
                            result = saveResult;
                            return result;
                        }
                    }

                    result = saveResult;

                }
            }
            return result;
        }
        private bool MergeResult(IOperationResult saveResult, IOperationResult nextResult)
        {
            bool isExit = false;
            IEnumerable<OperateResult> failResults = from e in nextResult.OperateResult
                                                     where !e.SuccessStatus
                                                     select e;
            if (failResults.Count<OperateResult>() > 0 || nextResult.ValidationErrors.Count > 0)
            {
                saveResult.IsSuccess = false;
                isExit = true;
                using (List<ValidationErrorInfo>.Enumerator enumerator = nextResult.ValidationErrors.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ValidationErrorInfo vError = enumerator.Current;
                        saveResult.ValidationErrors.Add(vError);
                        OperateResult validFailResult = saveResult.OperateResult.FirstOrDefault((OperateResult x) => x.PKValue.ToString().EqualsIgnoreCase(vError.BillPKID));
                        if (validFailResult != null)
                        {
                            saveResult.OperateResult.Remove(validFailResult);
                        }
                    }
                }
                using (IEnumerator<OperateResult> enumerator2 = saveResult.OperateResult.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        OperateResult item = enumerator2.Current;
                        OperateResult nextFailResult = failResults.FirstOrDefault((OperateResult x) => x.PKValue.ToString().EqualsIgnoreCase(item.PKValue.ToString()));
                        item.SuccessStatus = false;
                        item.Message = ((nextFailResult != null) ? nextFailResult.Message : "");
                        item.MessageType = ((nextFailResult != null) ? nextFailResult.MessageType : MessageType.FatalError);
                    }
                }
            }
            return isExit;
        }

        /// <summary>
        /// 保存单据
        /// </summary>
        /// <param name="billView"></param>
        /// <param name="saveOption"></param>
        /// <param name="ctx"></param>
        public IOperationResult SaveBillViewData(IBillView billView, OperateOption saveOption, Context ctx)
        {
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
            // 显示处理结果
            if (saveResult == null)
            {
                throw new KDException("", "未知原因导致保存失败!");
                //this.View.ShowErrMessage("未知原因导致保存物料失败！");
            }
            else if (saveResult.IsSuccess == true)
            {// 保存成功，直接显示
                
                // this.View.ShowOperateResult(saveResult.OperateResult);
               // return ;
            }
            // 保存失败，显示错误信息
            if (saveResult.IsShowMessage)
            {
                //暂存
                var billObj = billView.Model.DataObject;
                var rt = BusinessDataServiceHelper.Save(ctx, billObj);
                saveResult.MergeValidateErrors();
                //throw new KDException("", saveResult.OperateResult.ToString());
                //this.View.ShowOperateResult(saveResult.OperateResult);
            }
            return saveResult;
        }
    }
}
