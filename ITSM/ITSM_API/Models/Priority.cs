using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    /// <summary>
    /// 紧急度定义
    /// </summary>
    public class Priority
    {
        [Key]
        public long Id { get; set; }
        public int Level { get; set; }

        [StringLength(20), Required(ErrorMessage = "紧急度名称必须输入！")]
        public string Name { get; set; }
    }
}