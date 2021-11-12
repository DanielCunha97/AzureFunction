using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionApp.Services;
using AzureFunctionApp.Models;

namespace AzureFunctionApp
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<ActionResult<LogResponse>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string name = string.Empty;
            string finalLog = string.Empty;
            log.LogInformation("C# HTTP trigger function processed a request.");
            var successful =true;
            ILogService logService = new LogService(log);
            try
            {
                name = req.Query["log"];
                if (!string.IsNullOrEmpty(name)) //pass parameter by query
                {                  
                    dynamic data = JsonConvert.DeserializeObject(name);
                    finalLog = name ?? data?.name;
                }
                else //pass parameter by body
                {
                    finalLog = await new StreamReader(req.Body).ReadToEndAsync();
                }

                logService.InsertLog(finalLog);
                log.LogInformation("Log added to database successfully!");
            }
            catch (Exception ex)
            {
                log.LogInformation($"Exception: {ex}");
                successful = false;
            }

            return successful
                    ? (ActionResult)new OkObjectResult(logService.SelectLogs()) : new BadRequestObjectResult("Unable to process your request!");
        }
    }
}
