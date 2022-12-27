using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
	/// <summary>
	/// MES物料订单模型 
	/// </summary>
	public class MesDeleteBOMModel
	{
        
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string PartCode { get; set; }
        public string SetItemValueByID { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string User { get; set; }


    }
}
