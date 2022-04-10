using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure;
using Azure.Data.Tables;

namespace Company.LocalFunction
{
    public static class LocalFunction
    {
        [FunctionName("LocalFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string response = "";
            try
            {
                string nome = req.Query["nome"];
                string cognome = req.Query["cognome"];
                string email = req.Query["email"];
                int eta = Convert.ToInt32(req.Query["eta"]);
                
                Persona persona = new Persona(nome, cognome);
                persona.Email = email;
                persona.Età = eta;
                PersonaEntity personaEntity = new PersonaEntity(persona);
                
                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                TableClient tableClient = new TableClient(connectionString, "tabellapersone");
                tableClient.AddEntity<PersonaEntity>(personaEntity);
                // PersonaEntity personaEntity = tableClient.GetEntity<PersonaEntity>(cognome, nome);
                // personaEntity.Età = 200;
                // tableClient.UpdateEntity<PersonaEntity>(personaEntity, personaEntity.ETag);
                response = "OK";
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            return new OkObjectResult(response);
        }
    }
}
