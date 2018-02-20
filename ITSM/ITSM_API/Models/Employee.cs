using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}