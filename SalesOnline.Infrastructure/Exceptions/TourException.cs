using System;

namespace SalesOnline.Infrastructure.Exceptions
{
    public class TourException : Exception
    {
        public TourException() : base() { }
        public TourException(string message) : base(message) { }
        public TourException(string message, Exception innerException) : base(message, innerException) { }
    }
}
