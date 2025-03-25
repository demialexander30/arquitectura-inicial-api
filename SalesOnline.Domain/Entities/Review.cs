using SalesOnline.Domain.Core;
using System;

namespace SalesOnline.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
        
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsVerified { get; set; }
        public string Response { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
