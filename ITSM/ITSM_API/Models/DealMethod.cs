using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    /// <summary>
    /// 处理方法
    /// </summary>
    public class DealMethod
    {
        [Key]
        public long Id { get; set; }

        [StringLength(20), Required(ErrorMessage = "处理方法名称必须输入！")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Note { get; set; }
    }
}