using Newtonsoft.Json;
using Shopify.IO.helpers;
using Shopify.IO.Types;
using System;
using System.Collections.Generic;

namespace Shopify.IO.Operations
{
    public class Variants
    {
        public errors LastError { get; set; }
        private APIAccess CurrentStoreAPIAccess;
        public Variants(APIAccess ApiAccess)
        {
            // TODO: Complete member initialization
            this.CurrentStoreAPIAccess = ApiAccess;
        }

        public Variant GetVariant(long variant_id, params string[] requiredFields)
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
            restart:

            string tmpURL;
            if (requiredFields.Length > 0)
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/variants/" + variant_id.ToString() + ".json?" + fields;
            }
            else
            {
                tmpURL = CurrentStoreAPIAccess.APIUrl() + "/variants/" + variant_id.ToString() + ".json";
            }

            string jsonString = CurrentStoreAPIAccess.GetURL(tmpURL).ToString();

            if (jsonString.Contains("Exceeded 2 calls per second for api client."))
            {
                System.Threading.Thread.Sleep(1000);
                goto restart;
            }

            //de-serialize the JSON string to Dictionary
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonString);

            if (obj.product == null)
                LastError = obj.errors;

            return obj.variant;

        }

    }
}
