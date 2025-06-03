using Newtonsoft.Json;
using Shopify.IO.helpers;
using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.Linq;
//using ShopifySharp;
namespace Shopify.IO.Operations
{
    public class Locations
    {
        public errors LastError { get; set;  }
        private APIAccess CurrentStoreAPIAccess;
        //private ShopifySharp.ProductService service; 

        public Locations(APIAccess ApiAccess)
        {
            // TODO: Complete member initialization
            this.CurrentStoreAPIAccess = ApiAccess;
            //            service = new ProductService(CurrentSroreAPIAccess.APIUrl(),CurrentSroreAPIAccess.Password); 
        }

        public List<Shopify.IO.Types.Location> GetList(params string[] requiredFields)
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
            List<Shopify.IO.Types.Location> ll = new List<Shopify.IO.Types.Location>();
            CustomeResoponce cr;
        //int pageCount = this.Pages;

        //for (int i = 1; i <= pageCount; i++)
        //{
        //get json from Shopify.
        restart:
            string tmpURL;
            if (requiredFields.Length > 0)
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/locations.json?limit=50&" + fields;
            }
            else
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/locations.json?limit=50";
            }

            cr = CurrentStoreAPIAccess.GetURL(tmpURL);

            if (cr.result.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Shopify.IO.Types.Location nl in obj.locations)
            {
                ll.Add(nl);
            }

            var link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);


            if (link != null && link.Any())
            {
                if (link.FirstOrDefault().Value.ToString().Contains("next"))
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

                        foreach (Shopify.IO.Types.Location nl in obj.locations)
                        {
                            ll.Add(nl);
                        }

                        link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);

                        nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                    }
                }
            }
            return ll;
        }

    }
}
