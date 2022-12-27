using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    public class RKDServiceModels
    {
        public List<ShipmentDetailS> ShipmentDetailS { get; set; }
        public RKDServiceModels()
        {
            ShipmentDetailS = new List<ShipmentDetailS>();
        }

    }
    public class ShipmentDetailS
    {
        public string FNumber { get; set; }
        public string FName { get; set; }
        /// <summary>
        /// 口径
        /// </summary>
        public string F_PYLY_KJ { get; set; }
        public string FLOT { get; set; }
        public string F_PYLY__Xshth { get; set; }
    }
}

