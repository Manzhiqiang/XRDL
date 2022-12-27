using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
	/// <summary>
	///模型 
	/// </summary>
	public class RKHWModel
    {
        public warehouseEntry WarehouseEntry { get; set; }
    }
    public class warehouseEntry
    {
        public int FEntryID { get; set; }
        public string FName { get; set; }
        public string FModle { get; set; }
        public string FUnit { get; set; }
        public float FQty { get; set; }
        public float FPrice { get; set; }
        public float FAmount { get; set; }
        public string FRange { get; set; }
        public string FItem { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string ProNumber { get; set; }

        public string we_c_uid { get; set; }
        public string we_plannumber { get; set; }//计划号
        public string wh_ow_cnumber { get; set; }//合同号
        public string we_tagnumber { get; set; }//位号
        public string we_containerNo { get; set; }//箱号
        public String Wh_in_number { get; set; }
    }
}
