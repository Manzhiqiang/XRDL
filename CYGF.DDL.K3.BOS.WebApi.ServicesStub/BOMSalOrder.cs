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
using XRDL.DDL.K3.BOS.Tools;

namespace XRDL.DDL.K3.BOS.WebApi.ServicesStub
{

    /// <summary>
    /// 修改时间：2022-11-26
    /// 作用：BOM新增保存
    /// </summary>
    [Description("BOM新增保存并审核")]
    [HotUpdate]
    public class BOMSalOrder : ApiBaseService1
    {
        public BOMSalOrder(KDServiceContext context) : base(context)
        {

        }

        public Context Context { get; private set; }

        public string SaveSalOrder(string Parameters)
        {
            var ShipmentDetailList = new List<KDResultString>();
           
            BOMSaveModel Parms = JsonConvert.DeserializeObject<BOMSaveModel>(Parameters);
            var ship = new PlmBOMParms();
            for (int j=0;j< Parms.PlmMaterialParms.Count;j++)
            {
                var shipmate = Parms.PlmMaterialParms[j];
                if (shipmate.Model.Festimate.ToString() == "1")
                {
                    //传入1时，BOM为修改

                }
                else if(shipmate.Model.Festimate.ToString() == "2")
                {
                    //传入2时，BOM升版
                    bool isss = true;
                    var fmaList = Parms.PlmMaterialParms[j].Model.FTreeEntity.Select(m => m.FMATERIALIDCHILD.FNumber).ToList();
                    foreach (var BillData in fmaList)//判断子项物料是否存在且审核,
                    {
                        var sql1 = "/*dialect*/select FDOCUMENTSTATUS from T_BD_MATERIAL where FNUMBER ='" + BillData + "'";
                        var objc = DBServiceHelper.ExecuteScalar<string>(this.Ctx, sql1, "");
                        if (objc != "C")
                        {
                            isss = false;
                        }
                    }
                    if (isss)
                    {
                        ship = Parms.PlmMaterialParms[j];
                        var data = JsonConvert.SerializeObject(ship);
                        var select = SaveAndAduit("ENG_BOM", data).ExtConvertK3BaseModel();//获取返回数据


                        KDResultString result1 = new KDResultString();
                        result1.IsSuccess = select.isSuccess.ToString();
                        result1.Msg = select.Msg;
                        //result1.Data =
                        result1.Number = Parms.PlmMaterialParms[j].Model.FMATERIALID.FNumber.ToString();// select.FNumber;
                        result1.Id = select.FID.ToString();
                        ShipmentDetailList.Add(result1);
                        
                    }
                    else
                    {
                        KDResultString result2 = new KDResultString();
                        result2.IsSuccess = "False";
                        result2.Msg = "传入子物料部分不存在或未审核";
                        result2.Number = Parms.PlmMaterialParms[j].Model.FMATERIALID.FNumber.ToString();
                        ShipmentDetailList.Add(result2);
                    }
                }
                else
                {
                    var iis = "{\"IsSuccess\":\"未选择修改或升版，传入Festimate为空\"}";
                    return iis;
                }


            }
            var result = JsonConvert.SerializeObject(ShipmentDetailList);
            return result;
        }
    }

}
