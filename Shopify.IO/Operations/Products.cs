using Newtonsoft.Json;
using Shopify.IO.helpers;
using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.Linq;
//using ShopifySharp;
namespace Shopify.IO.Operations
{
    public class Products
    {
        public errors LastError { get; set; }
        private APIAccess CurrentStoreAPIAccess;
        //private ShopifySharp.ProductService service; 

        public Products(APIAccess ApiAccess)
        {
            // TODO: Complete member initialization
            this.CurrentStoreAPIAccess = ApiAccess;
//            service = new ProductService(CurrentSroreAPIAccess.APIUrl(),CurrentSroreAPIAccess.Password); 
        }

        public List<Shopify.IO.Types.Product> GetList(params string[] requiredFields)
        {
            string fields = "fields=";
            if (requiredFields.Length > 0)
            {
                foreach (string s in requiredFields)
                {
                    fields += s;
                    fields += ",";
                }
            }
            //empty list will hold all products returned from shopify.
            List<Shopify.IO.Types.Product> p = new List<Shopify.IO.Types.Product>();
            CustomeResoponce cr;
        //int pageCount = this.Pages;

        //for (int i = 1; i <= pageCount; i++)
        //{
        //get json from Shopify.
        restart:
            string tmpURL;
            if (requiredFields.Length > 0)
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products.json?limit=50&" + fields;
            }
            else
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products.json?limit=50";
            }

            cr = CurrentStoreAPIAccess.GetURL(tmpURL);

            if (cr.result.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Shopify.IO.Types.Product np in obj.products)
            {
                p.Add(np);
            }

