using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{

    /// <summary>
    /// 满意度定义
    /// </summary>
    public class SatisfactionLevel
    {
        [Key]
        public long Id { get; set; }
        public int Level { get; set; }

        [StringLength(20), Required(ErrorMessage = "满意度名称必须输入！")]
        public string Name { get; set; }
    }

}