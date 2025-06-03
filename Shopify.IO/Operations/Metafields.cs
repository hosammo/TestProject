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
    public class Metafields
    {
        public errors LastError { get; set; }

        private APIAccess CurrentStoreAPIAccess;

        public Metafields(APIAccess ApiAccess)
        {
            // TODO: Complete member initialization
            this.CurrentStoreAPIAccess = ApiAccess;
        }

        public List<Metafield> GetList(params string[] requiredFields)
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
            List<Metafield> p = new List<Metafield>();
            CustomeResoponce cr;

            //int pageCount = this.Pages;

            //for (int i = 1; i <= pageCount; i++)
            //{
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

            cr = CurrentStoreAPIAccess.GetURL(tmpURL); ;

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Metafield np in obj.metafields)
            {
                p.Add(np);
            }
            //}

            var link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);


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

                    foreach (Shopify.IO.Types.Metafield mf in obj.metafields)
                    {
                        p.Add(mf);
                    }

                    link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);

                    nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                }
            }

            return p;
        }

        public Metafield GetMetafield(long product_id, params string[] requiredFields)
        {
            CustomeResoponce cr;

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

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            if (obj.metafield == null)
                LastError = obj.errors;

            return obj.metafield;

        }

        public List<Metafield> GetListWithEndPoint(params MetafieldEndPoint[] EndPoints)
        {
            string EndPoints_str = "";
            if (EndPoints.Length > 0)
            {
                foreach (MetafieldEndPoint ep in EndPoints)
                {
                    EndPoints_str += ep.EndPointType.ToString() + "=" + ep.EntPointValue.ToString();
                    EndPoints_str += ",";
                }
            }
            EndPoints_str = EndPoints_str.Remove(EndPoints_str.Length - 1, 1);
            //empty list will hold all products returned from shopify.
            List<Metafield> p = new List<Metafield>();
            CustomeResoponce cr;
            //for (int i = 1; i <= pageCount; i++)
            //{
            //get json from Shopify.
            string tmpURL;
            if (EndPoints.Length > 0)
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/metafields.json?limit=50&" + EndPoints_str;
            }
            else
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/metafields.json?page=50";
            }

            cr = CurrentStoreAPIAccess.GetURL(tmpURL);

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            foreach (Metafield np in obj.metafields)
            {
                p.Add(np);
            }


            var link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);


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

                    foreach (Shopify.IO.Types.Metafield mf in obj.metafields)
                    {
                        p.Add(mf);
                    }

                    link = (from b in cr.fullResponse.Headers where b.Name == "Link" || b.Name == "link" select b);

                    nextURL = (from v in link.FirstOrDefault().ToString().Split(',') where v.Contains("next") select v);
                }
            }

            //}
            return p;
        }


        /// <summary>
        /// the resulted object returned in an instance of RootObject
        /// first check the product property.
        /// otherwise check the error property.
        /// </summary>
        /// <param name="newMetafield"></param>
        /// <returns></returns>
        public RootObject AddMetafield(Metafield NewMetafield)
        {
            //create list of variant with available values.
            var m = new
            {
                @namespace = NewMetafield.@namespace,
                key = NewMetafield.key,
                value = NewMetafield.value,
                value_type = NewMetafield.value_type,
                description = NewMetafield.description
            };


            var metafield_json = JsonConvert.SerializeObject(m);

            metafield_json = "{ " + "\"metafields" + "\"" + ":" + metafield_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/metafields.json";

            string jsonString = CurrentStoreAPIAccess.PostURL(tmpURL, metafield_json.ToString()).result;

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj;

        }

        public RootObject AddProductMetafield(Product product, Metafield NewMetafield)
        {

            var m = new
            {
                @namespace = NewMetafield.@namespace,
                key = NewMetafield.key,
                value = NewMetafield.value,
                value_type = NewMetafield.value_type,
                description = NewMetafield.description
            };


            var metafield_json = JsonConvert.SerializeObject(m);

            metafield_json = "{ " + "\"metafield" + "\"" + ":" + metafield_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product.id + "/metafields.json";

            string jsonString = CurrentStoreAPIAccess.PostURL(tmpURL, metafield_json.ToString()).result;

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj;

        }

        public RootObject UpdateProductMetafield(Product product, Metafield metafield)
        {

            var m = new
            {
                @namespace = metafield.@namespace,
                key = metafield.key,
                value = metafield.value,
                value_type = metafield.value_type,
                description = metafield.description
            };


            var metafield_json = JsonConvert.SerializeObject(m);

            metafield_json = "{ " + "\"metafield" + "\"" + ":" + metafield_json + "}";

            string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product.id + "/metafields.json";

            string jsonString = CurrentStoreAPIAccess.PutURL(tmpURL, metafield_json.ToString()).result;

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            return obj;

        }

        public List<Metafield> GetProductMetafields(Product product)
        {
            CustomeResoponce cr;

            //get json from Shopify.
            string tmpURL;
            tmpURL = CurrentStoreAPIAccess.APIUrl() + "/products/" + product.id.ToString() + "/metafields.json";

            cr = CurrentStoreAPIAccess.GetURL(tmpURL);

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(cr.result);

            if (obj.product == null)
                LastError = obj.errors;

            return obj.metafields;

        }

        public int Count
        {

            get
            {
                //get json from Shopify.
                string tmpURL = CurrentStoreAPIAccess.APIUrl() + "/metafields/count.json";
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
}
