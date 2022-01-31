using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Naveego.Sdk.Logging;
using Naveego.Sdk.Plugins;
using Newtonsoft.Json;
using PluginHL7FHIR.API.Factory;
using PluginHL7FHIR.API.Utility;

namespace PluginHL7FHIR.API.Write
{
    public static partial class Write
    {
        private static readonly SemaphoreSlim WriteSemaphoreSlim = new SemaphoreSlim(1, 1);

        public static async Task<string> WriteRecordAsync(IApiClient apiClient, Schema schema, Record record,
            IServerStreamWriter<RecordAck> responseStream)
        {
            throw new NotImplementedException();
        }
    }
}