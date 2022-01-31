using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Naveego.Sdk.Plugins;
using PluginHL7FHIR.API.Factory;
using PluginHL7FHIR.API.Utility;

namespace PluginHL7FHIR.API.Read
{
    public static partial class Read
    {
        public static async IAsyncEnumerable<Record> ReadRecordsAsync(IApiClient apiClient, Schema schema)
        {
            var endpoint = EndpointHelper.GetEndpointForSchema(schema);

            var records = endpoint?.ReadRecordsAsync(apiClient, schema);

            if (records != null)
            {
                await foreach (var record in records)
                {
                    yield return record;
                }
            }
        }
    }
}