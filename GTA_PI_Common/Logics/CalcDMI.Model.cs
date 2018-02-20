using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GTA.PI.Logics
{
    public class DMIHistory
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
        /// <summary>
        /// 最高价
        /// </summary>
        public double HIP { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public double LOP { get; set; }

        public double TR1 { get; set; }
        public double TR { get; set; }
        public double HD { get; set; }
        public double LD { get; set; }
        public double DMP { get; set; }
        public double DMM { get; set; }
        public double PDI { get; set; }
        public double MDI { get; set; }
        public double? ADX { get; set; }
        public double? ADXR { get; set; }

        /// <summary>
        /// 金叉
        /// </summary>
        public bool GoldX { get; set; }
        /// <summary>
        /// 死叉
        /// </summary>
        public bool BlackX { get; set; }
    }

    public class DMIResult
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
 
        public double PDI { get; set; }
        public double MDI { get; set; }
        public double? ADX { get; set; }

        /// <summary>
        /// 金叉
        /// </summary>
        public bool GoldX { get; set; }
        /// <summary>
        /// 死叉
        /// </summary>
        public bool BlackX { get; set; }
    }
}
