using SalesOnline.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesOnline.Domain.Entities
{
    public class Review : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        [Required]
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        public bool IsVerified { get; set; }
        
        [StringLength(1000)]
        public string Response { get; set; }
        
        public DateTime? ResponseDate { get; set; }
    }
}
