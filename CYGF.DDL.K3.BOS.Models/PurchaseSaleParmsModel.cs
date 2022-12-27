using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
	/// <summary>
	/// MES物料订单模型 
	/// </summary>
	public class PurchaseSaleParmsModel
    {



        //PurchaseSaleParmsModel Parms = JsonConvert.DeserializeObject<PurchaseSaleParmsModel>(Parameters);
        /// <summary>
        /// 物料编码
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string status { get; set; }
        
        //{"data":"A122010007","message":"新增成功","status":"success"}
}
}
