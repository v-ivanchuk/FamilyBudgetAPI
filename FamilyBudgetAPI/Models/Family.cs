using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetAPI.Models
{
    public class Family : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Member> Members { get; set; }
    }
}
