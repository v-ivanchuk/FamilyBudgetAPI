using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudgetAPI.Models
{
    public class Member : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int FamilyId { get; set; }

        [ForeignKey("FamilyId")]
        public Family Family { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
