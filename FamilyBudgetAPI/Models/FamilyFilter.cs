using System;

namespace FamilyBudgetAPI.Models
{
    public class FamilyFilter : Pagination
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Name { get; set; }
    }
}
