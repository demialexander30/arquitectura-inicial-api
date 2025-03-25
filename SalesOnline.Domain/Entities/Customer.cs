using SalesOnline.Domain.Core;

namespace SalesOnline.Domain.Entities
{
    public class Customer : Person
    {
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
