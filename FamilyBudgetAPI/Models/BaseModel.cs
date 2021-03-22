using System;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetAPI.Models
{
    public class BaseModel
    {
        [Required]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
