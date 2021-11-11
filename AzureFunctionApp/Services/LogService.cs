using AzureFunctionApp.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AzureFunctionApp.Services
{
    public class LogService : ILogService
    {
        private ILogger _log;
        public LogService(ILogger log)
        {
            _log = log;
        }

        public void InsertLog (string rlog)
        {
            var dbConStr = Environment.GetEnvironmentVariable("DbConnectionStr");
            _log.LogInformation("Connection string: " + dbConStr);
            using (var connection = new SqlConnection(dbConStr))
            {
                LogRequest logRequest = new LogRequest();
                connection.Open();
                if (rlog.Contains("{"))
                {
                    logRequest = JsonConvert.DeserializeObject<LogRequest>(rlog);
                }
                else
                {
                    logRequest.Log = rlog.Replace("/", string.Empty).Replace("\"", string.Empty);
                }
                SqlCommand command = new SqlCommand("InsertLogValue", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@id", logRequest.Id));
                command.Parameters.Add(new SqlParameter("@log", logRequest.Log));
                command.ExecuteNonQuery();
            }
        }
    }
}
