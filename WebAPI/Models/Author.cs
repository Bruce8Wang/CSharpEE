using System.ComponentModel.DataAnnotations;
namespace com.example.demo.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
