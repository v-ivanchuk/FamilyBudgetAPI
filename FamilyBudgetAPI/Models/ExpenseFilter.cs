using System;

namespace FamilyBudgetAPI.Models
{
    public class ExpenseFilter : Pagination
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public decimal Value { get; set; }

        public string Description { get; set; }

        public DateTime ExpenseDate { get; set; }

        public int MemberId { get; set; }

        public MemberFilter Member { get; set; }
    }
}
