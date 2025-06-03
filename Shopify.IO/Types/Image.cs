using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class Image : ICloneable
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public int position { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string src { get; set; }

        public string attachment { get; set; }
        public List<object> variant_ids { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}


