using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.IO.helpers
{
    public class APIAccess
    {

        #region Properties&Fields
        private string aPIKey;

        public string APIKey
        {
            get { return aPIKey; }
            set { aPIKey = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string sharedSecret;

        public string SharedSecret
        {
            get { return sharedSecret; }
            set { sharedSecret = value; }
        }

        private string hostName;

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private string apiVersion;
        public string ApiVersion
        {
            get { return apiVersion; }
            set { apiVersion = value; }
        }

        //public string APIPath { get; set; }

        #endregion

        #region Constructors
        public APIAccess(string apiKey, string password, string sharedsecret, string hostname, string apiVersion)
        {
            APIKey = apiKey; Password = password; SharedSecret = sharedsecret; HostName = hostname; ApiVersion = apiVersion;
        }

        public APIAccess()
        {

        }
        #endregion

        public string APIUrl()
        {
            //since SHOPIFY only accepts secure connection to their API
            //I had to use https.

            string tmpURL = "";

            tmpURL = "https://" + HostName + "/admin/api/" + ApiVersion;

            return tmpURL;
        }

        public CustomeResoponce GetURL(string URL)
        {
            #region old code
            //var req = (HttpWebRequest)WebRequest.Create(URL);
            //req.Method = "GET";
            //req.ContentType = "application/json";
            //req.Credentials = GetCredential(URL);
            //req.PreAuthenticate = true;

            //using (var resp = (HttpWebResponse)req.GetResponse())
            //{
            //    if (resp.StatusCode != HttpStatusCode.OK)
            //    {
            //        string message = String.Format("Call failed. Received HTTP {0}", resp.StatusCode);
            //        throw new ApplicationException(message);
            //    }

            //    var sr = new StreamReader(resp.GetResponseStream());
            //    return sr.ReadToEnd();
            //} 
            #endregion
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var rc = new RestClient(URL);

            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            //rr for RestRequest
            var rr = new RestRequest();

            rr.Method = Method.GET;

            rr.AddHeader("Accept", "application/json");

            //rr.AddBody(json);
            rr.Parameters.Clear();

            //adding parameter to request

            //rr.AddParameter("application/json", json, ParameterType.RequestBody);

            CustomeResoponce cr = new CustomeResoponce();
            var r = rc.Execute(rr);



            if (r.StatusCode == HttpStatusCode.NotFound)
            {
                Types.errors er = new Types.errors();

                er.title = new List<string>();

                er.title.Add("Not Found");

                var errorJson = JsonConvert.SerializeObject(er);

                errorJson = "{ " + "\"errors" + "\"" + ":" + errorJson + "}";

                cr.result = errorJson;
                cr.fullResponse = r;

                return cr;
            }
            else
            {
                var content = r.Content;

                cr.result = content.ToString();
                cr.fullResponse = r;

                return cr;
            }
        }

        public CustomeResoponce PutURL(string URL, string json)
        {
            #region old code
            //var req = (HttpWebRequest)WebRequest.Create(URL);
            //req.Method = "PUT";
            //req.ContentType = "application/json";
            //req.Credentials = GetCredential(URL);
            //req.PreAuthenticate = true;

            //using (var ms = new MemoryStream())
            //{
            //    using (var writer = new StreamWriter(req.GetRequestStream()))
            //    {
            //        writer.Write(json);
            //        writer.Close();
            //    }
            //}

            //using (var resp = (HttpWebResponse)req.GetResponse())
            //{
            //    if (resp.StatusCode != HttpStatusCode.OK)
            //    {
            //        string message = String.Format("Call failed. Received HTTP {0}", resp.StatusCode);
            //        throw new ApplicationException(message);
            //    }

            //    var sr = new StreamReader(resp.GetResponseStream());
            //    return sr.ReadToEnd();
            //}
            #endregion

            var rc = new RestClient(URL);

            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            //rr for RestRequest
            var rr = new RestRequest();

            rr.Method = Method.PUT;

            rr.AddHeader("Accept", "application/json");

            rr.AddJsonBody(json);
            rr.Parameters.Clear();

            //adding parameter to request

            rr.AddParameter("application/json", json, ParameterType.RequestBody);

            var r = rc.Execute(rr);

            var content = r.Content;

            CustomeResoponce cr = new CustomeResoponce();

            cr.result = content.ToString();
            cr.fullResponse = r;

            return cr;

        }

        public CustomeResoponce PostURL(string URL, string json)
        {
            #region old code kept for reference
            //var req = (HttpWebRequest)WebRequest.Create(URL);
            //req.Method = "Post";
            //req.ContentType = "application/json";
            //req.Credentials = GetCredential(URL);
            //req.PreAuthenticate = true;

            //using (var ms = new MemoryStream())
            //{
            //    using (var writer = new StreamWriter(req.GetRequestStream()))
            //    {
            //        writer.Write(json);
            //        writer.Close();
            //    }
            //}

            //using (var resp = (HttpWebResponse)req.GetResponse())
            //{
            //    if (resp.StatusCode != HttpStatusCode.Created)
            //    {
            //        string message = String.Format("Call failed. Received HTTP {0}", resp.StatusCode);
            //        throw new ApplicationException(message);
            //    }

            //    var sr = new StreamReader(resp.GetResponseStream());
            //    return sr.ReadToEnd();
            //}


            //new code using RestSharp.

            #endregion            //rc for RestSharp

            var rc = new RestClient(URL);

            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            //rr for RestRequest
            var rr = new RestRequest();

            rr.Method = Method.POST;

            rr.AddHeader("Accept", "application/json");

            rr.AddJsonBody(json);
            rr.Parameters.Clear();

            //adding parameter to request

            rr.AddParameter("application/json", json, ParameterType.RequestBody);

            var r = rc.Execute(rr);

            var content = r.Content;

            CustomeResoponce cr = new CustomeResoponce();

            cr.result = content.ToString();
            cr.fullResponse = r;

            return cr;

        }

        public CustomeResoponce DeleteURL(string URL)
        {
            var rc = new RestClient(URL);

            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            //rr for RestRequest
            var rr = new RestRequest();

            rr.Method = Method.DELETE;

            //rr.AddHeader("Accept", "application/json");

            //rr.AddBody("{}");
            //rr.Parameters.Clear();

            //adding parameter to request

            //rr.AddParameter("application/json", "{}", ParameterType.RequestBody);

            var r = rc.Execute(rr);

            var content = r.Content;

            CustomeResoponce cr = new CustomeResoponce();

            cr.result = content.ToString();
            cr.fullResponse = r;

            return cr;

        }



        private CredentialCache GetCredential(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            var credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(url), "Basic", new NetworkCredential(APIKey, Password));
            return credentialCache;
        }
    }

    public class CustomeResoponce
    {
        public string result { get; set; }

        public IRestResponse fullResponse { get; set; }
    }
}
