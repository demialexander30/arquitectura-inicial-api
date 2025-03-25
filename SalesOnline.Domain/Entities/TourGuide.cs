using SalesOnline.Domain.Core;
using System.Collections.Generic;

namespace SalesOnline.Domain.Entities
{
    public class TourGuide : Person
    {
        public string LicenseNumber { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
    }
}
