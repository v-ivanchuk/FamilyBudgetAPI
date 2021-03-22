using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetAPI.Models
{
    public class Family : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public ICollection<Member> Members { get; set; }
    }
}