            var link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);


            if (link != null && link.Any())
            {
                if (link.FirstOrDefault().Value.ToString().Contains("next"))
                {
                    var nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);

                    while (nextURL.FirstOrDefault() != null)
                    {
                    restart2: cr = CurrentStoreAPIAccess.GetURL(nextURL.FirstOrDefault().ToString().Split(';')[0].Replace("<", "").Replace(">", "").Replace("Link=", "").Replace("link=", ""));

                        if (cr.result.Contains("Exceeded 2 calls per second for api client."))
                        {
                            System.Threading.Thread.Sleep(1000);
                            goto restart2;
                        }

                        //de-serialize the JSON string to Dictionary
                        obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

                        foreach (Shopify.IO.Types.Product np in obj.products)
                        {
                            p.Add(np);
                        }

                        link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);

                        nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                    }
                }        
            }
            return p;
        }

        public Shopify.IO.Types.Product GetProduct(long product_id, params string[] requiredFields)
        {
            CustomeResoponce cr;

        restart:
            string fields = "fields=";
            if (requiredFields.Length > 0)
            {
                foreach (string s in requiredFields)
                {
                    fields += s;
                    fields += ",";
                }

            }
            fields = fields.Remove(fields.Length - 1, 1);


            //get json from Shopify.
            string tmpURL;
            if (requiredFields.Length > 0)
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product_id.ToString() + ".json?" + fields;
            }
            else
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product_id.ToString() + ".json";
            }

            cr = CurrentStoreAPIAccess.GetURL(tmpURL);

            if (cr.result.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            if (obj.product == null)
                LastError = obj.errors;

            return obj.product;

        }

        public List<Shopify.IO.Types.Product> GetListWithEndPoint(params ProductEndPoint[] EndPoints)
        {
            string EndPoints_str = "";
            if (EndPoints.Length > 0)
            {
                foreach (ProductEndPoint ep in EndPoints)
                {
                    EndPoints_str += ep.EndPointType.ToString() + "=" + ep.EndPointValue.ToString();
                    EndPoints_str += ",";
                }
            }
            EndPoints_str = EndPoints_str.Remove(EndPoints_str.Length - 1, 1);
            //empty list will hold all products returned from shopify.
            List<Shopify.IO.Types.Product> p = new List<Shopify.IO.Types.Product>();
            CustomeResoponce cr;


        restart:
            //get json from Shopify.
            string tmpURL;
            if (EndPoints.Length > 0)
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products.json?limit=50&" + EndPoints_str;
            }
            else
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products.json?limit=50";
            }

            cr = CurrentStoreAPIAccess.GetURL(tmpURL);

            if (cr.result.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Product np in obj.products)
            {
                p.Add(np);
            }

            var link = from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b;


            if (link.FirstOrDefault() != null && link.FirstOrDefault().Value.ToString().Contains("next"))
            {
                var nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);

                while (nextURL.FirstOrDefault() != null)
                {
                restart2: cr = CurrentStoreAPIAccess.GetURL(nextURL.FirstOrDefault().ToString().Split(';')[0].Replace("<", "").Replace(">", "").Replace("Link=", ""));

                    if (cr.result.Contains("Exceeded 2 calls per second for api client."))
                    {
                        System.Threading.Thread.Sleep(1000);
                        goto restart2;
                    }

                    //de-serialize the JSON string to Dictionary
                    obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

                    foreach (Shopify.IO.Types.Product np in obj.products)
                    {
                        p.Add(np);
                    }

                    link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);

                    nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                }
            }
            return p;
        }

        public List<Variant> ZeroQtyVariants
        {
            get
            {
                List<Shopify.IO.Types.Product> p = this.GetList();
                List<Variant> v0 = new List<Variant>();

                foreach (Shopify.IO.Types.Product pr in p)
                {
                    foreach (Variant vr in pr.variants)
                    {
                        if (vr.inventory_quantity <= 0)
                        {
                            v0.Add(vr);
                        }
                    }
                }
                return v0;
            }


        }

        public List<Variant> ZeroWeightVariants
        {
            get
            {
                List<Shopify.IO.Types.Product> p = this.GetList(new string[] { "id", "variants" });
                List<Variant> v0 = new List<Variant>();

                foreach (Shopify.IO.Types.Product pr in p)
                {
                    foreach (Variant vr in pr.variants)
                    {
                        if ((double)vr.weight <= 0)
                        {
                            v0.Add(vr);
                        }
                    }
                }
                return v0;
            }
        }

        public List<Variant> ZeroPriceVariants
        {
            get
            {
                List<Shopify.IO.Types.Product> p = this.GetList(new string[] { "id", "variants" });
                List<Variant> v0 = new List<Variant>();

                foreach (Shopify.IO.Types.Product pr in p)
                {
                    foreach (Variant vr in pr.variants)
                    {
                        if (vr.price == "0" | vr.price == "0.0" | vr.price == "0.00" | vr.price == "")
                        {
                            v0.Add(vr);
                        }
                    }
                }
                return v0;
            }
        }

        public Variant LegacyUpdateInvetorey(Variant productVariant, int newInventory, long location_id)
        {
            restart:
            var v = new { location_id = location_id, inventory_item_id = productVariant.inventory_item_id, available = newInventory };
            //v.id = productVariant.id;
            //v.inventory_quantity = newInventory;
            //v.old_inventory_quantity = productVariant.inventory_quantity;

            var variant_json = JsonConvert.SerializeObject(v);

            //variant_json = "{ " + "\"variant" + "\"" + ":" + variant_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/inventory_levels/set.json";

            string jsonString = CurrentStoreAPIAccess.PostURL(tmpURL, variant_json.ToString()).result;

            RootObject obj;
            errors er = new errors();

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }
            else if (jsonString.Contains("errors"))
            {
                er.title = new List<string>();
                er.title.Add(jsonString.Split(':')[1].Replace("[", "").Replace("]", "").Replace("}", ""));
                productVariant.LastError = er;
            }
            else
            {
                obj = JsonConvert.DeserializeObject<RootObject>(jsonString);
                productVariant.inventory_Level = obj.inventory_level;

            }
            return productVariant;
        }

        public Variant UpdateInvetorey(Variant productVariant, int newInventory, long location_id)
        {
            const int maxRetries = 3;
            int attempts = 0;
            bool success = false;

            while (!success && attempts < maxRetries)
            {
                attempts++;

                try
                {
                    var payload = new
                    {
                        location_id = location_id,
                        inventory_item_id = productVariant.inventory_item_id,
                        available = newInventory
                    };

                    string variantJson = JsonConvert.SerializeObject(payload);
                    string url = CurrentStoreAPIAccess.APIUrl() + "/inventory_levels/set.json";
                    string jsonResponse = CurrentStoreAPIAccess.PostURL(url, variantJson).result;

                    // Rate limiting — wait and retry
                    if (jsonResponse.Contains("Exceeded 2 calls per second for api client."))
                    {
                        System.Threading.Thread.Sleep(1000);
                        continue;
                    }

                    // Error response handling
                    if (jsonResponse.Contains("errors"))
                    {
                        productVariant.LastError = new errors
                        {
                            title = new List<string>
                    {
                        jsonResponse.Split(':')[1].Replace("[", "").Replace("]", "").Replace("}", "").Trim()
                    }
                        };
                        break;
                    }

                    // Deserialize safely
                    RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonResponse);
                    if (obj != null && obj.inventory_level != null)
                    {
                        productVariant.inventory_Level = obj.inventory_level;
                    }
                    else
                    {
                        productVariant.LastError = new errors
                        {
                            title = new List<string> { "Invalid response structure from API." }
                        };
                    }

                    success = true; // Exit loop
                }
                catch (Exception ex)
                {
                    // Catch all other exceptions and record error in the variant
                    productVariant.LastError = new errors
                    {
                        title = new List<string> { ex.Message }
                    };
                    break;
                }
            }

            return productVariant;
        }


        public Shopify.IO.Types.Product UpdateTags(Shopify.IO.Types.Product product, string NewTags)
        {
            restart:
            var p = new { id = product.id, tags = NewTags };

            var product_json = JsonConvert.SerializeObject(p);

            product_json = "{ " + "\"product" + "\"" + ":" + product_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product.id + ".json";

            string jsonString = CurrentStoreAPIAccess.PutURL(tmpURL, product_json.ToString()).result;

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj.product;
        }

        public Variant UpdateVariantSKU(Variant productVariant, string NewSKU)
        {
            restart:
            var v = new { id = productVariant.id, sku = NewSKU};
            //v.id = productVariant.id;
            //v.inventory_quantity = newInventory;
            //v.old_inventory_quantity = productVariant.inventory_quantity;

            var variant_json = JsonConvert.SerializeObject(v);

            variant_json = "{ " + "\"variant" + "\"" + ":" + variant_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/variants/" + productVariant.id + ".json";

            string jsonString = CurrentStoreAPIAccess.PutURL(tmpURL, variant_json.ToString()).result;

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj.variant;

        }

        public Variant UpdatePrices(Variant productVariant, decimal price, decimal comparedToPrice)
        {
            restart:
            if (price == 0M)
                throw new Exception("product price cannot be 0.");

            var v = new { id = productVariant.id, price = price, compare_at_price = comparedToPrice };
            //v.id = productVariant.id;
            //v.inventory_quantity = newInventory;
            //v.old_inventory_quantity = productVariant.inventory_quantity;

            var variant_json = JsonConvert.SerializeObject(v);

            variant_json = "{ " + "\"variant" + "\"" + ":" + variant_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/variants/" + productVariant.id + ".json";

            string jsonString = CurrentStoreAPIAccess.PutURL(tmpURL, variant_json.ToString()).result;

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj.variant;
        }

        /// <summary>
        /// the resulted object returned in an instance of RootObject
        /// first check the product property.
        /// otherwise check the error property.
        /// </summary>
        /// <param name="newProduct"></param>
        /// <returns></returns>
        public RootObject AddProduct(Shopify.IO.Types.Product newProduct)
        {
           restart:
            //create list of variant with available values.
            List<object> lov = new List<object>();
            foreach (Variant ov in newProduct.variants)
            {
                var v = new
                {
                    title = ov.title,
                    price = ov.price,
                    sku = ov.sku,
                    position = ov.position,
                    grams = ov.grams,
                    inventory_policy = ov.inventory_policy,
                    compare_at_price = ov.compare_at_price,
                    fulfillment_service = ov.fulfillment_service,
                    inventory_management = ov.inventory_management,
                    option1 = ov.option1,
                    option2 = ov.option2,
                    option3 = ov.option3,
                    requires_shipping = ov.requires_shipping,
                    taxable = ov.taxable,
                    barcode = ov.barcode,
                    inventory_quantity = ov.inventory_quantity,
                    image_id = ov.image_id
                };
                lov.Add(v);
            }

            //create list of the option of the product with the available values.
            List<object> loo = new List<object>();
            foreach (Option oo in newProduct.options)
            {
                var o = new
                {
                    name = oo.name,
                    values = oo.values,
                };
                loo.Add(o);
            }


            var p = new
            {
                title = newProduct.title,
                body_html = newProduct.body_html,
                vendor = newProduct.vendor,
                product_type = newProduct.product_type,
                published = newProduct.published,
                variants = lov,
                options = loo,
                tags = newProduct.tags,
                image = newProduct.image,
                images = newProduct.images
            };


            var product_json = JsonConvert.SerializeObject(p);

            product_json = "{ " + "\"product" + "\"" + ":" + product_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products.json";

            string jsonString = CurrentStoreAPIAccess.PostURL(tmpURL, product_json.ToString()).result;

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj;

        }
        /// <summary>
        /// the resulted object returned in an instance of RootObject
        /// first check the variant property.
        /// otherwise check the error property.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="NewVariant"></param>
        /// <returns></returns>
        public RootObject AddProductVariant(Shopify.IO.Types.Product product, Variant NewVariant)
        {
            var v = new
            {
                title = NewVariant.title,
                price = NewVariant.price,
                sku = NewVariant.sku,
                position = NewVariant.position,
                grams = NewVariant.grams,
                inventory_policy = NewVariant.inventory_policy,
                compare_at_price = NewVariant.compare_at_price,
                fulfillment_service = NewVariant.fulfillment_service,
                inventory_management = NewVariant.inventory_management,
                option1 = NewVariant.option1,
                option2 = NewVariant.option2,
                option3 = NewVariant.option3,
                requires_shipping = NewVariant.requires_shipping,
                taxable = NewVariant.taxable,
                barcode = NewVariant.barcode,
                inventory_quantity = NewVariant.inventory_quantity,
                image_id = NewVariant.image_id
            };

            restart:
            var variant_json = JsonConvert.SerializeObject(v);

            variant_json = "{ " + "\"variant" + "\"" + ":" + variant_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product.id + "/variants.json";

            string jsonString = CurrentStoreAPIAccess.PostURL(tmpURL, variant_json.ToString()).result;

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj;

        }

        //public int Count
        //{
        //    get
        //    {
        //        restart:
        //        //get json from Shopify.
        //        string tmpURL = CurrentStoreAPIAccess.APIUrl() + CurrentStoreAPIAccess.APIPath + "/products/count.json";
        //        string jsonString = CurrentStoreAPIAccess.GetURL(tmpURL).result;
        //        if (jsonString.Contains("Exceeded 2 calls per second for api client."))
        //        {
        //            System.Threading.Thread.Sleep(1000);
        //            goto restart;
        //        }
        //        RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

        //        return (int)obj.count;
        //    }
        //}

        //public int Pages
        //{
        //    get
        //    {
        //        int pageRemain; int count;
        //        count = this.Count;

        //        pageRemain = count % 50;

        //        if (pageRemain > 0)
        //        {
        //            pageRemain = 1;
        //        }
        //        else
        //        {
        //            pageRemain = 0;
        //        }

        //        return ((int)count / 50) + pageRemain;
        //    }
        //}

        public int ImagesCount()
        {
            return 0;
        }

        public errors CurrentError { get; set; }

        public List<Image> GetProductImages(Shopify.IO.Types.Product product)
        {
            return new List<Image>();
        }

        public Image GetProductImage(Shopify.IO.Types.Product product, long image_id)
        {
            return new Image();
        }

        public Image AddProductImage(Shopify.IO.Types.Product product, Image NewImage)
        {
            return new Image();
        }

        public Image UpdateProductImage(Shopify.IO.Types.Product product, Image ModifiedImage)
        {
            return new Image();
        }

        public bool DeleteProductImage(Shopify.IO.Types.Product product, Image image)
        {
            return true;
        }

        public bool DeleteProductVariant(Product product, Variant variant)
        {
            restart:
            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product.id + "/variants/" + variant.id + ".json";

            string jsonString = CurrentStoreAPIAccess.DeleteURL(tmpURL).result;

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            //RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            if (jsonString.Contains("OK"))
                return true;
            else
                return false;
        }
        public int GetVariantsCount(List<Product> lp)
        {
            int counter = 0;
            foreach (Product p in lp)
            {
                counter += p.variants.Count;
            }

            return counter;
        }

       
    }
}
