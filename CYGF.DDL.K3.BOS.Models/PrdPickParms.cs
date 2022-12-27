using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    public class PrdPickParms
    {
        public PrdPickParms()
        {
            Entrys = new List<Entry>();
        }
        public string FTranType;//WMS业务类型
        public string FOrg;//组织代码
        public string FDeptNumber;//部门代码
        public string FDate;//入库日期
        public string FUserName;
        public string FWMSBillNo;//WMS入库单号
        public string FSrcBillNo;//生产订单单据编号
        public List<Entry> Entrys;
    }

   
}
