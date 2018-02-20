using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GTA.PI.Logics
{
    public class BOLLResult
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)

        /// <summary>
        /// 股票安全码
        /// </summary>
        public ulong SecurityID { get; set; }
        /// <summary>
        /// 交易日
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TradingDate { get; set; }
        /// <summary>
        /// 股票编号
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public double CP { get; set; }

        public double MA { get; set; }
        public double MD { get; set; }

        public double UP { get; set; }
        public double DN { get; set; }
        public double MB { get; set; }

        /// <summary>
        /// 突破上轨
        /// </summary>
        public bool Upper { get; set; }

        /// <summary>
        /// 突破中轨
        /// </summary>
        public bool Mid { get; set; }
    }
}
