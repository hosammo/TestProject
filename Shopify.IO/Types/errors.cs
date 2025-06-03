using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class errors : ICloneable
    {
        public List<string> title { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
