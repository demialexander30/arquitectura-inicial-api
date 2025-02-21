using SalesOnline.Domain.Core;

namespace SalesOnline.Domain.Entities
{
    public class Flight : BaseEntity
    {
        public required string FlightNumber { get; set; }
        public required string Origin { get; set; }
        public required string Destination { get; set; }
    }
}
