using System;
using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
	/// <summary>
	/// 需求与问题跟踪表
	/// </summary>
	public class IssueTrack
    {
        [Key]
        public long Id { get; set; }

        //单据编号
        [MaxLength(50)]
        public string BillNo { get; set; }

        //优先级
        public string Priority { get; set; }

        // 模块
        public string Module { get; set; }

        // 问题类型
        public string IssueType { get; set; }

        // 业务提交人
        public string Submitter { get; set; }

        //业务提交部门
        public string SubmitDept { get; set; }

        //反馈日期
        public DateTime FeedbackDate { get; set; }

        //期望处理时间
        public DateTime HopeTime { get; set; }

        //实际处理时间
        public DateTime DealTime { get; set; }

        //处理状态 
        public string Status { get; set; }

        //问题详细描述 
        public string Description { get; set; }

        //解决方案
        public string Solution { get; set; }

        //故障图片路径
        public string ImagePath { get; set; }
    }
}