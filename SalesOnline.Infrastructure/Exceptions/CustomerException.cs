using System;

namespace SalesOnline.Infrastructure.Exceptions
{
    public class CustomerException : Exception
    {
        public CustomerException() : base() { }
        public CustomerException(string message) : base(message) { }
        public CustomerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
