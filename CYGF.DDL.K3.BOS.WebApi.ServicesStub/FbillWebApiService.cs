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
using CYSD.DDL.K3.BOS.Models;
using CYSD.DDL.K3.BOS.WebApi.ServicesStub.APIBase;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.WebApi.ServicesStub;
using Kingdee.BOS.ServiceHelper;

namespace CYSD.DDL.K3.BOS.WebApi.ServicesStub
{

    /// <summary>
    /// 创建时间：2022-5-22
    /// 作用：SQL语句查询
    /// </summary>
    [Description("SQL语句查询")]
    [HotUpdate]
    public class FbillWebApiService : AbstractWebApiBusinessService
    {
        public FbillWebApiService(KDServiceContext context) : base(context) { }

        /// <summary>      
        /// /// 执行SQL并返回查询结果 
        /// /// </summary>    
        /// /// <param name="sql">SQL脚本</param>  
        /// /// <returns>返回字典集合</returns> 
        public object ExecuteDynamicObject(string sql)
        {
            return (object)DBServiceHelper.ExecuteDynamicObject(this.KDContext.Session.AppContext, sql);
        }

        /// <summary>  
        /// /// 执行SQL并返回查询结果 
        /// /// </summary>     
        /// /// <param name="sql">SQL脚本</param>  
        /// /// <returns>返回DataSet</returns>   
        public object ExecuteDataSet(string sql)
        {
            return DBServiceHelper.ExecuteDataSet(this.KDContext.Session.AppContext, sql);
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql)
        {
            return DBServiceHelper.Execute(this.KDContext.Session.AppContext, sql);
        }

    }
}