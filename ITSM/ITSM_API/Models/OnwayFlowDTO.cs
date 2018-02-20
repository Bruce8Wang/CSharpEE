using System;

namespace ITSM.Models
{
    public class OnwayFlowDTO
    {
        public long Id { get; set; }

        //关联的报修单
        public string RepairAppyBillNo { get; set; }
        //当前处理人
        public string CurrentDealer { get; set; }
        //下一处理人
        public string NextDealer { get; set; }
        //处理方法
        public string DealMethodName { get; set; }
        //处理意见
        public string DealNote { get; set; }
        //中间处理过程
        public string DealProcess { get; set; }
        //处理日期
        public DateTime DealDate { get; set; }
        public string EMail { get; set; }
        public string Mobile { get; set; }
    }
}