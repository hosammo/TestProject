using Newtonsoft.Json;
using Shopify.IO.helpers;
using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.Operations
{
    public class Orders
    {

        public errors LastError { get; set; }
        private APIAccess CurrentStoreAPIAccess;
        public Orders(APIAccess ApiAccess)
        {
            // TODO: Complete member initialization
            this.CurrentStoreAPIAccess = ApiAccess;
        }

        public List<Order> GetList(params string[] requiredFields)
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
            List<Order> p = new List<Order>();
            CustomeResoponce cr;

            //get json from Shopify.
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

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Order no in obj.orders)
            {
                p.Add(no);
            }

            var link = (from b in cr.fullResponse.Headers where b.Name == "Link" select b);


            if (link != null && link.FirstOrDefault().Value.ToString().Contains("next"))
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

                    foreach (Shopify.IO.Types.Order np in obj.orders)
                    {
                        p.Add(np);
                    }

                    link = (from b in cr.fullResponse.Headers where b.Name == "Link" select b);

                    nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                }
            }

            return p;
        }

        public Product GetOrder(long product_id, params string[] requiredFields)
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

            string jsonString = CurrentStoreAPIAccess.GetURL(tmpURL).ToString();

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            if (obj.product == null)
                LastError = obj.errors;

            return obj.product;

        }

        public List<Order> GetListWithEndPoint(params OrderEndPoint[] EndPoints)
        {
            string EndPoints_str = "";
            if (EndPoints.Length > 0)
            {
                foreach (OrderEndPoint ep in EndPoints)
                {
                    EndPoints_str += ep.EndPointType.ToString() + "=" + ep.EntPointValue.ToString();
                    EndPoints_str += ",";
                }
            }
            EndPoints_str = EndPoints_str.Remove(EndPoints_str.Length - 1, 1);
            //empty list will hold all products returned from shopify.
            List<Order> o = new List<Order>();
            CustomeResoponce cr;
            //for (int i = 1; i <= pageCount; i++)
            //{
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

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Order no in obj.orders)
            {
                o.Add(no);
            }
            //}

            var link = (from b in cr.fullResponse.Headers where b.Name == "Link" select b);


            if (link != null && link.FirstOrDefault().Value.ToString().Contains("next"))
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

                    foreach (Shopify.IO.Types.Order np in obj.orders)
                    {
                        o.Add(np);
                    }

                    link = (from b in cr.fullResponse.Headers where b.Name == "Link" select b);

                    nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                }
            }


            return o;
        }
        public int Count
        {
            get
            {
                //get json from Shopify.
                string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/orders/count.json";
                string jsonString = CurrentStoreAPIAccess.GetURL(tmpURL).result;

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

                return (int)obj.count;
            }
        }

        public int Pages
    {
        get
        {
            int pageRemain; int count;
            count = this.Count;

            pageRemain = count % 50;

            if (pageRemain > 0)
            {
                pageRemain = 1;
            }
            else
            {
                pageRemain = 0;
            }

            return ((int)count / 50) + pageRemain;
        }
    }
    }

    public enum OrderEndPointTypes
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

    public class OrderEndPoint
    {
        public OrderEndPointTypes EndPointType { get; set; }

        public string EntPointValue { get; set; }
    }

}
