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
    public static class LocalFunctionDelete
    {
        [FunctionName("LocalFunctionDelete")]
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

                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                TableClient tableClient = new TableClient(connectionString, "tabellapersone");
                PersonaEntity personaEntity = await tableClient.GetEntityAsync<PersonaEntity>(cognome, nome);
                await tableClient.DeleteEntityAsync(cognome, nome, personaEntity.ETag);

                response = "Ok";
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message);
            }

            return new OkObjectResult(response);
        }
    }
}
