using System;

namespace SalesOnline.Infrastructure.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
