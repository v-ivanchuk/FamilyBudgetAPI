using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudgetAPI.Models
{
    public class Member : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int? FamilyId { get; set; }

        [ForeignKey("FamilyId")]
        public Family Family { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
