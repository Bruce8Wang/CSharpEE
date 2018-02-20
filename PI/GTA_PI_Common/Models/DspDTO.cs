using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GTA.PI.Models
{
    /// <summary>
    /// 财务指标信息
    /// </summary>
    public class FinanceIndexInfo
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)

        /// <summary>
        /// 股票安全码
        /// </summary>
        public ulong SecurityID { get; set; }

        /// <summary>
        /// 股票编号
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 指标编码
        /// </summary>
        public string IndexCode { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 截止日期名称
        /// </summary>
        public string EndDateName { get; set; }

        /// <summary>
        /// 指标值
        /// </summary>
        public double IndexValue { get; set; }
    }

    /// <summary>
    /// 股票信息
    /// </summary>
    public class SymbolInfo
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)

        /// <summary>
        /// 股票安全码
        /// </summary>
        public ulong SecurityID { get; set; }

        ///// <summary>
        ///// 股票编号
        ///// </summary>
        //public string Symbol { get; set; }

        //public string FullName { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Market { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime? EnterDate { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime? OutDate { get; set; }
        //public string UpdateState { get; set; }
    }

    /// <summary>
    /// 行情指标信息
    /// </summary>
    public class DataByTimeIndexInfo
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)
        /// <summary>
        /// 股票安全码
        /// </summary>
        public ulong SecurityID { get; set; }

        /// <summary>
        /// 股票编号
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 指标编码
        /// </summary>
        public string IndexCode { get; set; }

        /// <summary>
        /// 交易日
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TradingDate { get; set; }

        /// <summary>
        /// 交易日名称
        /// </summary>
        public string TradingDateName { get; set; }

        /// <summary>
        /// 指标值
        /// </summary>
        public double IndexValue { get; set; }

        /// <summary>
        /// 填充判断 Filling=0：正常数据；其他：填充数据。
        /// </summary>
        public string Filling { get; set; }
    }

    /// <summary>
    /// 交易日信息
    /// </summary>
    public class TradeCalendarInfo
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)

        /// <summary>
        /// 交易所
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// 是否开市
        /// </summary>
        public string IsOpen { get; set; }

        /// <summary>
        /// 交易日日期
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CalendarDate { get; set; }
    }

    /// <summary>
    /// 板块下的股票信息
    /// </summary>
    public class PlateSymbolInfo
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)

        /// <summary>
        /// 所属板块ID
        /// </summary>
        public Int64 PlateID { get; set; }

        /// <summary>
        /// 股票信息
        /// </summary>
        public IEnumerable<ulong> SecurityIDs { get; set; }
    }

    /// <summary>
    /// 板块指标信息
    /// </summary>
    public class PlateInfo
    {
        public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)
        /// <summary>
        /// 板块ID
        /// </summary>
        public Int64 PlateID { get; set; }

        /// <summary>
        /// 父板块ID
        /// </summary>
        public Int64 PlateTreeID { get; set; }

        /// <summary>
        /// 板块标题
        /// </summary>
        public string PlateTitle { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public string NodeLevel { get; set; }
    }

    /// <summary>
    /// 全部行情指标
    /// </summary>
    public class DataByTime
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
        /// 证券名称
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 填充判断 Filling=0：正常数据；其他：填充数据。
        /// </summary>
        public string Filling { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public double? OP { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public double? CP { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public double? HIP { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public double? LOP { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public double? ChangeRatio { get; set; }


        ///// <summary>
        ///// 区间成交量
        ///// </summary>
        //public UInt64? CQ { get; set; }
        ///// <summary>
        ///// 区间成交额
        ///// </summary>
        //public double? CM { get; set; }
        ///// <summary>
        ///// 振幅
        ///// </summary>
        //public double? Amplitude { get; set; }
       
        ///// <summary>
        ///// 换手率
        ///// </summary>
        //public double? TurnoverRate { get; set; }
        ///// <summary>
        ///// 流通市值
        ///// </summary>
        //public double? CMV { get; set; }
        ///// <summary>
        ///// 总市值
        ///// </summary>
        //public double? TMV { get; set; }
        ///// <summary>
        ///// 流通股本
        ///// </summary>
        //public double? CirShares { get; set; }

        //public double? OBPD { get; set; }
        //public double? Vwap { get; set; }
        //public double? LCP { get; set; }
        //public double? LIMITUP { get; set; }
        //public double? LIMITDOWN { get; set; }
        //public double? Change { get; set; }
        //public double? SP { get; set; }
        //public double? LSP { get; set; }
        //public double? Position { get; set; }
        //public double? ContractUnit { get; set; }
        //public double? MarginUnit { get; set; }
        //public double? MaintainingMargin { get; set; }
        //public double? AccruedInterest { get; set; }
    }

    /// <summary>
    /// 实时行情
    /// </summary>
    [BsonIgnoreExtraElements]
    public class L1QuoteInfo
    {
        //public ObjectId _id; // 这个对应了 MongoDB.Bson.ObjectId (插入新数据不需要加这个字段，用于查询的)
        /// <summary>
        /// 股票安全码
        /// </summary>
        public ulong SecurityID { get; set; }
        /// <summary>
        /// 证券代码
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 证券名称
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 现价/最新成交价
        /// </summary>
        public double LastPrice { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public double ChangeRatio { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public double OpenPrice { get; set; }
        /// <summary>
        /// 昨收盘价
        /// </summary>
        public double PreClosePrice { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public double TotalVolume { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public double TotalAmount { get; set; }
    }
}