using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class Location : ICloneable
    {
        public Location(
            long id,
            string name,
            string address1,
            string address2,
            string city,
            string zip,
            string province,
            string country,
            string phone,
            string created_at,
            string updated_at,
            string country_code,
            string country_name,
            string province_code,
            bool legacy,
            bool active,
            string admin_graphql_api_id,
            string localized_country_name,
            string localized_province_name
        )
        {
            this.id = id;
            this.name = name;
            this.address1 = address1;
            this.address2 = address2;
            this.city = city;
            this.zip = zip;
            this.province = province;
            this.country = country;
            this.phone = phone;
            this.created_at = created_at;
            this.updated_at = updated_at;
            this.country_code = country_code;
            this.country_name = country_name;
            this.province_code = province_code;
            this.legacy = legacy;
            this.active = active;
            this.admin_graphql_api_id = admin_graphql_api_id;
            this.localized_country_name = localized_country_name;
            this.localized_province_name = localized_province_name;
        }

        public long id { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string province_code { get; set; }
        public bool legacy { get; set; }
        public bool active { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string localized_country_name { get; set; }
        public string localized_province_name { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}