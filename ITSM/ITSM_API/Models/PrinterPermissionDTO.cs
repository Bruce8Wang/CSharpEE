using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
	public class PrinterPermissionDTO
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Center { get; set; }
        public string Color { get; set; }
        public string FiveFloor { get; set; }
        public string SixFloor { get; set; }
        public string SevenFloor { get; set; }
        public string EighthFloor { get; set; }
        public string SecondHill { get; set; }
        public string ForthBamboo { get; set; }
        public string ThirtyfifthSZStock{ get; set; }
        public string Note { get; set; }

    }
}