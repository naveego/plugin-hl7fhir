using System.Threading.Tasks;

namespace PluginHL7FHIR.API.Factory
{
    public interface IApiAuthenticator
    {
        Task<string> GetToken();
    }
}