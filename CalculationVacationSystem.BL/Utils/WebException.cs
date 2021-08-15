using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Utils
{
    public enum IncorrectDataType
    {
        Username,
        Password
    }
    /// <summary>
    /// Custom exception for example when gets wrong data
    /// </summary>
    public class WebException : Exception
    {
        private static string _message;

        public WebException() : base(_message) {}

        public WebException(string message) : base(message) { }

        public WebException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public static void ConcreteException(IncorrectDataType dataType)
        {
            _message = dataType switch
            {
                IncorrectDataType.Username => "The username is incorrect",
                IncorrectDataType.Password => "The password is incorrect",
                _ => "Data is incorrect",
            };
        }
    }
}
