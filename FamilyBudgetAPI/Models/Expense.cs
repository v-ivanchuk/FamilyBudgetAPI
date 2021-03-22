using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudgetAPI.Models
{
    public class Expense : BaseModel
    {
        [Required]
        public decimal? Value { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        public int? MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member Member { get; set; }
    }
}
