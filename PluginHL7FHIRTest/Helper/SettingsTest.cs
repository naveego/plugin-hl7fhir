using System;
using PluginHL7FHIR.Helper;
using Xunit;

namespace PluginHubspotTest.Helper
{
    public class SettingsTest
    {
        [Fact]
        public void ValidateValidTest()
        {
            // setup
            var settings = new Settings
            {
                Endpoint = @"https://server.fire.ly"
            };

            // act
            settings.Validate();

        }
    }
}