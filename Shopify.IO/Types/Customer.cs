using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class Customer : ICloneable
    {
        public object id { get; set; }
        public string email { get; set; }
        public bool accepts_marketing { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int orders_count { get; set; }
        public string state { get; set; }
        public string total_spent { get; set; }
        public object last_order_id { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public string tags { get; set; }
        public string last_order_name { get; set; }
        public DefaultAddress default_address { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
