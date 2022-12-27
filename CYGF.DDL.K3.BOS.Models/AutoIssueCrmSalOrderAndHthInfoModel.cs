using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYSD.DDL.K3.BOS.Models
{
    /// <summary>
    /// CRM 下发销售订单主表模型
    /// </summary>
    public class AutoIssueCrmSalOrderInfoModel
    {
        public string FBillNo { get; set; }
        /// <summary>
        /// CRM单据号
        /// </summary>
        public string danjuhao { get; set; }

        public string shengchanhuifudejiaohuoriqi { get; set; }

        public string yingshouyue { get; set; }

        public string shengchanhuifujiaohuohuixieshijian { get; set; }
    }

    /// <summary>
    /// CRM 下发销售订单子表模型
    /// </summary>
    public class AutoIssueCrmSalOrderEntryInfoModel
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string danjuhao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string hangyewuid { get; set; }

        /// <summary>
        /// 是否入库【产品明细】
        /// </summary>
        public string shifouruku { get; set; }

        /// <summary>
        /// 入库时间【产品明细】
        /// </summary>
        public string rukushijian { get; set; }

        /// <summary>
        /// 是否出库【产品明细】
        /// </summary>
        public string shifouchuku { get; set; }
        /// <summary>
        /// 出库时间【产品明细】
        /// </summary>
        public string chukushijian { get; set; }
        /// <summary>
        /// 是否开票【产品明细】
        /// </summary>
        public string shifoukaipiao { get; set; }
        /// <summary>
        /// 开票日期【产品明细】
        /// </summary>
        public string kaipiaoriqi { get; set; }
        /// <summary>
        /// 表号【产品明细】
        /// </summary>
        public string biaohao { get; set; }

        /// <summary>
        /// 发票号【产品明细】
        /// </summary>
        public string fapiaohao { get; set; }
        /// <summary>
        /// 发票金额【产品明细】
        /// </summary>
        public string fapiaojine { get; set; }
        /// <summary>
        /// 工序状态【产品明细】
        /// </summary>
        public string gongxuzhuangtai { get; set; }
        /// <summary>
        /// 存货类别【产品明细】
        /// </summary>
        public string cunhuoleibie { get; set; }
        /// <summary>
        /// 生产车间【产品明细】
        /// </summary>
        public string shengchanchejian { get; set; }
    }

    /// <summary>
    ///  CRM 下发销售订单 核销信息
    /// </summary>
    public class AutoIssueCrmSalOrderHXInfoModel
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string danjuhao { get; set; }

        /// <summary>
        /// 核销类型【核销明细】
        /// </summary>
        public string hexiaoleixing { get; set; }

        ///// <summary>
        ///// 序号1
        ///// </summary>
        //public string xuhao1 { get; set; }

        /// <summary>
        /// 核销ID
        /// </summary>
        public string hexiaoid { get; set; }

        /// <summary>
        /// 核销金额【核销明细】
        /// </summary>
        public string hexiaojine { get; set; }

        /// <summary>
        /// 核销日期【核销明细】
        /// </summary>
        public string hexiaoriqi { get; set; }
       
    }

    /// <summary>
    ///  CRM 下发销售订单综合数据
    /// </summary>
    public class AutoIssueCrmZHSalOrderInfoModel {
        /// <summary>
        ///  CRM 下发销售订单主表模型
        /// </summary>
        public AutoIssueCrmSalOrderInfoModel zhubiao { get; set; }

        /// <summary>
        /// CRM 下发销售订单子表模型
        /// </summary>
        public  List<AutoIssueCrmSalOrderEntryInfoModel> mingxi2{get;set;}

        /// <summary>
        ///  CRM 下发销售订单 核销信息
        /// </summary>
        public List<AutoIssueCrmSalOrderHXInfoModel> mingxi4 { get; set; }
    }

    /// <summary>
    /// 合同号信息 下发回款信息
    /// </summary>
    public class AutoIssueCrmZHHTHInfoModel
    {
        public string FBillNo { get; set; }

        /// <summary>
        /// 内部合同号
        /// </summary>
        public string neibuhetonghao { get; set; }

        /// <summary>
        /// 预收金额
        /// </summary>
        public string yushoujine { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<AutoIssueCrmHTHInfoModel> mingxi1 { get; set; }
    }

    /// <summary>
    /// 合同号信息 下发回款信息
    /// </summary>
    public class AutoIssueCrmHTHInfoModel
    {
         /// <summary>
         /// 回款ID
         /// </summary>
        public string huikuanid { get; set; }

        /// <summary>
        /// 回款日期
        /// </summary>
        public string huikuanriqi { get; set; }

        /// <summary>
        /// 回款金额
        /// </summary>
        public string huikuanjine { get; set; }
    }
}
