using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    /// <summary>
    /// 流程配置
    /// </summary>
    public class FlowConfig
    {
        [Key]
        public long Id { get; set; }
        public int Level { get; set; }

        [StringLength(20), Required(ErrorMessage = "必须输入处理人！")]
        public string Dealer { get; set; }
        //public Emloyee Dealer { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public string EMail { get; set; }

    }
}