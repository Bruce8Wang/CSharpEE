using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
	public class Filter
    {
        [MaxLength(20)]
        public string BeginTime { get; set; }
        [MaxLength(20)]
        public string EndTime { get; set; }
        [MaxLength(50)]
        public string BillNo { get; set; }

    }
}