using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    /// <summary>
    /// 报修状态表
    /// </summary>
    public class Status
    {
        [Key]
        public long Id { get; set; }

        [StringLength(20), Required(ErrorMessage = "状态名称必须输入！")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
    }
}