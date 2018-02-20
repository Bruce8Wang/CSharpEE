using System;
using System.ComponentModel.DataAnnotations;

namespace GTA.PI.API.Models
{
    public class ChooseSolution
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string InputCondition { get; set; }

        public DateTime CreateDate { get; set; }

        // Foreign Key
        public int? UserId { get; set; }

        // Navigation property
        public User User { get; set; }

    }
}