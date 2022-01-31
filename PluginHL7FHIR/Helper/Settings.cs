using System;

namespace PluginHL7FHIR.Helper
{
    public class Settings
    {
        public string Endpoint { get; set; }
        public string Token { get; set; }
        /// <summary>
        /// Validates the settings input object
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Validate()
        {
           
            if (String.IsNullOrEmpty(Endpoint))
            {
                throw new Exception("the Endpoint property must be set");
            }
            
        }
    }
}