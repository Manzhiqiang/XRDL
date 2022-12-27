using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    /// <summary>
    /// BOM新增模型 
    /// </summary>
    public class PLMBOMSaveModel
    {
        public jsons json { get; set; }
    }
    public class jsons
    {
        public string FFormId { get; set; }
        public string FDBId { get; set; }
        public string FUserName { get; set; }
        public string FPassWord { get; set; }
        public string FSysName { get; set; }
        public List<FDatass> FData { get; set; }
    }

    public class FDatass
    {
        public string FName { get; set; }//
        public string FNumber { get; set; }//
        public FFNum FMATERIALID { get; set; }//
        public string FISPROCESS { get; set; }//

        public List<FTreeEntitys> FTreeEntity { get; set; }//

        public List<FBopEntitys> FBopEntity { get; set; }

    }

    public class FTreeEntitys
    {
        public string FFIXSCRAPQTY { get; set; }//
        public string FEFFECTDATE { get; set; }//
        public string FEXPIREDATE { get; set; }//
        public string FISSUETYPE { get; set; }//
        public FFNum FMATERIALIDCHILD { get; set; }
        public string FSCRAPRATE { get; set; }
        public string FSupplyType { get; set; }
    }
    public class FBopEntitys
    {
        public FFNum FBopMaterialId { get; set; }
        public string FBopNumerator { get; set; }
        public string FBopDenominator { get; set; }
    }
    public class FFNum
    {
        public string Number { get; set; }

    }


}