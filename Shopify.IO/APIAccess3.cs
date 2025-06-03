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

        #endregion

        #region Constructors
        public APIAccess(string apiKey, string password, string sharedsecret, string hostname, string apiVersion)
        {
            APIKey = apiKey;
            Password = password;
            SharedSecret = sharedsecret;
            HostName = hostname;
            ApiVersion = apiVersion;
        }

        public APIAccess()
        {
        }
        #endregion

        #region URL Builders
        public string APIUrl()
        {
            string tmpURL = "https://" + HostName + "/admin/api/" + ApiVersion;
            return tmpURL;
        }

        public string GraphQLUrl()
        {
            string tmpURL = "https://" + HostName + "/admin/api/" + ApiVersion + "/graphql.json";
            return tmpURL;
        }
        #endregion

        #region REST API Methods (Existing)
        public CustomeResoponce GetURL(string URL)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var rc = new RestClient(URL);
            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            var rr = new RestRequest();
            rr.Method = Method.GET;
            rr.AddHeader("Accept", "application/json");
            rr.Parameters.Clear();

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
            var rc = new RestClient(URL);
            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            var rr = new RestRequest();
            rr.Method = Method.PUT;
            rr.AddHeader("Accept", "application/json");
            rr.AddJsonBody(json);
            rr.Parameters.Clear();
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
            var rc = new RestClient(URL);
            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            var rr = new RestRequest();
            rr.Method = Method.POST;
            rr.AddHeader("Accept", "application/json");
            rr.AddJsonBody(json);
            rr.Parameters.Clear();
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

            var rr = new RestRequest();
            rr.Method = Method.DELETE;

            var r = rc.Execute(rr);
            var content = r.Content;

            CustomeResoponce cr = new CustomeResoponce();
            cr.result = content.ToString();
            cr.fullResponse = r;
            return cr;
        }
        #endregion

        #region GraphQL Methods
        /// <summary>
        /// Execute a GraphQL query against Shopify Admin API
        /// </summary>
        /// <param name="query">GraphQL query string</param>
        /// <param name="variables">Optional variables for the query</param>
        /// <returns>CustomeResoponce containing the GraphQL response</returns>
        public CustomeResoponce ExecuteGraphQL(string query, object variables = null)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var graphqlUrl = GraphQLUrl();
            var rc = new RestClient(graphqlUrl);
            rc.Authenticator = new HttpBasicAuthenticator(APIKey, Password);

            var rr = new RestRequest();
            rr.Method = Method.POST;
            rr.AddHeader("Content-Type", "application/json");
            rr.AddHeader("Accept", "application/json");

            // Add Shopify API version header (recommended for GraphQL)
            rr.AddHeader("X-Shopify-Access-Token", Password);

            // Build GraphQL request body
            var requestBody = new
            {
                query = query,
                variables = variables
            };

            var json = JsonConvert.SerializeObject(requestBody);
            rr.Parameters.Clear();
            rr.AddParameter("application/json", json, ParameterType.RequestBody);

            var r = rc.Execute(rr);
            var content = r.Content;

            CustomeResoponce cr = new CustomeResoponce();
            cr.result = content.ToString();
            cr.fullResponse = r;

            return cr;
        }

        /// <summary>
        /// Execute a GraphQL query with typed response
        /// </summary>
        /// <typeparam name="T">Expected response type</typeparam>
        /// <param name="query">GraphQL query string</param>
        /// <param name="variables">Optional variables for the query</param>
        /// <returns>Typed GraphQL response</returns>
        public GraphQLResponse<T> ExecuteGraphQL<T>(string query, object variables = null)
        {
            var response = ExecuteGraphQL(query, variables);

            try
            {
                var graphqlResponse = JsonConvert.DeserializeObject<GraphQLResponse<T>>(response.result);
                graphqlResponse.RawResponse = response;
                return graphqlResponse;
            }
            catch (JsonException ex)
            {
                return new GraphQLResponse<T>
                {
                    Errors = new List<GraphQLError>
                    {
                        new GraphQLError { Message = $"JSON Deserialization Error: {ex.Message}" }
                    },
                    RawResponse = response
                };
            }
        }

        /// <summary>
        /// Execute a GraphQL mutation
        /// </summary>
        /// <param name="mutation">GraphQL mutation string</param>
        /// <param name="variables">Variables for the mutation</param>
        /// <returns>CustomeResoponce containing the mutation response</returns>
        public CustomeResoponce ExecuteGraphQLMutation(string mutation, object variables = null)
        {
            return ExecuteGraphQL(mutation, variables);
        }

        /// <summary>
        /// Execute a GraphQL mutation with typed response
        /// </summary>
        /// <typeparam name="T">Expected response type</typeparam>
        /// <param name="mutation">GraphQL mutation string</param>
        /// <param name="variables">Variables for the mutation</param>
        /// <returns>Typed GraphQL response</returns>
        public GraphQLResponse<T> ExecuteGraphQLMutation<T>(string mutation, object variables = null)
        {
            return ExecuteGraphQL<T>(mutation, variables);
        }

        /// <summary>
        /// Get GraphQL schema introspection query
        /// </summary>
        /// <returns>Schema introspection response</returns>
        public CustomeResoponce GetGraphQLSchema()
        {
            string introspectionQuery = @"
                query IntrospectionQuery {
                    __schema {
                        queryType { name }
                        mutationType { name }
                        subscriptionType { name }
                        types {
                            ...FullType
                        }
                        directives {
                            name
                            description
                            locations
                            args {
                                ...InputValue
                            }
                        }
                    }
                }

                fragment FullType on __Type {
                    kind
                    name
                    description
                    fields(includeDeprecated: true) {
                        name
                        description
                        args {
                            ...InputValue
                        }
                        type {
                            ...TypeRef
                        }
                        isDeprecated
                        deprecationReason
                    }
                    inputFields {
                        ...InputValue
                    }
                    interfaces {
                        ...TypeRef
                    }
                    enumValues(includeDeprecated: true) {
                        name
                        description
                        isDeprecated
                        deprecationReason
                    }
                    possibleTypes {
                        ...TypeRef
                    }
                }

                fragment InputValue on __InputValue {
                    name
                    description
                    type { ...TypeRef }
                    defaultValue
                }

                fragment TypeRef on __Type {
                    kind
                    name
                    ofType {
                        kind
                        name
                        ofType {
                            kind
                            name
                            ofType {
                                kind
                                name
                                ofType {
                                    kind
                                    name
                                    ofType {
                                        kind
                                        name
                                        ofType {
                                            kind
                                            name
                                            ofType {
                                                kind
                                                name
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            ";

            return ExecuteGraphQL(introspectionQuery);
        }
        #endregion

        #region Helper Methods
        private CredentialCache GetCredential(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            var credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(url), "Basic", new NetworkCredential(APIKey, Password));
            return credentialCache;
        }
        #endregion
    }

    #region Response Classes
    public class CustomeResoponce
    {
        public string result { get; set; }
        public IRestResponse fullResponse { get; set; }
    }

    public class GraphQLResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("errors")]
        public List<GraphQLError> Errors { get; set; }

        [JsonProperty("extensions")]
        public Dictionary<string, object> Extensions { get; set; }

        [JsonIgnore]
        public CustomeResoponce RawResponse { get; set; }

        public bool HasErrors => Errors != null && Errors.Count > 0;
    }

    public class GraphQLError
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("locations")]
        public List<GraphQLLocation> Locations { get; set; }

        [JsonProperty("path")]
        public List<object> Path { get; set; }

        [JsonProperty("extensions")]
        public Dictionary<string, object> Extensions { get; set; }
    }

    public class GraphQLLocation
    {
        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }
    }
    #endregion
}