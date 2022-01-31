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
    public class PatientEndpointHelper
    {
        private class PatientEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                //Supporting endpoints needed:
                //patient.GeneralPractitioner
                
                
                
                
                //patient.Link //probably not needed here, links to other relevant patients
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", //.net ID datatype, string
                    "active", //bool
                    "deceased", //custom datatype, string
                    "gender", //custom datatype, string
                    "birthdate", //string
                    "managing_organization", //string, but is unique dt
                    "marital_status", //string, but is unique dt
                    "multiple_birth", //bool, but is unique dt
                    "text", //string, likely system field
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
                        case ("active"):
                            property.IsKey = false;
                            property.Type = PropertyType.Bool;
                            property.TypeAtSource = "bool";
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var recordMap = new Dictionary<string, object>();
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    recordMap["id"] = patient.Id;
                    recordMap["active"] = patient.Active ?? false;
                    recordMap["deceased"] = patient.Deceased?.ToString() ?? "";
                    recordMap["gender"] = patient.Gender?.ToString() ?? AdministrativeGender.Unknown.ToString();
                    recordMap["birthdate"] = patient.BirthDate;
                    recordMap["managing_organization"] = patient.ManagingOrganization?.ToString() ?? "";
                    recordMap["marital_status"] = patient.MaritalStatus?.ToString() ?? "";
                    recordMap["text"] = patient.Text?.ToString() ?? "";
                    recordMap["multiple_birth"] = patient.MultipleBirth?.ToString() ?? "";
                    
                    
                    yield return new Record
                    {
                        Action = Record.Types.Action.Upsert,
                        DataJson = JsonConvert.SerializeObject(recordMap)
                    };
                }
            }
        }
        private class PatientAddressEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id",
                    "city",
                    "country",
                    "district",
                    "state",
                    "text",
                    "postal_code",
                    "use"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    
                    foreach (var addressEntry in patient.Address)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";

                        recordMap["id"] = addressEntry.ElementId ?? "";
                        recordMap["city"] = addressEntry.City ?? "";
                        recordMap["country"] = addressEntry.Country ?? "";
                        recordMap["district"] = addressEntry.District ?? "";
                        recordMap["state"] = addressEntry.State ?? "";
                        recordMap["text"] = addressEntry.Text ?? "";
                        recordMap["postal_code"] = addressEntry.PostalCode ?? "";
                        recordMap["use"] = addressEntry.Use?.ToString() ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        
        private class PatientIdentifiersEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "value", 
                    "system", 
                    "assigner", 
                    "use"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = await client.SearchAsync<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    
                    foreach (var identifierEntry in patient.Identifier)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";

                        recordMap["id"] = identifierEntry.ElementId ?? "";
                        recordMap["value"] = identifierEntry.Value ?? "";
                        recordMap["system"] = identifierEntry.System ?? "";
                        recordMap["assigner"] = identifierEntry.Assigner?.ToString() ?? "";
                        recordMap["use"] = identifierEntry.Use?.ToString() ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        private class PatientCommunicationEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "preferred", 
                    "language"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    
                    foreach (var communicationEntry in patient.Communication)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";

                        recordMap["id"] = communicationEntry.ElementId ?? "";
                        recordMap["preferred"] = communicationEntry.Preferred?.ToString() ?? "";
                        recordMap["language"] = communicationEntry.Language.ToString() ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        private class PatientContactEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "address", 
                    "gender",
                    "name",
                    "organization",
                    "relationship"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    
                    foreach (var contactEntry in patient.Contact)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";

                        recordMap["id"] = contactEntry.ElementId ?? "";
                        recordMap["address"] = contactEntry.Address?.ToString() ?? "";
                        recordMap["gender"] = contactEntry.Gender?.ToString() ?? "";
                        recordMap["name"] = contactEntry.Name?.ToString() ?? "";
                        recordMap["organization"] = contactEntry.Organization?.ToString() ?? "";
                        recordMap["relationship"] = contactEntry.Relationship[0]?.Text ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        private class PatientNamesEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "family", 
                    "text",
                    "use",
                    "prefix",
                    "suffix",
                    "given"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    
                    foreach (var namesEntry in patient.Name)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";

                        recordMap["id"] = namesEntry.ElementId ?? "";
                        recordMap["family"] = namesEntry.Family ?? "";
                        recordMap["text"] = namesEntry.Text ?? "";
                        recordMap["use"] = namesEntry.Use?.ToString() ?? "";
                        recordMap["prefix"] = string.Join(' ', namesEntry.Prefix) ?? "";
                        recordMap["suffix"] = string.Join(' ', namesEntry.Suffix) ?? "";
                        recordMap["given"] = string.Join(' ', namesEntry.Given) ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        private class PatientTelecomsEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "use",
                    "rank",
                    "system",
                    "value"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    
                    foreach (var telecomsEntry in patient.Telecom)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";

                        recordMap["id"] = telecomsEntry.ElementId ?? "";
                        recordMap["use"] = telecomsEntry.Use?.ToString() ?? "";
                        recordMap["rank"] = telecomsEntry.Rank?.ToString() ?? "";
                        recordMap["system"] = telecomsEntry.System?.ToString() ?? "";
                        recordMap["value"] = telecomsEntry.Value ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        private class PatientContactTelecomsEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "contact_id", 
                    "use",
                    "rank",
                    "system",
                    "value"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    foreach (var contactEntry in patient.Contact)
                    {
                        foreach (var telecomsEntry in contactEntry.Telecom)
                        {
                            var recordMap = new Dictionary<string, object>();

                            recordMap["patient_id"] = patient.Id ?? "";
                            recordMap["contact_id"] = contactEntry.ElementId ?? "";
                            
                            recordMap["id"] = telecomsEntry.ElementId ?? "";
                            recordMap["use"] = telecomsEntry.Use?.ToString() ?? "";
                            recordMap["rank"] = telecomsEntry.Rank?.ToString() ?? "";
                            recordMap["system"] = telecomsEntry.System?.ToString() ?? "";
                            recordMap["value"] = telecomsEntry.Value ?? "";
                            
                            yield return new Record
                            {
                                Action = Record.Types.Action.Upsert,
                                DataJson = JsonConvert.SerializeObject(recordMap)
                            };
                        }
                    }
                }
            }
        }
        private class PatientGeneralPractitionersEndpoint : Endpoint
        {
            public override async Task<Schema> GetStaticSchemaAsync(IApiClient apiClient, Schema schema)
            {
                List<string> staticSchemaProperties = new List<string>()
                {
                    "id", 
                    "patient_id", 
                    "display", 
                    "identifier_assigner",
                    "identifier_system",
                    "identifier_value",
                    "reference"
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
                        case ("patient_id"):
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

               // var searchResultResponse = await System.Threading.Tasks.Task.Run(() => client.Search<Patient>());
                var searchResultResponse = client.Search<Patient>();

                foreach (var entry in searchResultResponse.Entry)
                {
                    var patient = new Patient();
                    try
                    {
                        patient = (Patient) entry.Resource;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    foreach (var generalPractitionerEntry in patient.GeneralPractitioner)
                    {
                        var recordMap = new Dictionary<string, object>();

                        recordMap["patient_id"] = patient.Id ?? "";
                        
                        recordMap["id"] = generalPractitionerEntry.ElementId ?? "";
                        recordMap["display"] = generalPractitionerEntry.Display ?? "";
                        recordMap["identifier_assigner"] = generalPractitionerEntry.Identifier?.Assigner?.ToString() ?? "";
                        recordMap["identifier_system"] = generalPractitionerEntry.Identifier?.System ?? "";
                        recordMap["identifier_value"] = generalPractitionerEntry.Identifier?.Value ?? "";
                        recordMap["reference"] = generalPractitionerEntry.Reference ?? "";
                        
                        yield return new Record
                        {
                            Action = Record.Types.Action.Upsert,
                            DataJson = JsonConvert.SerializeObject(recordMap)
                        };
                    }
                }
            }
        }
        public static readonly Dictionary<string, Endpoint> PatientEndpoints = new Dictionary<string, Endpoint>
        {
            {
                "AllPatients", new PatientEndpoint
                {
                    Id = "AllPatients",
                    Name = "All Patients",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientNames", new PatientNamesEndpoint()
                {
                    Id = "PatientNames",
                    Name = "Patient Names",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientIdentifiers", new PatientIdentifiersEndpoint
                {
                    Id = "PatientIdentifiers",
                    Name = "Patient Identifiers",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientAddresses", new PatientAddressEndpoint
                {
                    Id = "PatientAddresses",
                    Name = "Patient Addresses",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientCommunication", new PatientCommunicationEndpoint
                {
                    Id = "PatientCommunication",
                    Name = "Patient Communication",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientContact", new PatientContactEndpoint
                {
                    Id = "PatientContact",
                    Name = "Patient Contact",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientTelecoms", new PatientTelecomsEndpoint
                {
                    Id = "PatientTelecoms",
                    Name = "Patient Telecoms",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientContactTelecoms", new PatientContactTelecomsEndpoint
                {
                    Id = "PatientContactTelecoms",
                    Name = "Patient Contact Telecoms",
                    BasePath = "Patients",
                    AllPath = "Patients",
                    ShouldGetStaticSchema = true,
                    SupportedActions = new List<EndpointActions>
                    {
                        EndpointActions.Get
                    }
                }
            },
            {
                "PatientGeneralPractitioners", new PatientGeneralPractitionersEndpoint()
                {
                    Id = "PatientGeneralPractitioners",
                    Name = "Patient General Practitioners",
                    BasePath = "Patients",
                    AllPath = "Patients",
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