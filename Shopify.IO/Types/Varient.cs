using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class Variant : ICloneable
    {

        public Variant(
            string title, 
            string price, 
            string sku,
            int position,
            string grams,
            string inventory_policy, 
            string compare_at_price, 
            string fulfillment_service,
            string inventory_management, 
            string option1,
            string option2,
            string option3,
            bool requires_shipping, 
            bool taxable,
            string barcode, 
            int inventory_quantity
            )
        {
            this.title = title;
            this.price = price;
            this.sku = sku;
            this.position = position;
            this.grams = grams;
            this.inventory_policy = inventory_policy;
            this.compare_at_price = compare_at_price;
            this.fulfillment_service = fulfillment_service;
            this.inventory_management = inventory_management;
            this.option1 = option1;
            this.option2 = option2;
            this.option3 = option3;
            this.requires_shipping = requires_shipping;
            this.taxable = taxable;
            this.barcode = barcode;
            this.inventory_quantity = inventory_quantity;
        }

        public long id { get; set; }
        public long product_id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public int position { get; set; }
        public string grams { get; set; }
        public string inventory_policy { get; set; }
        public string compare_at_price { get; set; }
        public string fulfillment_service { get; set; }
        public string inventory_management { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public bool requires_shipping { get; set; }
        public bool taxable { get; set; }
        public string barcode { get; set; }
        public int inventory_quantity { get; set; }
        public string old_inventory_quantity { get; set; }
        public long? image_id { get; set; }
        public double weight { get; set; }
        public string weight_unit { get; set; }

        public Int64 inventory_item_id { get; set; }

        public InventoryLevel inventory_Level { get; set; }

        public errors LastError { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}