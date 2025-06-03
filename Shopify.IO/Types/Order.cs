using System;
using System.Collections.Generic;
namespace Shopify.IO.Types
{
    public class Order : ICloneable
    {
        public object id { get; set; }
        public string email { get; set; }
        public object closed_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int number { get; set; }
        public string note { get; set; }
        public string token { get; set; }
        public string gateway { get; set; }
        public bool test { get; set; }
        public string total_price { get; set; }
        public string subtotal_price { get; set; }
        public int total_weight { get; set; }
        public string total_tax { get; set; }
        public bool taxes_included { get; set; }
        public string currency { get; set; }
        public string financial_status { get; set; }
        public bool confirmed { get; set; }
        public string total_discounts { get; set; }
        public string total_line_items_price { get; set; }
        public string cart_token { get; set; }
        public bool buyer_accepts_marketing { get; set; }
        public string name { get; set; }
        public string referring_site { get; set; }
        public string landing_site { get; set; }
        public object cancelled_at { get; set; }
        public object cancel_reason { get; set; }
        public string total_price_usd { get; set; }
        public string checkout_token { get; set; }
        public object reference { get; set; }
        public object user_id { get; set; }
        public object location_id { get; set; }
        public object source_identifier { get; set; }
        public object source_url { get; set; }
        public string processed_at { get; set; }
        public object device_id { get; set; }
        public string browser_ip { get; set; }
        public object landing_site_ref { get; set; }
        public int order_number { get; set; }
        public List<object> discount_codes { get; set; }
        public List<object> note_attributes { get; set; }
        public List<string> payment_gateway_names { get; set; }
        public string processing_method { get; set; }
        public object checkout_id { get; set; }
        public string source_name { get; set; }
        public object fulfillment_status { get; set; }
        public List<object> tax_lines { get; set; }
        public string tags { get; set; }
        public string contact_email { get; set; }
        public string order_status_url { get; set; }
        public List<LineItem> line_items { get; set; }
        public List<ShippingLine> shipping_lines { get; set; }
        public BillingAddress billing_address { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public List<object> fulfillments { get; set; }
        public ClientDetails client_details { get; set; }
        public List<object> refunds { get; set; }
        public Customer customer { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
