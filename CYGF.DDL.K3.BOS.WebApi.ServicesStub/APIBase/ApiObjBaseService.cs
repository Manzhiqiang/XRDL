using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Kingdee.BOS;
using Kingdee.BOS.Core;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.ServicesStub;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.DynamicForm;
using Newtonsoft.Json;
using Kingdee.BOS.JSON;
using CYSD.DDL.K3.BOS.Models;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.App.Data;
using System.Transactions;
using CYSD.DDL.K3.BOS.Tools;

namespace CYSD.DDL.K3.BOS.WebApi.ServicesStub.APIBase
{

    /// <summary>
    /// 创建人：徐思强
    /// 创建时间：2021-2-23
    /// 作用：接口基类,用于对象相关元数据集合
    /// </summary>
    [Description("接口基类")]
    [HotUpdate]
    public class ApiObjBaseService : AbstractWebApiBusinessService
    {
        public Context Ctx;
        public string Msg;
        public bool Validate = true;
        HttpContext req;
        public string formid = string.Empty;
        public ApiObjBaseService(KDServiceContext context)
        : base(context)
        {
            req = this.KDContext.WebContext.Context;
            string UserName = req.Request["UserName"] == null ? "" : req.Request["UserName"].ToString();
            string AppId = "222121_T37oxyCFTog/x+/oWdWK5z9N6K563PrK";//req.Request["AppId"] == null ? "" : req.Request["AppId"].ToString();//请求参数的时候，注意对参数进行 HttpUtility.UrlEncode(AppId)。防止特殊字符串无法获取
            string AppSecret = req.Request["AppSecret"] == null ? "" : req.Request["AppSecret"].ToString();
            string DBId = req.Request["DBId"] == null ? "" : req.Request["DBId"].ToString();
            this.Validate = LoginHelper.LogIn(context, DBId, UserName, AppId, AppSecret, ref this.Msg, ref this.Ctx);
        }
      

