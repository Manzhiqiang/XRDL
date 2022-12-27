using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Tools
{
    public class WmsConfigHelper : BaseConfigHelper
    {
        public WmsConfigHelper() : base(AppDomain.CurrentDomain.BaseDirectory + "Config\\WmsConfig.config")
        {
            this.MaterialUrl = _MaterialUrl();
            this.PrdPickUrl = _PrdPickUrl();
            this.PrdMoUrl = _PrdMoUrl();
            this.StatusQueryUrl = _StatusQueryUrl();
            this.StkStockUrl = _StkStockUrl();
        }
        public string MaterialUrl;//物料审核推送WMS地址
        public string PrdMoUrl;//生产投料推送WMS地址
        public string StatusQueryUrl;//单据状态查询请求地址
        public string OutInStockUrl;//出入库请求地址
        public string StkStockUrl;//出库校验请求地址
        public string PrdPickUrl;//装配投料推送WMS地址

        private string _MaterialUrl()
        {
            return this.GetAppSettings("MaterialUrl");
        }

        private string _PrdPickUrl()
        {
            return this.GetAppSettings("PrdPickUrl");
        }

        private string _PrdMoUrl()
        {
            return this.GetAppSettings("PrdMoUrl");
        }

        private string _StatusQueryUrl()
        {
            return this.GetAppSettings("StatusQueryUrl");
        }

        private string _OutInStockUrl()
        {
            return this.GetAppSettings("OutInStockUrl");
        }
        private string _StkStockUrl()
        {
            return this.GetAppSettings("StkStockUrl");
        }
    }
}
