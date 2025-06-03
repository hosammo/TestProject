using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyHelper.IO
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public override string ToString()
        {
            return BranchName;
        }
    }

    // Class representing a masking rule.
    public class StockMaskingRule
    {
        public int RangeFrom { get; set; }
        public int? RangeTo { get; set; }
        public int? MaskedStock { get; set; }
    }

    public class LocationConfig
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> LocalDBBranch { get; set; } // CSV list of branch IDs.

        public List<StockMaskingRule> MaskingRules { get; set; }
    }

}
