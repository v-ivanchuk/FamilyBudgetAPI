using System;

namespace FamilyBudgetAPI.Models
{
    public class MemberFilter : Pagination
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int FamilyId { get; set; }

        public FamilyFilter Family { get; set; }
    }
}
