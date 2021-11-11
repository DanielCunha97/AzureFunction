using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Services
{
    public interface ILogService
    {
        void InsertLog(string rlog);
    }
}