        public virtual string Save(DynamicObject obj)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, this.formid);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult opResult = BusinessDataServiceHelper.Save(this.Ctx, meta.BusinessInfo, obj, serviceOption);
            return GetResultMsg(opResult);
        }

        public virtual string Save(string FormId,DynamicObject obj)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, FormId);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult opResult = BusinessDataServiceHelper.Save(this.Ctx, meta.BusinessInfo, obj, serviceOption);
            return GetResultMsg(opResult);
        }

        public virtual string SaveAndAudit(Context ctx, BusinessInfo BusinessInfo, DynamicObject obj)
        {
            IOperationResult result = null;
            using (var tran = new KDTransactionScope(TransactionScopeOption.Required))
            {
                OperateOption saveOption = OperateOption.Create();
                saveOption.SetIgnoreWarning(true);
                // 调用保存操作
                IOperationResult saveResult = BusinessDataServiceHelper.Save(
                            ctx,
                            BusinessInfo,
                           obj,
                            saveOption,
                            "Save");
                if (saveResult.IsSuccess)
                {
                    object[] pk = saveResult.SuccessDataEnity.Select(a => a["Id"]).ToArray();
                    IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(ctx, BusinessInfo, pk, "Submit", saveOption);
                    if (SubmitResult.IsSuccess)
                    {
                        IOperationResult AuditResult = BusinessDataServiceHelper.Audit(ctx, BusinessInfo, pk, saveOption);
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
            return GetResultMsg(result);
        }
        public virtual string SaveAndAudit(Context Ctx, BusinessInfo businessInfo, DynamicObject[] dataObject, OperateOption serviceOption)
        {
            IOperationResult result = null;
            using (var tran = new KDTransactionScope(TransactionScopeOption.Required))
            {
                IOperationResult SaveResult = BusinessDataServiceHelper.Save(
                             Ctx,
                             businessInfo,
                            dataObject,
                             serviceOption,
                            "Save");
                if (SaveResult.IsSuccess)
                {
                    object[] pk = SaveResult.SuccessDataEnity.Select(a => a["Id"]).ToArray();
                    IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(Ctx, businessInfo, pk, "Submit", serviceOption);
                    if (SubmitResult.IsSuccess)
                    {
                        IOperationResult AuditResult = BusinessDataServiceHelper.Audit(Ctx, businessInfo, pk, serviceOption);
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
                    result = SaveResult;
                }
            }
            return GetResultMsg(result);
        }

        public virtual string Submit(object pkId)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, this.formid);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(this.Ctx, meta.BusinessInfo, new object[] { pkId }, "Submit", serviceOption);
            return GetResultMsg(SubmitResult);
        }

        public virtual string Submit(string FormId,object pkId)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, FormId);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(this.Ctx, meta.BusinessInfo, new object[] { pkId }, "Submit", serviceOption);
            return GetResultMsg(SubmitResult);
        }

        //public virtual string Submit(string[] pkId)
        //{
        //    FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Context, this.formid);
        //    OperateOption serviceOption = OperateOption.Create();
        //    serviceOption.SetIgnoreWarning(true);
        //    IOperationResult SubmitResult = BusinessDataServiceHelper.Submit(this.Context, meta.BusinessInfo, pkId, "Submit", serviceOption);
        //    return GetResultMsg(SubmitResult);
        //}

        public virtual string Audit(object pkId)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, this.formid);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult AuditResult = BusinessDataServiceHelper.Audit(this.Ctx, meta.BusinessInfo, new object[] { pkId }, serviceOption);
            return GetResultMsg(AuditResult);
        }

        public virtual string Audit(string FormId,object pkId)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, FormId);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult AuditResult = BusinessDataServiceHelper.Audit(this.Ctx, meta.BusinessInfo, new object[] { pkId }, serviceOption);
            return GetResultMsg(AuditResult);
        }

        public virtual string UnAudit(object pkId)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, this.formid);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult UnAuditResult = BusinessDataServiceHelper.UnAudit(this.Ctx, meta.BusinessInfo, new object[] { pkId }, serviceOption);
            return GetResultMsg(UnAuditResult);
        }

        public virtual string UnAudit(string FormId,object pkId)
        {
            FormMetadata meta = MetaDataServiceHelper.GetFormMetaData(this.Ctx, FormId);
            OperateOption serviceOption = OperateOption.Create();
            serviceOption.SetIgnoreWarning(true);
            IOperationResult UnAuditResult = BusinessDataServiceHelper.UnAudit(this.Ctx, meta.BusinessInfo, new object[] { pkId }, serviceOption);
            return GetResultMsg(UnAuditResult);
        }


        #region --消息处理--

        public virtual string GetResultMsg(IOperationResult opResult)
        {
            KDResult op = new KDResult();
            op.IsSuccess = opResult.IsSuccess;
            op.Msg = GetOpMsg(opResult);
            op.Data = GetData(opResult);
            if (op.IsSuccess == true)
            {
                try
                {
                    op.Id = opResult.SuccessDataEnity.FirstOrDefault()["Id"].ToString();
                    op.Number = opResult.SuccessDataEnity.FirstOrDefault()["BillNo"].ToString();
                    if (string.IsNullOrEmpty(op.Number.Trim()))
                    {
                        op.Number = opResult.SuccessDataEnity.FirstOrDefault()["Number"].ToString();
                    }
                }
                catch
                { }
            }
            return JsonConvert.SerializeObject(op);
        }

        private string GetOpMsg(IOperationResult result)
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

        private string GetData(IOperationResult Result)
        {
            string Data = string.Empty;
            JSONArray Ary = new JSONArray();
            foreach (var p in Result.OperateResult)
            {
                JSONObject obj = new JSONObject();
                obj.Add("PKValue", p.PKValue);
                obj.Add("Number", p.Number);
                obj.Add("Message", p.Message);
                Ary.Add(obj);
            }
            return JsonConvert.SerializeObject(Ary);
        }
        #endregion
    }
}
