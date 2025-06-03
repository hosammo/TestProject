using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class Metafield : ICloneable
    {
        public long id { get; set; }
        public string @namespace { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
        public object description { get; set; }
        public long owner_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string owner_resource { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public enum MetafieldEndPointTypes
    {
        limit, //Amount of results (default: 50) (maximum: 250)
        since_id, //Restrict results to after the specified ID
        created_at_min, //Show metafields created after date (format: 2008-12-31 03:00)
        created_at_max, //Show metafields created before date (format: 2008-12-31 03:00)
        updated_at_min, //Show metafields last updated after date (format: 2008-12-31 03:00)
        updated_at_max, //Show metafields last updated before date (format: 2008-12-31 03:00)
        namepace, //Show metafields with given namespace
        key, //Show metafields with given key
        value_type, //string - Show only metafields with string value types integer - Show only metafields with integer value types
        fields //comma-separated list of fields to include in the response    }
    }
    public class MetafieldEndPoint
    {
        public MetafieldEndPointTypes EndPointType { get; set; }

        public string EntPointValue { get; set; }
    }
}
