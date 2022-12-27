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
using Kingdee.BOS.App.Data;
using Kingdee.BOS;
using Kingdee.BOS.ServiceHelper;

namespace XRDL.DDL.K3.BOS.WebApi.ServicesStub
{

    /// <summary>
    /// 创建时间：2022-11-11
    /// 最后修改时间：2022-11-25
    /// 作用：物料新增保存
    /// </summary>
    [Description("物料新增保存")]
    [HotUpdate]
    public class MaterialSave : ApiBaseService
    {
        public MaterialSave(KDServiceContext context) : base(context)
        {

        }

        // public Context Context { get; private set; }

        public string Save(string Parameters)
        {
            var ShipmentDetailList = new List<KDResultString>();
            PlmMaterial Parms = JsonConvert.DeserializeObject<PlmMaterial>(Parameters);
            var ship = new PlmMaterialParms();
            for (int i = 0; i < Parms.PlmMaterialParms.Count; i++)
            {
                ship = Parms.PlmMaterialParms[i];
                var sql = string.Format(@" /*dialect*/ select FNUMBER from T_BD_MATERIAL where FNUMBER='" + ship.Model.FNumber.ToString() + "'");
                DataTable FNUMBER = AppServiceContext.DbUtils.ExecuteDataSet(this.Ctx, sql.Trim()).Tables[0];
                if (FNUMBER.Rows.Count > 0 && FNUMBER != null)//物料修改
                {
                    string SQL = string.Format("/*dialect*/ update t2 set t2.FNAME='"+ ship .Model.FName+ "' ,t2.FSPECIFICATION='" + ship.Model.FSpecification + "' from T_BD_MATERIAL t1 inner join T_BD_MATERIAL_L t2 on t1.FMATERIALID=t2.FMATERIALID and t2.FLOCALEID='2052' where t1.FNUMBER='" + ship.Model.FNumber + "'");
                    AppServiceContext.DbUtils.Execute(this.Ctx, SQL);
                    KDResultString result1 = new KDResultString();
                    result1.IsSuccess = "True";
                    result1.Msg = "当前物料编码已存在，当前操作为修改";
                    result1.Number = ship.Model.FNumber;
                    ShipmentDetailList.Add(result1);
                }
                else
                {
                    var data = JsonConvert.SerializeObject(ship);
                    var saveJG = SaveAndAduit("BD_MATERIAL", data);

                    KDResult saveJSON = JsonConvert.DeserializeObject<KDResult>(saveJG);
                    KDResultString result5 = new KDResultString();
                    result5.IsSuccess = saveJSON.IsSuccess.ToString();
                    result5.Msg = saveJSON.Msg == null ? "" : saveJSON.Msg;
                    result5.Data = saveJSON.Data == null ? "" : saveJSON.Data;
                    result5.Number = saveJSON.Number == null ? "" : saveJSON.Number;
                    result5.Id = saveJSON.Id == null ? "" : saveJSON.Id;
                    ShipmentDetailList.Add(result5);
                   
                }
            }
            var date = JsonConvert.SerializeObject(ShipmentDetailList);
            return date;
        }
    }

}
