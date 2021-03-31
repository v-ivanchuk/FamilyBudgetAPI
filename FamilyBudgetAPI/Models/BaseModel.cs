using System;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetAPI.Models
{
    public abstract class BaseModel
    {
        [Required]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateUpdated { get; set; }
    }
}
