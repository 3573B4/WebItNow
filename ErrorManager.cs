using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebItNow_Peacock
{
    public class ErrorManager
    {
        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }

        public void LogError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }

        public void ClearError()
        {
            ErrorMessage = string.Empty;
            HasError = false;
        }

    }
}