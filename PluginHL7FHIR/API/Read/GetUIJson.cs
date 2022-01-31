using System.Collections.Generic;
using Newtonsoft.Json;

namespace PluginHL7FHIR.API.Read
{
    public static partial class Read
    {
        public static string GetUIJson()
        {
            var uiJsonObj = new Dictionary<string, object>
            {
                {"ui:order", new []
                {
                    "PollingInterval"
                }}
            };

            return JsonConvert.SerializeObject(uiJsonObj);
        }
    }
}