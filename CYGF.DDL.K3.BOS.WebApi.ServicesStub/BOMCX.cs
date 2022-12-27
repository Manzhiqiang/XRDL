using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.Util;
using Kingdee.K3.MFG.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XRDL.DDL.K3.BOS.Models;
using XRDL.DDL.K3.BOS.WebApi.ServicesStub.APIBase;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Core.Metadata;

namespace XRDL.DDL.K3.BOS.WebApi.ServicesStub
{

    /// <summary>
    /// 创建时间：2021-12-21
    /// 作用：物料数据接口
    /// </summary>
    [Description("BOM状态查询")]
    [HotUpdate]
    public class BOMCX : ApiBaseService
    {
        public BOMCX(KDServiceContext context) : base(context)
        {

        }
        
        public string GetProOrderData(string Parameters)
        {
            KDResult result = new KDResult();
            var data = JsonConvert.DeserializeObject<BOMs>(Parameters);
            if (string.IsNullOrEmpty(data.FNumber))
            {
                result.IsSuccess = false;
                result.Msg = "输入内容为空！";
            }
            else
            {
                var tdata = GetQueryDatas("ENG_BOM", "FMATERIALID.fnumber ='" + data.FNumber + "'", new[] { "FDocumentStatus" });
                //var tdata = GetQueryDatas("BD_MATERIAL", "FNumber='" + data.FNumber + "'", new[] { "FNumber", "FName" });

                var newdata = tdata.Select(m => new
                {
                    FDocumentStatus = m[0] == null ? string.Empty : m[0].ToString(),
                   
                    //FQty = Convert.ToDecimal(m[4])
                }).ToList();

                result.IsSuccess = true;
                result.Msg = "获取成功。";
                result.Data = JsonConvert.SerializeObject(newdata);

            }
            return JsonConvert.SerializeObject(result);
        }


        public DynamicObjectCollection GetQueryDatas(string FormId, string filter, string[] fieldkeys)
        {

            QueryBuilderParemeter paramCatalog = new QueryBuilderParemeter()
            {
                FormId = FormId,//取数的业务对象
                FilterClauseWihtKey = filter,//过滤条件
                SelectItems = SelectorItemInfo.CreateItems(fieldkeys),//要筛选的字段【业务对象的字段Key】
            };

            DynamicObjectCollection dyDatas = Kingdee.BOS.ServiceHelper.QueryServiceHelper.GetDynamicObjectCollection(this.KDContext.Session.AppContext, paramCatalog);
            return dyDatas;
        }

    }

    public class BOMs
    {
        public string FNumber { get; set; }
    }
}
