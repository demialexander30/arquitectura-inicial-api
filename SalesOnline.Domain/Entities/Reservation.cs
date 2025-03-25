using SalesOnline.Domain.Core;
using System;

namespace SalesOnline.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
        
        public DateTime ReservationDate { get; set; }
        public int NumberOfPersons { get; set; }
        public string SpecialRequirements { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
