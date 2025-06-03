using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Shopify.IO.Types
{
    public class Product : ICloneable
    {
        public Product()
        {

        }
        public Product(string title, string body_html, string vendor, string product_type, bool published, List<Variant> variants, List<Option> options, string tags)
        {
            this.title = title;
            this.body_html = body_html;
            this.vendor = vendor;
            this.product_type = product_type;
            this.published = published;
            this.variants = variants;
            this.options = options;
            this.tags = tags;
        }

        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string vendor { get; set; }
        public string product_type { get; set; }
        public DateTime created_at { get; set; }
        public string handle { get; set; }
        public string updated_at { get; set; }
        public string published_at { get; set; }
        public string template_suffix { get; set; }
        public string published_scope { get; set; }
        public string tags { get; set; }
        public List<Variant> variants { get; set; }
        public List<Option> options { get; set; }
        public List<Image> images { get; set; }
        public Image image { get; set; }
        public bool published { get; set; }
        public List<Metafield> metafields { get; set; }

        //read only properties.

        public string sku
        {
            get
            {
                return variants[0].sku;
            }
        }

        //public string preview_url { get { return "https://" +  + handle; } }

        //public string edit_url { get { return "https://montania-fashion.myshopify.com/admin/products/" + id; } }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public enum ProductEndPointTypes
    {
        ids, //A comma-separated list of product ids
        limit, //Amount of results (default: 50) (maximum: 250)
        page, //Page to show (default: 1)
        since_id, //Restrict results to after the specified ID
        title, //Filter by product title
        vendor, //Filter by product vendor
        handle, //Filter by product handle
        product_type, //Filter by product type
        collection_id, //Filter by collection id
        created_at_min, //Show products created after date (format: 2008-12-31 03:00)
        created_at_max, //Show products created before date (format: 2008-12-31 03:00)
        updated_at_min, //Show products last updated after date (format: 2008-12-31 03:00)
        updated_at_max, //Show products last updated before date (format: 2008-12-31 03:00)
        published_at_min, //Show products published after date (format: 2008-12-31 03:00)
        published_at_max, //Show products published before date (format: 2008-12-31 03:00)
        published_status //published - Show only published products 
                         //unpublished - Show only unpublished products any - Show all products (default)
    }

    public class ProductEndPoint
    {
        public ProductEndPointTypes EndPointType { get; set; }

        public string EndPointValue { get; set; }
    }
}
