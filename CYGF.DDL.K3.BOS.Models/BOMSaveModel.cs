using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    /// <summary>
    /// BOM新增模型 
    /// </summary>
    public class BOMSaveModel
    {
        public List<PlmBOMParms> PlmMaterialParms { get; set; }
    }
    public class PlmBOMParms
    {
        public BOMModel Model { get; set; }
    }
    public class BOMModel
    {
        public string Festimate { get; set; }//传1时未修改，传入2时，BOM生版
        public FPLMBOMNumber FMATERIALID { get; set; }//父项物料编码
        public FPLMBOMNumber FUNITID { get; set; }//父项物料单位

        public List<PLMBOMFTreeEntitys> FTreeEntity { get; set; }
        

    }

    public class PLMBOMFTreeEntitys
    {
        public FPLMBOMNumber FMATERIALIDCHILD { get; set; }
        public string FMATERIALTYPE { get; set; }//子项类型
        public FPLMBOMNumber FCHILDUNITID { get; set; }//子项单位
        public FPLMBOMNumber FDOSAGETYPE { get; set; }//用量类型
        public decimal FNUMERATOR { get; set; }//分子
        public decimal FDENOMINATOR { get; set; }//分母
        public bool FISSkip { get; set; }//跳层
    }
   
    public class FPLMBOMNumber
    {
        public string FNumber { get; set; }

    }


}