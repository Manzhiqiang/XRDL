using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingdee.BOS.Core.List;
using Kingdee.BOS.Orm;

namespace CYSD.DDL.K3.BOS.Models
{
    public class ConvertOption
    {
        public ConvertOption()
        {
            this.IsSave = true;
        }

        public ListSelectedRow[] BizSelectRows { get; set; }

        public string ConvertRuleKey { get; set; }

        public bool IsAudit { get; set; }

        public bool IsDraft { get; set; }
        public bool IsSubmit { get; set; }

        public bool IsSave { get; set; }

        public OperateOption Option { get; set; }

        public string SourceFormId { get; set; }

        public string TargetFormId { get; set; }

        public string TargetBillTypeId { get; set; }
        public long TargetOrgId { get; set; }
        public string SourceBillTypeId { get; set; }
    }
}
