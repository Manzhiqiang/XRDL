using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRDL.DDL.K3.BOS.Models
{
    public class K3BaseMessageModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 成功ID
        /// </summary>
        public Int64 FID { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string FNumber { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }
}
