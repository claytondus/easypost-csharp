using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace EasyPost {
    public class Client {
        public string version;

        internal RestClient client;
        internal ClientConfiguration configuration;
        
        public Client(ClientConfiguration clientConfiguration) {
            //System.Net.ServicePointManager.SecurityProtocol = Security.GetProtocol();

            if (clientConfiguration == null) throw new ArgumentNullException(nameof(clientConfiguration));
            configuration = clientConfiguration;

            client = new RestClient(clientConfiguration.ApiBase);

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);

            version = typeof(Client).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public async Task<IRestResponse> ExecuteAsync(Request request) {
            return await client.Execute(PrepareRequest(request));
        }

        public async Task<T> ExecuteAsync<T>(Request request) where T : new() {
            RestResponse<T> response = (RestResponse<T>) await client.Execute<T>(PrepareRequest(request));
            int StatusCode = Convert.ToInt32(response.StatusCode);

            if (StatusCode > 399) {
                try {
                    Dictionary<string, Dictionary<string, object>> Body = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(response.Content);
                    throw new HttpException(StatusCode, (string)Body["error"]["message"], (string)Body["error"]["code"]);
                } catch {
                    throw new HttpException(StatusCode, "RESPONSE.PARSE_ERROR", response.Content);
                }
            }

            return response.Data;
        }

        internal RestRequest PrepareRequest(Request request) {
            RestRequest restRequest = (RestRequest)request;

            restRequest.AddHeader("user_agent", string.Concat("EasyPost/v2 CSharp/", version));
            restRequest.AddHeader("authorization", "Bearer " + this.configuration.ApiKey);
            restRequest.AddHeader("content_type", "application/x-www-form-urlencoded");

            return restRequest;
        }
    }
}
