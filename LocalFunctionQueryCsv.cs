using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Data.Tables;

namespace Company.LocalFunction
{
    public static class LocalFunctionQueryCsv
    {
        [FunctionName("LocalFunctionQueryCsv")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string response = "";
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                TableClient tableClient = new TableClient(connectionString, "tabellapersone");
                var personaEntities = tableClient.Query<PersonaEntity>();

                response =  "COGNOME;NOME;ETA\n";
                foreach (PersonaEntity personaEntity in personaEntities)
                {
                    Persona persona = personaEntity.ToPersona();
                    response += $"{persona.Cognome};{persona.Nome};{persona.Eta}\n";
                }
            }
            catch (Exception e)
            {
                response = e.Message;
                response = e.ToString();
            }

            return new ContentResult()
            {
                Content = response,
                ContentType = "text/csv"
            };
        }
    }
}
