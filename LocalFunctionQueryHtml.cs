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
    public static class LocalFunctionQueryHtml
    {
        [FunctionName("LocalFunctionQueryHtml")]
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

                response =  "<html>" + 
                    "<style>table, th, td {border: 1px solid black; border-collapse: collapse;}</style>" +
                    "<body>" + 
                    "<table>" +
                    "<th>COGNOME</th>" + 
                    "<th>NOME</th>" +
                    "<th>ETA</th></tr>";

                foreach (PersonaEntity personaEntity in personaEntities)
                {
                    Persona persona = personaEntity.ToPersona();
                    response += $"<tr><td> {persona.Cognome} </td>" + 
                    $"<td>{persona.Nome}</td>" +
                    $"<td>{persona.Eta}</td></tr>";
                }
                response += "</table></body></html>";
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message);
            }

            return new ContentResult()
            {
                Content = response,
                ContentType = "text/html"
            };        }
    }
}
