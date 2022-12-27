using Kingdee.BOS;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.WebApi.ServicesStub;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Kingdee.BOS.WebApi.FormService;
using XRDL.DDL.K3.BOS.Tools;
using XRDL.DDL.K3.BOS.Models;

namespace XRDL.DDL.K3.BOS.WebApi.ServicesStub.APIBase
{
    public class ApiBaseService1 : AbstractWebApiBusinessService
    {
        public Context Ctx;
        public string Msg;
        public bool Validate = true;
        HttpContext req;
        public ApiBaseService1(KDServiceContext context)
        : base(context)
        {
            req = this.KDContext.WebContext.Context;
            string UserName = req.Request["UserName"] == null ? "" : req.Request["UserName"].ToString();
            string AppId =  req.Request["AppId"] == null ? "" : req.Request["AppId"].ToString();//"221642_S52O1wGIVnhbT9VFx+xN08VJVqT60OLK"; //请求参数的时候，注意对参数进行 HttpUtility.UrlEncode(AppId)。防止特殊字符串无法获取
            string AppSecret = req.Request["AppSecret"] == null ? "" : req.Request["AppSecret"].ToString();
            string DBId = req.Request["DBId"] == null ? "" : req.Request["DBId"].ToString();
            this.Validate = LoginHelper.LogIn(context, DBId, UserName, AppId, AppSecret, ref this.Msg, ref this.Ctx);
        }

        //查询如果成功返回的是数据，所以此处不能简单返回
        //public virtual string View(string Formid, string Parameters)
        //{
        //    if (!this.Validate)
        //    {
        //        return this.Msg;
        //    }
        //    return GetMsg(WebApiServiceCall.View(this.Ctx, Formid, Parameters));
        //}


        public virtual string Save(string Formid, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            return GetMsg(WebApiServiceCall.Save(this.Ctx, Formid, Parameters));
        }

        public virtual string Submit(string Formid, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            return GetMsg(WebApiServiceCall.Submit(this.Ctx, Formid, Parameters));
        }

        public virtual string SaveAndAduit(string Formid, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            object Result = WebApiServiceCall.Save(this.Ctx, Formid, Parameters);
            string strResult = JsonConvert.SerializeObject(Result);
            JObject o = JsonConvert.DeserializeObject(strResult) as JObject;
            if ((bool)o["Result"]["ResponseStatus"]["IsSuccess"] == true)
            {
                string Id = o["Result"]["Id"].ToString();
                string content = "{\"ids\":\"" + Id + "\"}";
                Result = WebApiServiceCall.Submit(this.Ctx, Formid, content);
                strResult = JsonConvert.SerializeObject(Result);
                o = JsonConvert.DeserializeObject(strResult) as JObject;
                if ((bool)o["Result"]["ResponseStatus"]["IsSuccess"] == true)
                {
                    Result = WebApiServiceCall.Audit(this.Ctx, Formid, content);
                }
            }
            return GetMsg(Result);
        }


        public virtual string Audit(string Formid, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            return GetMsg(WebApiServiceCall.Audit(this.Ctx, Formid, Parameters));
        }


        public virtual string UnAudit(string Formid, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            return GetMsg(WebApiServiceCall.UnAudit(this.Ctx, Formid, Parameters));
        }


        public virtual string Delete(string Formid, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            return GetMsg(WebApiServiceCall.Delete(this.Ctx, Formid, Parameters));
        }

        public virtual string Operation(string Formid,string opNumber, string Parameters)
        {
            if (!this.Validate)
            {
                return this.Msg;
            }
            return GetMsg(WebApiServiceCall.Execute(this.Ctx, Formid, opNumber, Parameters));
        }

        public string GetMsg(object ObjResult)
        {
            string strResult = string.Empty;
            KDResult result = new KDResult();
            
                strResult = JsonConvert.SerializeObject(ObjResult);
            return JsonConvert.SerializeObject(strResult);
        }



    }
}
