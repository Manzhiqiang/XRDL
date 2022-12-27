using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYSD.DDL.K3.BOS.Models
{
    public class PlmMaterial
    {
        public List<PlmMaterialParms> PlmMaterialParms { get; set; }
     

    }
    public class PlmMaterialParms
    {
        public ModelS Model { get; set; }
      
    }
    public class ModelS
    {
        public string FNumber { get; set; }//编码
        public string FName { get; set; }//名称
        public string FSpecification { get; set; }//规格型号
        public string FDescription { get; set; }//描述
        public FNum F_VKPG_Assistant { get; set; }//产成品类别
        public string FMnemonicCode { get; set; }//助记码

        public SubHeadEntity SubHeadEntity { get; set; }//基本
        public SubHeadEntity1 SubHeadEntity1 { get; set; }//库存
        public SubHeadEntity5 SubHeadEntity5 { get; set; }//生产
    }
    public class SubHeadEntity
    {
        public FNum FCategoryID { get; set; }//存货类别
        public FNum FBaseUnitId { get; set; }//基本单位
        public FNum FMaterialGroup { get; set; }//物料分组
        public string FErpClsID { get; set; }//物料属性

    }
    public class SubHeadEntity1
    {
        public FNum FStockId { get; set; }//仓库
        public bool FIsBatchManage { get; set; }//启用批号管理
        public bool FIsSNManage { get; set; }//库存管理,是否启用序列号

    }
    public class SubHeadEntity5
    {
        public bool FIsKitting { get; set; }//是否关键件
        public bool FIsCompleteSet { get; set; }//是否齐套件

    }
    public class  FNum
    {
        public string FNumber { get; set; }

    }

}
