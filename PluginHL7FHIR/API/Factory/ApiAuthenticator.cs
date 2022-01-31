using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Naveego.Sdk.Logging;
using Newtonsoft.Json;
using PluginHL7FHIR.DataContracts;
using PluginHL7FHIR.Helper;

namespace PluginHL7FHIR.API.Factory
{
    public class ApiAuthenticator: IApiAuthenticator
    {
        private HttpClient Client { get; set; }
        private Settings Settings { get; set; }
        private string Token { get; set; }
        private DateTime ExpiresAt { get; set; }
        
        private const string AuthUrl = "https://api.hubapi.com/oauth/v1/token";
        
        public ApiAuthenticator(HttpClient client, Settings settings)
        {
            Client = client;
            Settings = settings;
            ExpiresAt = DateTime.Now;
            Token = "";
        }

        public async Task<string> GetToken()
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetNewToken()
        {
            throw new NotImplementedException();
        }
    }
}