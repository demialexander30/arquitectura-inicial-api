using System.ComponentModel.DataAnnotations;
using SalesOnline.Domain.Core;
using System.Collections.Generic;

namespace SalesOnline.Domain.Entities
{
    public class Customer : Person
    {
        [Required]
        [StringLength(50)]
        public string PassportNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PreferredLanguage { get; set; } = string.Empty;

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
