using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GTA.PI.Logics
{
    public class KDJHistory
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

        public double LLV { get; set; }
        public double HHV { get; set; }
        public double RSV { get; set; }
        public double K { get; set; }
        public double D { get; set; }
        public double J { get; set; }

        /// <summary>
        /// 金叉
        /// </summary>
        public bool GoldX { get; set; }
        /// <summary>
        /// 死叉
        /// </summary>
        public bool BlackX { get; set; }
        /// <summary>
        /// 低位金叉
        /// </summary>
        public bool LowGoldX { get; set; }
        /// <summary>
        /// 顶背离
        /// </summary>
        public bool TopDepart { get; set; }
        /// <summary>
        /// 底背离
        /// </summary>
        public bool DownDepart { get; set; }
        /// <summary>
        /// 超卖
        /// </summary>
        public bool Oversold { get; set; }
        /// <summary>
        /// 超买
        /// </summary>
        public bool Overbought { get; set; }
    }

    public class KDJResult
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

        public double K { get; set; }
        public double D { get; set; }
        public double J { get; set; }

        /// <summary>
        /// 金叉
        /// </summary>
        public bool GoldX { get; set; }
        /// <summary>
        /// 死叉
        /// </summary>
        public bool BlackX { get; set; }
        /// <summary>
        /// 低位金叉
        /// </summary>
        public bool LowGoldX { get; set; }
        /// <summary>
        /// 顶背离
        /// </summary>
        public bool TopDepart { get; set; }
        /// <summary>
        /// 底背离
        /// </summary>
        public bool DownDepart { get; set; }
        /// <summary>
        /// 超卖
        /// </summary>
        public bool Oversold { get; set; }
        /// <summary>
        /// 超买
        /// </summary>
        public bool Overbought { get; set; }
    }
}
