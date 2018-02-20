using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    /// <summary>
    /// 故障类型
    /// </summary>
    public class FaultType
    {
        [Key]
        public long Id { get; set; }
        [StringLength(20), Required(ErrorMessage = "故障类型必须输入！")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
    }
}