using System;

namespace Domain.Exceptions
{
    public class InvalidDomainAccessException : Exception
    {
        public InvalidDomainAccessException(string msg) : base(msg)
        {

        }
    }
}
