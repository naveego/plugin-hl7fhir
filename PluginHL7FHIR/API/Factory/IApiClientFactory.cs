using PluginHL7FHIR.Helper;

namespace PluginHL7FHIR.API.Factory
{
    public interface IApiClientFactory
    {
        IApiClient CreateApiClient(Settings settings);
    }
}