using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Types
{
    public class RootObject : ICloneable
    {
        public List<Product> products { get; set; }
        public List<Location> locations { get; set; }
        public List<Order> orders { get; set; }
        public int count { get; set; }
        public Variant variant { get; set; }
        public Product product { get; set; }
        public errors errors { get; set; }
        public List<Metafield> metafields { get; set; }
        public Metafield metafield { get; set; }
        public InventoryLevel inventory_level { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
 }
