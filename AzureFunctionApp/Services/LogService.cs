using AzureFunctionApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AzureFunctionApp.Services
{
    public class LogService : ILogService
    {
        public void InsertLog (string rlog)
        {
            var dbConStr = Environment.GetEnvironmentVariable("ConnectionStrings:DbConnectionStr");
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
