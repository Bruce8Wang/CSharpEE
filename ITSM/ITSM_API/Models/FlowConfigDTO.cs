using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    public class FlowConfigDTO
    {
        [Key]
        public long Id { get; set; }
        public int Level { get; set; }
        public string Employee { get; set; }
        public string Note { get; set; }
        public string EMail { get; set; }
    }
}