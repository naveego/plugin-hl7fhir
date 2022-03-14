using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Naveego.Sdk.Logging;
using Naveego.Sdk.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginHL7FHIR.API.Factory;
using PluginHL7FHIR.DataContracts;
using Task = Hl7.Fhir.Model.Task;

namespace PluginHL7FHIR.API.Utility.EndpointHelperEndpoints
{
    public class MedicationEndpointHelper
    {
        private class MedicationDispenseEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "form", 
                    "code", 
                    "manufacturer", 
                    "status", 
                    "amount_numerator", 
                    "amount_numerator_unit", 
                    "amount_numerator_code", 
                    "amount_denominator", 
                    "amount_denominator_unit", 
                    "amount_denominator_code", 
                };

                var properties = new List<Property>();

                
                
                foreach (var staticProperty in staticSchemaProperties)
                {
                    var property = new Property();
                
                    property.Id = staticProperty;
                    property.Name = staticProperty;
                
                    switch (staticProperty)
                    {
                        case ("id"):
                            property.IsKey = true;
                            property.Type = PropertyType.String;
                            property.TypeAtSource = "string";
                            break;
                        default:
                            property.IsKey = false;
                            property.Type = PropertyType.String;
                            property.TypeAtSource = "string";
                            break;
                    }
                
                    properties.Add(property);
                }

                schema.Properties.Clear();
                schema.Properties.AddRange(properties);

                schema.DataFlowDirection = GetDataFlowDirection();

                return schema;
            }

            public async override IAsyncEnumerable<Record> ReadRecordsAsync(IApiClient apiClient, Schema schema,
                bool isDiscoverRead = false)
            {

                var client = apiClient.GetClient();

                var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Medication>());
                //var searchResultResponse = client.Search<Medication>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var recordMap = new Dictionary<string, object>();
                    var medicationDispense = new Medication();
                    
                    try
                    {
                        medicationDispense = (Medication) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    recordMap["id"] = medicationDispense.Id ?? "";
                    
                    recordMap["form"] = medicationDispense.Form?.Text ?? "";
                    recordMap["code"] = medicationDispense.Code?.Text ?? "";
                    recordMap["manufacturer"] = medicationDispense.Manufacturer?.Display ?? "";
                    
                    recordMap["status"] = medicationDispense.Status.HasValue ? medicationDispense.Status.Value.ToString() : "";
                    recordMap["amount_numerator"] = medicationDispense.Amount?.Numerator.Value.ToString() ?? "";
                    recordMap["amount_numerator_unit"] = medicationDispense.Amount?.Numerator.Unit ?? "";
                    recordMap["amount_numerator_code"] = medicationDispense.Amount?.Numerator.Code ?? "";
                    
                    recordMap["amount_denominator"] = medicationDispense.Amount?.Denominator.Value.ToString() ?? "";
                    recordMap["amount_denominator_unit"] = medicationDispense.Amount?.Denominator.Unit ?? "";
                    recordMap["amount_denominator_code"] = medicationDispense.Amount?.Denominator.Code ?? "";
                    
                    yield return new Record
                    {
                        Action = Record.Types.Action.Upsert,
                        DataJson = JsonConvert.SerializeObject(recordMap)
                    };
                }
            }
        }
        private class MedicationEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "code",   
                    "form",   
                    "status",   
                    "amount_denominator",   
                    "amount_denominator_unit",   
                    "amount_denominator_code",   
                    "amount_numerator", 
                    "amount_numerator_unit", 
                    "amount_numerator_code", 
                };

                var properties = new List<Property>();

                
                
                foreach (var staticProperty in staticSchemaProperties)
                {
                    var property = new Property();
                
                    property.Id = staticProperty;
                    property.Name = staticProperty;
                
                    switch (staticProperty)
                    {
                        case ("id"):
                            property.IsKey = true;
                            property.Type = PropertyType.String;
                            property.TypeAtSource = "string";
                            break;
                        default:
                            property.IsKey = false;
                            property.Type = PropertyType.String;
                            property.TypeAtSource = "string";
                            break;
                    }
                
                    properties.Add(property);
                }

                schema.Properties.Clear();
                schema.Properties.AddRange(properties);

                schema.DataFlowDirection = GetDataFlowDirection();

                return schema;
            }

            public async override IAsyncEnumerable<Record> ReadRecordsAsync(IApiClient apiClient, Schema schema,
                bool isDiscoverRead = false)
            {

                var client = apiClient.GetClient();

                var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Medication>());
                //var searchResultResponse = client.Search<Medication>();

                
                foreach (var entry in searchResultResponse.Entry)
                {
                    var recordMap = new Dictionary<string, object>();
                    var medication = new Medication();
                    
                    try
                    {
                        medication = (Medication) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    recordMap["id"] = medication.Id;
                    recordMap["code"] = medication.Code?.Text ?? "";
                    recordMap["form"] = medication.Form?.Text ?? "";
                    recordMap["manufacturer"] = medication.Manufacturer?.Display ?? "";
                    recordMap["status"] = medication.Status.HasValue ? medication.Status.Value.ToString() : "";
                    
                    recordMap["amount_denominator"] = medication.Amount?.Denominator.Value.ToString() ?? "";
                    recordMap["amount_denominator_unit"] = medication.Amount?.Denominator.Unit ?? "";
                    recordMap["amount_denominator_code"] = medication.Amount?.Denominator.Code ?? "";
                    
                    recordMap["amount_numerator"] = medication.Amount?.Numerator.Value.ToString() ?? "";
                    recordMap["amount_numerator_unit"] = medication.Amount?.Numerator.Unit ?? "";
                    recordMap["amount_numerator_code"] = medication.Amount?.Numerator.Code ?? "";
                    
                    
                    yield return new Record
                    {
                        Action = Record.Types.Action.Upsert,
                        DataJson = JsonConvert.SerializeObject(recordMap)
                    };
                }
            }
        }
        public static readonly Dictionary<string, Endpoint> MedicationDispenseEndpoints = new Dictionary<string, Endpoint>
        {
            {
                "MedicationDispense", new MedicationDispenseEndpoint
                {
                    Id = "MedicationDispense",
                    Name = "Medication Dispense",
                    BasePath = "MedicationDispense",
                    AllPath = "MedicationDispense",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "Medication", new MedicationEndpoint
                {
                    Id = "Medication",
                    Name = "Medication",
                    BasePath = "Medication",
                    AllPath = "Medication",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            }
        };
    }
}