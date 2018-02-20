using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GTA.PI.Logics
{
    /// <summary>
    /// 结果
    /// </summary>
    public class MACDResult
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
        public double DIF { get; set; }
        public double DEA { get; set; }
        public double MACD { get; set; }
        /// <summary>
        /// MACD金叉
        /// </summary>
        public bool GoldX { get; set; }
        /// <summary>
        /// MACD死叉
        /// </summary>
        public bool BlackX { get; set; }
        public bool TopDepart { get; set; }
        public bool DownDepart { get; set; }
    }

    /// <summary>
    /// 结果
    /// </summary>
    public class MACDHistory
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
        public double EMAshort { get; set; }
        public double EMAlong { get; set; }
        public double DIF { get; set; }
        public double DEA { get; set; }
        public double MACD { get; set; }
        /// <summary>
        /// MACD金叉
        /// </summary>
        public bool GoldX { get; set; }
        /// <summary>
        /// MACD死叉
        /// </summary>
        public bool BlackX { get; set; }
        /// <summary>
        /// MACD顶背离
        /// </summary>
        public bool TopDepart { get; set; }
        /// <summary>
        /// MACD底背离
        /// </summary>
        public bool DownDepart { get; set; }
    }

}
