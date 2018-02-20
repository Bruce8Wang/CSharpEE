using System;
using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    public class RepairApplyBillDTO
    {
        public long Id { get; set; }

        //单据编号
        public string BillNo { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public long FaultTypeId { get; set; }
        public FaultType FaultType { get; set; }
        public string FaultTypeName { get; set; }
        public DateTime BXDate { get; set; }
        public string AssetCode { get; set; }
        public string ComputerName { get; set; }
        public long StatusId { get; set; }
        public Status Status { get; set; }
        public string StatusName { get; set; }
        public string BXEmployee { get; set; }
        public string BXDealEmployee { get; set; }
        public string NextEmployee { get; set; }
        public string BXDept { get; set; }
        public string ApplyEmployee { get; set; }
        public string ApplyDept { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        //故障前操作
        public string PrevOperation { get; set; }
        //紧急程度
        public long PriorityId { get; set; }
        public Priority Priority { get; set; }
        public string PriorityName { get; set; }
        //期望处理时间
        public DateTime HopeTime { get; set; }
        //报修处理意见
        public string BXDealNote { get; set; }
        //处理过程
        public string BXDealProcess { get; set; }
        //报修处理时间
        public DateTime BXDealTime { get; set; }
        //故障图片路径
        public string ImagePath { get; set; }
        //多媒体路径
        public string VedioPath { get; set; }
        //满意度
        public long SatisfactionLevelId { get; set; }
        public SatisfactionLevel SatisfactionLevel { get; set; }
        public string SatisfactionLevelName { get; set; }

        [MaxLength(10)]
        public string DeviceType { get; set; }


    }
}