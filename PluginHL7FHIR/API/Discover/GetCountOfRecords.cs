using System.Threading.Tasks;
using Naveego.Sdk.Plugins;
using PluginHL7FHIR.API.Factory;
using PluginHL7FHIR.API.Utility;

namespace PluginHL7FHIR.API.Discover
{
    public static partial class Discover
    {
        public static Task<Count> GetCountOfRecords(IApiClient apiClient, Endpoint? endpoint)
        {
            return endpoint != null
                ? endpoint.GetCountOfRecords(apiClient)
                : Task.FromResult(new Count {Kind = Count.Types.Kind.Unavailable});
        }
    }
}