using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BusinessException()
        {
        }
    }
}
