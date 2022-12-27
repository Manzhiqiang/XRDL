using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
	/// <summary>
	///模型 
	/// </summary>
	public class RKDModel
    {
        public string username = "13368251422";//{ get; set; }
        public string password ="123456";//{ get; set; }

        public wareEntry Warehouse { get; set; }
        /// <summary>
        /// 发运单分录
        /// </summary>
        public List<WarehouseDetail> WarehouseDetail { get; set; }
    }
    public class wareEntry
    {
        /// <summary>
        /// 存放地编码
        /// </summary>
        public int wh_sl_coding { get; set; }
        public String wh_en_cnumber { get; set; }
        public String wh_en_number { get; set; }
        public String wh_ow_cnumber { get; set; }
        /// <summary>
        /// 入库单ID
        /// </summary>
        public int wh_en_unid { get; set; }
        public int Wh_is_ow_cf { get; set; }
        public int wh_ow_cf_status { get; set; }
        public String wh_fcfhid { get; set; }
        public decimal wh_in_money { get; set; }
        public int wh_in_gtype { get; set; }
        public String wh_en_remark { get; set; }
        public String wh_in_number { get; set; }

        public String wh_delivery_date { get; set; }
    }
    public class WarehouseDetail
    {
        public String whd_gname { get; set; }
        public int whd_in_piece { get; set; }
        public decimal whd_in_quantity { get; set; }
        public int whd_packtype { get; set; }

    }
}
