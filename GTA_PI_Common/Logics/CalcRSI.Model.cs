using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GTA.PI.Logics
{
    public class RSIResult
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
        /// 涨跌幅
        /// </summary>
        public double ChangeRatio { get; set; }

        /// <summary>
        /// RSI6值
        /// </summary>
        public double RSI6 { get; set; }

        /// <summary>
        /// RSI12值
        /// </summary>
        public double RSI12 { get; set; }

        /// <summary>
        /// RSI值
        /// </summary>
        public double RSI { get; set; }

        /// <summary>
        /// RSI金叉
        /// </summary>
        public bool RSIGoldX { get; set; }
        /// <summary>
        /// RSI死叉
        /// </summary>
        public bool RSIBlackX { get; set; }
    }
}
