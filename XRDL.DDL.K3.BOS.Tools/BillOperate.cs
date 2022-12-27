using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS;
using Kingdee.BOS.Util;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.FormElement;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.Interaction;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Core.SqlBuilder;
using System.ComponentModel;
using Kingdee.BOS.Core.Bill.PlugIn;

namespace XRDL.DDL.K3.BOS.Tools
{
    public class BillOperate
    {
        /// <summary>
        /// 获取业务对象
        /// </summary>
        /// <param name="formId,fiterField,filterValue,fieldkeys"></param>
        public static DynamicObjectCollection GetQueryDatas(Context ctx, string formId, string fiter, string[] fieldkeys)
        {
            QueryBuilderParemeter paramCatalog = new QueryBuilderParemeter()
            {
                FormId = formId,//取数的业务对象
                FilterClauseWihtKey = fiter,//过滤条件，通过业务对象的字段Key拼装过滤条件
                SelectItems = SelectorItemInfo.CreateItems(fieldkeys),//要筛选的字段【业务对象的字段Key】，可以多个，如果要取主键，使用主键名
            };

            DynamicObjectCollection dyDatas = Kingdee.BOS.ServiceHelper.QueryServiceHelper.GetDynamicObjectCollection(ctx, paramCatalog);
            return dyDatas;
        }
    }
}
