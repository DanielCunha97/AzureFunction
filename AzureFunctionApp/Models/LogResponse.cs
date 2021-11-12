using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Models
{
    public class LogResponse
    {
        public string Log
        {
            get; set; 
        }

        public DateTime LogDate
        {
            get; set;
        }
    }
}
