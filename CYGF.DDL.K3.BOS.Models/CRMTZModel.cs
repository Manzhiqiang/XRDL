using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    public class CRMTZModel
    {

        public List<mingxi2> mingxi22 { get; set; }
        public List<mingxi3> mingxi33 { get; set; }
        public List<mingxi4> mingxi44 { get; set; }
        public List<zhubiao> zhubiaos { get; set; }
        public class mingxi2
        {
            public string hangyewuid { get; set; }
            public string shifouruku { get; set; }
            public string rukushijian { get; set; }
            public string shifouchuku { get; set; }
            public string chukushijian { get; set; }
            public string shifoukaipiao { get; set; }
            public string kaipiaoriqi { get; set; }
            public string biaohao { get; set; }
            public string fapiaohao { get; set; }
            public string fapiaojine { get; set; }
        }
     
        public class mingxi3
        { 
            public string fukuanfangshi { get; set; }
            public string huikuanjine { get; set; }
            public string huikuanshijian { get; set; }
            public string xuhao2 { get; set; }
        }

        public class mingxi4
        {    
            public string hexiaoleixing { get; set; }
            public string xuhao1 { get; set; }
            public string hexiaojine { get; set; }
            public string hexiaoriqi { get; set; }
        }
        
        public class zhubiao
        {  
            public string danjuhao { get; set; }
            public string shengchanhuifudejiaohuoriqi { get; set; }
            public string yingshouyue { get; set; }
            public string yushoujine { get; set; }
            public string shengchanhuifujiaohuohuixieshijian { get; set; }
        }
    }
}
