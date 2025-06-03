using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class ShippingLine : ICloneable
    {
        public object id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string code { get; set; }
        public string source { get; set; }
        public object phone { get; set; }
        public object carrier_identifier { get; set; }
        public List<object> tax_lines { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
