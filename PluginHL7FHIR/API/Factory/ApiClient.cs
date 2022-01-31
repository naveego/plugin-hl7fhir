using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Naveego.Sdk.Logging;
using PluginHL7FHIR.API.Utility;
using PluginHL7FHIR.Helper;
using Task = System.Threading.Tasks.Task;

namespace PluginHL7FHIR.API.Factory
{
    public class ApiClient: IApiClient
    {
        private static FhirClient Client { get; set; }
        private Settings Settings { get; set; }


        public ApiClient(Settings settings)
        {
            if (settings.Token?.Length > 0)
            {
                var handler = new HttpClientEventHandler();
                handler.OnBeforeRequest += (sender, e) =>
                {
                    e.RawRequest.Headers
                        .Add("Authorization", $"Bearer {settings.Token}");
                };
                
                Client = new FhirClient(settings.Endpoint, FhirClientSettings.CreateDefault(), handler);
            }
            else
            {
                Client = new FhirClient(settings.Endpoint);
            }
            
            Settings = settings;
        }

        public FhirClient GetClient()
        {
            return Client;
        }
        
        public async Task TestConnection()
        {
            try
            {
                var client = new FhirClient(Settings.Endpoint);

                // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> PostAsync(string path, StringContent json)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> PutAsync(string path, StringContent json)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> PatchAsync(string path, StringContent json)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> DeleteAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}