using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS;
using System;
using System.Linq;
using System.Data;
using Kingdee.K3.MFG.App;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Orm.Metadata.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Contracts;
using System.Collections.Generic;
using Kingdee.BOS.Authentication;
using Newtonsoft.Json;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.WebApi.ServicesStub;
 using CYSD.DDL.K3.BOS.Models;

namespace CYSD.DDL.K3.BOS.Tools
{
    public class DataHepler
    {
      



        public static long GetOrgId(Context ctx, string OrgNumber)
        {
            string strSQL = string.Format(" SELECT FORGID FROM dbo.T_ORG_ORGANIZATIONS WHERE FNUMBER ='{0}' ", OrgNumber);
            DynamicObjectCollection Objs = DBServiceHelper.ExecuteDynamicObject(ctx, strSQL);
            if (Objs != null && Objs.Count > 0)
            {
                return long.Parse(Objs[0]["FORGID"].ToString());
            }
            else { return 0; }
        }

        //根据物料编码，组织，仓库辅助属性维度，检查库存维度是否启用
        //public static bool IsEnablePosition(Context ctx,string UseOrgNum, string MaterialNum, string AuxName = "仓位")
        //{
        //    bool IsEnable = false;
        //    string strSQL = string.Format(@"SELECT top 1 Pty.FISENABLE  FROM T_BD_MATERIAL M 
        //        INNER JOIN t_BD_MaterialInvPty Pty  ON Pty.FMATERIALID = M.FMATERIALID
        //        INNER JOIN T_ORG_ORGANIZATIONS  Org ON Org.FORGID = M.FUSEORGID
        //        INNER JOIN
        //        (SELECT B.FID FROM T_BD_INVPROPERTY A INNER JOIN T_BD_INVPROPERTY_L  B
        //         ON a.FID = b.FID AND B.FLOCALEID = 2052
        //         WHERE b.FNAME = N'{2}'
        //        ) AuxID  ON AuxID.FID = Pty.FINVPTYID
        //         WHERE M.FNUMBER = '{0}' AND Org.FNUMBER = '{1}'", UseOrgNum, MaterialNum, AuxName);
        //    IDataReader reader = AppServiceContext.DbUtils.ExecuteReader(ctx, strSQL);
        //    while (reader.Read())
        //    {
        //        int i = reader.GetInt16(0);
        //        IsEnable = i > 0 ? true : false;
        //    }
        //    return IsEnable;
        //}

