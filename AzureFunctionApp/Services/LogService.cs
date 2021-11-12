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
                command.Parameters.Add(new SqlParameter("@date", DateTime.UtcNow));
                command.Parameters.Add(new SqlParameter("@log", logRequest.Log));         
                command.ExecuteNonQuery();
            }
        }

        public List<LogResponse> SelectLogs()
        {
            List<LogResponse> logs = new List<LogResponse>();
            var dbConStr = Environment.GetEnvironmentVariable("DbConnectionStr");
            _log.LogInformation("Connection string: " + dbConStr);
            using (var connection = new SqlConnection(dbConStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SelectLogs", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandTimeout = 60
                };
                SqlDataReader rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {      
                    while (rdr.Read())
                    {
                        _log.LogInformation("Log: " + rdr["logreq"].ToString().Trim());
                        Console.WriteLine("Log: " + rdr["logreq"].ToString().Trim());
                        _log.LogInformation("Log Date: " + rdr.GetDateTime(2));
                        Console.WriteLine("Log Date: " + rdr.GetDateTime(2));
                        logs.Add(new LogResponse
                        {
                            Log = rdr["logreq"].ToString().Trim(),
                            LogDate = Convert.ToDateTime(rdr["logDate"])
                        });
                    }
                }
                else
                {
                    _log.LogInformation("No rows found.");
                }

                rdr.Close();
            }

            return logs;
        }
    }
}
