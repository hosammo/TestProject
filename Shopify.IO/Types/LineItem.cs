using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class LineItem : ICloneable
    {
        public object id { get; set; }
        public object variant_id { get; set; }
        public string title { get; set; }
        public int quantity { get; set; }
        public string price { get; set; }
        public int grams { get; set; }
        public string sku { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public string fulfillment_service { get; set; }
        public object product_id { get; set; }
        public bool requires_shipping { get; set; }
        public bool taxable { get; set; }
        public bool gift_card { get; set; }
        public string name { get; set; }
        public string variant_inventory_management { get; set; }
        public List<object> properties { get; set; }
        public bool product_exists { get; set; }
        public int fulfillable_quantity { get; set; }
        public string total_discount { get; set; }
        public object fulfillment_status { get; set; }
        public List<object> tax_lines { get; set; }
        public OriginLocation origin_location { get; set; }
        public DestinationLocation destination_location { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
