using System;
using System.Globalization;

namespace CalculationVacationSystem.BL.Utils
{
    public enum IncorrectDataType
    {
        Username,
        Password,
        NoSuchUser,
        NoRights
    }
    /// <summary>
    /// Custom exception for example when gets wrong data
    /// </summary>
    public class CVSApiException : Exception
    {
        private static string _message;

        public CVSApiException() : base(_message) { }

        public CVSApiException(string message) : base(message) { }

        public CVSApiException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public static void ConcreteException(IncorrectDataType dataType)
        {
            _message = dataType switch
            {
                IncorrectDataType.Username => "The username is incorrect",
                IncorrectDataType.Password => "The password is incorrect",
                IncorrectDataType.NoSuchUser => "There's no users with that id",
                IncorrectDataType.NoRights => "User doesn't have rights to do this actions",
                _ => "Data is incorrect",
            };
        }
    }
}
