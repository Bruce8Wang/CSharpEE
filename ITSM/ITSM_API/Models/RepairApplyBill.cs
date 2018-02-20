using System;
using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    /// <summary>
    /// 报修单
    /// </summary>
    public class RepairApplyBill
    {
        [Key]
        public long Id { get; set; }

        //单据编号
        //[Required]
        [MaxLength(50)]
        public string BillNo { get; set; }

        [Required(ErrorMessage = "主题必须录入！")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "报修内容必须录入！")]
        public string Note { get; set; }

        [Required(ErrorMessage = "故障类型必须录入！")]
        public long FaultTypeId { get; set; }
        public FaultType FaultType { get; set; }

        [Required(ErrorMessage = "申请人必须录入！")]
        [MaxLength(20)]
        public string ApplyEmployee { get; set; }
        //public Emloyee ApplyEmployee;

        [Required(ErrorMessage = "申请人中心必须录入！")]
        [MaxLength(50)]
        public string ApplyDept { get; set; }

        [Required(ErrorMessage = "报修时间必须录入！")]
        public DateTime BXDate { get; set; }

        //故障前操作
        public string PrevOperation { get; set; }

        //紧急程度
        public long PriorityId { get; set; }
        public Priority Priority { get; set; }


        //期望处理时间
        public DateTime HopeTime { get; set; }

        //报修处理时间
        public DateTime BXDealTime { get; set; }

        //报修处理意见
        public string BXDealNote { get; set; }

        //处理过程
        public string BXDealProcess { get; set; }

        //故障图片路径
        public string ImagePath { get; set; }

        //多媒体路径
        public string VedioPath { get; set; }

        [Required(ErrorMessage = "报修人必须录入！")]
        [MaxLength(20)]
        public string BXEmployee { get; set; }
        //public Emloyee BXEmployee { get; set; }

        //报修处理人
        [MaxLength(20)]
        public string BXDealEmployee { get; set; }

        public string NextEmployee { get; set; }

        [Required(ErrorMessage = "报修人中心必须录入！")]
        [MaxLength(50)]
        public string BXDept { get; set; }

        //手机号码
        [MaxLength(50)]
        public string Phone { get; set; }

        //邮箱
        [MaxLength(50)]
        public string EMail { get; set; }

        [Required(ErrorMessage = "资产编号必须录入！")]
        public string AssetCode { get; set; }

        [MaxLength(50)]
        public string ComputerName { get; set; }

        //当前状态
        public long StatusId { get; set; }
        public Status Status { get; set; }

        //满意度          
        public long SatisfactionLevelId { get; set; }
        public SatisfactionLevel SatisfactionLevel { get; set; }

        [MaxLength(10)]
        public string DeviceType { get; set; }


    }

}
