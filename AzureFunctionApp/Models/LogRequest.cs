using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Models
{
    public class LogRequest
    {
        public int Id
        {
            get; set;
        }

        public string Log
        {
            get; set;
        }
    }
}
