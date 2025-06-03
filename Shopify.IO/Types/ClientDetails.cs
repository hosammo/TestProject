using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class ClientDetails : ICloneable
    {
        public string browser_ip { get; set; }
        public string accept_language { get; set; }
        public string user_agent { get; set; }
        public string session_hash { get; set; }
        public int browser_width { get; set; }
        public int browser_height { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
