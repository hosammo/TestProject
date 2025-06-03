using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class Option : ICloneable
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string name { get; set; }
        public int position { get; set; }
        public List<string> values { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
