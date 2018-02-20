using System.ComponentModel.DataAnnotations;

namespace GTA.PI.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string UserName { get; set; }        
    }
}