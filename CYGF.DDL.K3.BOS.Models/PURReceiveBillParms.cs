using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    public class PURReceiveBillParms
    {
        public PURReceiveBillParms()
        {
            Entry = new List<PURReceiveBillEntry>();
        }

        public string BillId { get; set; }
        /// <summary>
        /// 收料通知单编号
        /// </summary>
        public string BillNumber { get; set; }

        public List<PURReceiveBillEntry> Entry { get; set; }

        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
    }

    public class PURReceiveBillEntry
    {
        public PURReceiveBillEntry()
        {
            SubEntry = new List<PURReceiveBillSubEntry>();
        }
        public long EntryId { get; set; }
        public int Seq { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNumber { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string StockNumber { get; set; }

        /// <summary>
        /// 仓位编码
        /// </summary>
        public string StockLocNumber { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string FLot { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public double Qty { get; set; }

        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Remark4 { get; set; }
        public string Remark5 { get; set; }

        public List<PURReceiveBillSubEntry> SubEntry { get; set; }
    }

    public class PURReceiveBillSubEntry
    {
        public string EntryId;//分录内码
        public string SerialNo;//序列号
        public string XH;//箱号
    }
}
