using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRDL.DDL.K3.BOS.Tools
{
	public class MesConfigHelper : BaseConfigHelper
	{
		public string shipmentInterfaceUrl;//发运单新增、修改、查询地址
		public string productionOrderUrl;//生产订单下发
		public string mesUserName;
		public string mesUserPwd;

		public MesConfigHelper() : base(AppDomain.CurrentDomain.BaseDirectory + "Config\\MesConfig.config")
		{
			this.shipmentInterfaceUrl = _ShipmentInterfaceUrl();
			this.productionOrderUrl = _ProductionOrderUrl();
			this.mesUserName = _MesUserName();
			this.mesUserPwd = _MesUserPwd();
		}

		/// <summary>
		/// 发运单地址
		/// </summary>
		/// <returns></returns>
		private string _ShipmentInterfaceUrl()
		{
			return this.GetAppSettings("ShipmentInterfaceUrl");
		}

		/// <summary>
		/// 生产订单地址
		/// </summary>
		/// <returns></returns>
		private string _ProductionOrderUrl()
		{
			return this.GetAppSettings("ProductionOrderUrl");
		}

		/// <summary>
		/// MES用户名
		/// </summary>
		/// <returns></returns>
		private string _MesUserName()
		{
			return this.GetAppSettings("MesUserName");
		}

		/// <summary>
		/// MES密码
		/// </summary>
		/// <returns></returns>
		private string _MesUserPwd()
		{
			return this.GetAppSettings("MesUserPwd");
		}
	}
}
