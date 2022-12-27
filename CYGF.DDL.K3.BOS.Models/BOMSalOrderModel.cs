using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    /// <summary>
    /// BOM新增模型 
    /// </summary>
    public class BOMSalOrderModel
    {
        public Model model { get; set; }
    }
    public class Model
    {
        public List<FTreeEntity> FTreeEntity { get; set; }
    }
    public class FTreeEntity
    {
        public FMATERIALIDCHILD FMATERIALIDCHILD { get; set; }
    }
    public class FMATERIALIDCHILD
    {
        public string FNumber { get; set; }
    }
}