        //根据仓库判断是否启用仓位
        public static bool IsEnablePosition(Context ctx, string UseOrgNum, string StockNum)
        {
            bool IsEnable = false;
            string strSQL = string.Format(@"SELECT FISOPENLOCATION  FROM t_BD_Stock Stock INNER JOIN T_ORG_ORGANIZATIONS Org 
             ON Stock.FUSEORGID=Org.FORGID  WHERE Org.FNUMBER='{0}' AND  Stock.FNUMBER='{1}'", UseOrgNum, StockNum);
            using (IDataReader reader = AppServiceContext.DbUtils.ExecuteReader(ctx, strSQL))
            {
                while (reader.Read())
                {
                    IsEnable = reader.GetString(0) == "0" ? false : true;
                }
            }
            return IsEnable;
        }

        //根据组织，仓库获取一条默认仓位
        public static string DefaultStockLocNumber(Context ctx, string UseOrgNum, string StockNumber)
        {
            string StockLocNumber = string.Empty;
            string strSQL = string.Format(@"SELECT top 1 FValue.FNUMBER  FROM t_BD_Stock Stock  
                        inner join T_ORG_ORGANIZATIONS Org on Stock.FUSEORGID=Org.FORGID
                        inner join T_BD_STOCKFLEXITEM Item on Stock.FSTOCKID=Item.FSTOCKID
                        inner join T_BD_STOCKFLEXDETAIL Detail on Item.FENTRYID=Detail.FENTRYID
                        inner join V_BAS_FLEXVALUESENTRY FValue on Detail.FFLEXENTRYID=FValue.FENTRYID and FValue.fforbidstatus='A' and FValue.fdocumentstatus='C'
                        where Org.FNUMBER='{0}' and Stock.FNUMBER='{1}' 
                        ", UseOrgNum, StockNumber);
            using (IDataReader reader = AppServiceContext.DbUtils.ExecuteReader(ctx, strSQL))
            {
                while (reader.Read())
                {
                    StockLocNumber = reader.GetString(0);
                }
            }
            return StockLocNumber;
        }

        public static long GetStockLocId(Context ctx, string UseOrgNum, string StockNumber, string StockLotNumber)
        {
            //string StockLocNumber = string.Empty;
            string strSQL = string.Format(@"/*dialect*/SELECT S1.FSTOCKID,
S1.FUSEORGID,
       S1.FNUMBER,
       S2.FNAME,
       S3.FSTOCKLOCID,
       locCol1.FNUMBER,
       locCol1_L.FNAME
FROM T_BD_STOCK AS S1
 inner join T_ORG_ORGANIZATIONS Org on S1.FUSEORGID=Org.FORGID
    INNER JOIN T_BD_STOCK_L AS S2
        ON S2.FSTOCKID = S1.FSTOCKID
           AND S2.FLOCALEID = 2052
    INNER JOIN T_BD_FLEXVALUESCOM S3
        ON S3.FSTOCKID = S1.FSTOCKID
    INNER JOIN T_BAS_FLEXVALUESDETAIL loc
        ON S3.FSTOCKLOCID = loc.FID
    INNER JOIN T_BAS_FLEXVALUESENTRY locCol1
        ON (loc.FF100008 = locCol1.FENTRYID)
    INNER JOIN T_BAS_FLEXVALUESENTRY_L locCol1_L
        ON (
               locCol1_L.FENTRYID = locCol1.FENTRYID
               AND locCol1_L.FLOCALEID = 2052
           )
		   where S1.FNUMBER='{0}' and locCol1.FNUMBER='{1}' and Org.FNUMBER='{2}' 
                        ", StockNumber, StockLotNumber, UseOrgNum);
            var dyns = AppServiceContext.DbUtils.ExecuteDynamicObject(ctx, strSQL);
            if (dyns.Any())
            {
                var dyn = dyns.FirstOrDefault();
                if (dyn == null) return 0;
                return Convert.ToInt64(dyn["FSTOCKLOCID"]);
            }
            return 0;
        }

        public static string GetFlexNumber(Context ctx, string FlexName = "仓位")
        {
            string FlexNumber = string.Empty;
            string strSQL = string.Format(@"SELECT a.FFLEXNUMBER FROM  T_BAS_FLEXVALUES a  INNER JOIN dbo.T_BAS_FLEXVALUES_L b ON a.FID = b.FID AND b.FLOCALEID = 2052 WHERE b.FNAME = N'{0}'", FlexName);
            using (IDataReader reader = AppServiceContext.DbUtils.ExecuteReader(ctx, strSQL))
            {
                while (reader.Read())
                {
                    FlexNumber = reader.GetString(0);
                }
            }
            return FlexNumber;
        }

        public static DynamicObject[] GetStock(Context ctx, DynamicObjectType type, string InStockNumber, string stockorgID)
        {
            QueryBuilderParemeter parameter = new QueryBuilderParemeter();
            parameter.FormId = "BD_STOCK";
            parameter.FilterClauseWihtKey = string.Format(" FNumber='{0}' AND FUseOrgId='{1}' ", InStockNumber, stockorgID);
            parameter.IsSingleForQuery = true;
            return BusinessDataServiceHelper.Load(ctx, type, parameter);
        }

        public static DynamicObject GetBillObj(Context ctx,string FormId,string FNumber,string OrgNum)
        {
            List<SelectorItemInfo> list = new List<SelectorItemInfo> { new SelectorItemInfo("FID") };
            QueryBuilderParemeter parameter = new QueryBuilderParemeter();
            parameter.FormId = FormId;
            parameter.FilterClauseWihtKey = string.Format(" FNumber ='{0}' AND FUseOrgId='{1}' ", FNumber, OrgNum);
            //parameter.SelectItems.Add(new SelectorItemInfo("FID"));
            parameter.SelectItems= list;
            parameter.IsSingleForQuery = true;
            return ServiceFactory.GetService<IQueryService>(ctx).GetDynamicObjectCollection(ctx, parameter, null).FirstOrDefault();

        }


        /// <summary>
        /// 根据组织，单据编号获取生产领料单审核
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="FNumber"></param>
        /// <param name="OrgNum"></param>
        /// <returns></returns>
        public static long GerPickMtrId(Context ctx, string FNumber, string OrgNum)
        {
            string strSQL = string.Format(@"/*dialect*/  SELECT  FID from T_PRD_PICKMTRL  WHERE FBILLNO='{0}' ", FNumber);
            DynamicObjectCollection Objs = DBServiceHelper.ExecuteDynamicObject(ctx, strSQL);
            if (Objs != null && Objs.Count > 0)
            {
                return long.Parse(Objs[0]["FID"].ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据组织，单据编号获取直接调拨单审核
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="FNumber"></param>
        /// <param name="OrgNum"></param>
        /// <returns></returns>
        public static long GerSTK_TransferDirectId(Context ctx, string FNumber, string OrgNum)
        {
            string strSQL = string.Format(@"/*dialect*/  SELECT  FID from T_STK_STKTRANSFERIN WHERE FBILLNO='{0}' ", FNumber);
            DynamicObjectCollection Objs = DBServiceHelper.ExecuteDynamicObject(ctx, strSQL);
            if (Objs != null && Objs.Count > 0)
            {
                return long.Parse(Objs[0]["FID"].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static void SetContext(ref Context ctx, string number)
        {
            string sql = string.Format(@"SELECT A.FORGID,B.FNAME,A.FORGFUNCTIONS,A.FACCTORGTYPE FROM T_ORG_ORGANIZATIONS A 
            LEFT JOIN T_ORG_ORGANIZATIONS_L B ON A.FORGID=B.FORGID WHERE (A.FNUMBER='{0}' OR B.FNAME='{0}') AND B.FLOCALEID=2052", number);
            DynamicObject orgObj = DBServiceHelper.ExecuteDynamicObject(ctx, sql).FirstOrDefault();
            if (orgObj != null)
            {
                string Name = Convert.ToString(orgObj["FNAME"]);
                long FORGID = Convert.ToInt32(orgObj["FORGID"]);
                List<long> FunctionIds = new List<long>();
                string[] str = Convert.ToString(orgObj["FORGFUNCTIONS"]).Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    FunctionIds.Add(Convert.ToInt32(str[i]));
                }
                string AcctOrgType = Convert.ToString(orgObj["FACCTORGTYPE"]);
                ctx.CurrentOrganizationInfo = new OrganizationInfo()
                {
                    ID = FORGID,
                    Name = Name,
                    FunctionIds = FunctionIds,
                    AcctOrgType = AcctOrgType
                };
            }

        }

        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="formId"></param>
        ///// <param name="fiter"></param>
        ///// <param name="fieldkeys"></param>
        ///// <returns></returns>
        //public static DynamicObjectCollection GetQueryDatas(string formId, string fiter, Context context, string[] fieldkeys)
        //{
        //    QueryBuilderParemeter paramCatalog = new QueryBuilderParemeter()
        //    {
        //        FormId = formId,
        //        FilterClauseWihtKey = fiter,
        //        SelectItems = SelectorItemInfo.CreateItems(fieldkeys)
        //    };
        //    DynamicObjectCollection dyDatas = Kingdee.BOS.ServiceHelper.QueryServiceHelper.GetDynamicObjectCollection(context, paramCatalog);
        //    return dyDatas;
        //}

    }
}
