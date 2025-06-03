using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class InventoryLevel
    {
        public long inventory_item_id { get; set; }
        public long location_id { get; set; }
        public int available { get; set; }
        public DateTime updated_at { get; set; }
        public string admin_graphql_api_id { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }



}
