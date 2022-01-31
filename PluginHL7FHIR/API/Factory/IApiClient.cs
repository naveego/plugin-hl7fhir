using System.Net.Http;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Task = System.Threading.Tasks.Task;

namespace PluginHL7FHIR.API.Factory
{
    public interface IApiClient
    {
        Task TestConnection();

        FhirClient GetClient();
        Task<HttpResponseMessage> GetAsync(string path);
        Task<HttpResponseMessage> PostAsync(string path, StringContent json);
        Task<HttpResponseMessage> PutAsync(string path, StringContent json);
        Task<HttpResponseMessage> PatchAsync(string path, StringContent json);
        Task<HttpResponseMessage> DeleteAsync(string path);
    }
}