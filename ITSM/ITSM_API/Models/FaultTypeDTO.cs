using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
	public class FaultTypeDTO
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
    }
}