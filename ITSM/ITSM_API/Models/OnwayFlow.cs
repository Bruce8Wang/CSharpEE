using System;
using System.ComponentModel.DataAnnotations;
namespace ITSM.Models
{
    /// <summary>
    /// 在途报修流程表
    /// </summary>
    public class OnwayFlow
    {
        [Key]
        public long Id { get; set; }

        //关联的报修单
        public long RepairAppyBillId { get; set; }
        public RepairApplyBill RepairAppyBill { get; set; }

        //当前处理人
        [MaxLength(20)]
        public string CurrentDealer { get; set; }
        //public Emloyee CurrentDealer { get; set; }

        //下一处理人
        [MaxLength(20)]
        public string NextDealer { get; set; }
        //public Emloyee NextDealer { get; set; }

        //下一步处理人邮件
        [MaxLength(50)]
        public string EMail { get; set; }

        //中间处理过程
        public string DealProcess { get; set; }

        //处理方法
        public long DealMethodId { get; set; }
        public DealMethod DealMethod { get; set; }

        //处理意见
        public string DealNote { get; set; }
        //处理时间
        public DateTime DealDate { get; set; }
    }
}