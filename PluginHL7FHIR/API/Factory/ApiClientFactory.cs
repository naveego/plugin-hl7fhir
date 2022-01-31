using System.Net.Http;
using Hl7.Fhir.Rest;
using PluginHL7FHIR.Helper;

namespace PluginHL7FHIR.API.Factory
{
    public class ApiClientFactory: IApiClientFactory
    {
        private FhirClient Client { get; set; }

        public ApiClientFactory(FhirClient client)
        {
            Client = client;
        }

        public IApiClient CreateApiClient(Settings settings)
        {
            return new ApiClient(settings);
        }
    }
}