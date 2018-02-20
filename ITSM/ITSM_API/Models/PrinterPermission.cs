using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
	public class PrinterPermission
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Center { get; set; }
        public bool Color { get; set; }
        public bool FiveFloor { get; set; }
        public bool SixFloor { get; set; }
        public bool SevenFloor { get; set; }
        public bool EighthFloor { get; set; }
        public bool SecondHill { get; set; }
        public bool ForthBamboo { get; set; }
        public bool ThirtyfifthSZStock { get; set; }
        public string Note { get; set; }

    }
}