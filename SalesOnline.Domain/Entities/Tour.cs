using System.ComponentModel.DataAnnotations;
using SalesOnline.Domain.Core;

namespace SalesOnline.Domain.Entities
{
    public class Tour : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public decimal Price { get; set; }
        
        [Required]
        public string Destination { get; set; } = string.Empty;
        
        public int Duration { get; set; } // Duration in days
        public int MaxGroupSize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}
