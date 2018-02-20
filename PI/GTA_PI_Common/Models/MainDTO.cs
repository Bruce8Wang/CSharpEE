using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GTA.PI.Models
{
    /// <summary>
    /// 仅仅提供指标的Id、Name，供查询智能感知使用
    /// </summary>
    [BsonIgnoreExtraElements]
    public class IndexLiteDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 最近使用的指标
    /// </summary>
    [BsonIgnoreExtraElements]
    public class IndexRankingDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int Ranking { get; set; }
    }

    /// <summary>
    /// 指标树
    /// </summary>
    [BsonIgnoreExtraElements]
    public class IndexTreeDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 一级指标类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 一级指标类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 二级指标类型代码
        /// </summary>

        public string SubTypeCode { get; set; }

        /// <summary>
        /// 二级指标类型名称
        /// </summary>
        public string SubTypeName { get; set; }

        /// <summary>
        /// 指标释义
        /// </summary>
        public string Desc { get; set; }
    }

    /// <summary>
    /// 我的选股方案及推荐选股方案
    /// </summary>
    public class MySolutionDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 股票指标的明细
    /// </summary>
    [BsonIgnoreExtraElements]
    public class IndexDetailDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// 选择的期限
        /// </summary>
        public string SelectTerm { get; set; }

        /// <summary>
        /// 选择的期限显示内容
        /// </summary>
        public string SelectTermDisplay { get; set; }

        /// <summary>
        /// 期限选择项列表
        /// </summary>
        public List<SelectOption> SelectTermList { get; set; }

        /// <summary>
        /// 具体的分布值
        /// </summary>
        public double[] Value { get; set; }

        /// <summary>
        /// 参数(用逗号隔开)
        /// (技术指标用)
        /// </summary>
        public List<InputParams> ParamsValues { get; set; }

        /// <summary>
        /// 是否默认选项
        /// </summary>
        public bool IsDefault { get; set; }
    }

    /// <summary>
    /// 期限选择项
    /// </summary>
    public class SelectOption
    {
        /// <summary>
        /// 选择项
        /// </summary>
        public string SelectItem { get; set; }

        /// <summary>
        /// 选择项显示内容
        /// </summary>
        public string SelectDisplay { get; set; }
    }

    /// <summary>
    /// 技术指标输入的参数
    /// </summary>
    public class InputParams
    {
        public double InputValue;
    }

    /// <summary>
    /// 选股结果明细
    /// </summary>
    public class StockDetailDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }

        //最新价
        public double RecentRice { get; set; }
        //涨跌幅；为百分比
        public double ChangeRate { get; set; }

        //开盘价
        public double ChangeAmount { get; set; }

        //昨收盘价
        public double LastAmount { get; set; }

        //成交额
        public double TodayAmount { get; set; }

        //成交量
        public double volume { get; set; }
    }

    /// <summary>
    /// 常量定义
    /// </summary>
    public class ConstDefine
    {
        /// <summary>
        /// 最近常用指标
        /// </summary>
        public const string CstCode_CommonlyUsedIndex = "0";

        /// <summary>
        /// 财务指标
        /// </summary>
        public const string CstCode_FinancialIndex = "1";

        /// <summary>
        /// 板块指标
        /// </summary>
        public const string CstCode_PlateIndex = "2";

        /// <summary>
        /// 行情指标
        /// </summary>
        public const string CstCode_MarketIndex = "3";

        /// <summary>
        /// 技术指标
        /// </summary>
        public const string CstCode_TechnicalIndex = "4";

        /// <summary>
        /// 无子分类
        /// </summary>
        public const string CstSubCode_None = "0";

        /// <summary>
        /// 财务指标_财务分析指标
        /// </summary>
        public const string CstSubCode_Financial_FinancialAnalysis = "1_1";

        /// <summary>
        /// 财务指标_资产负债表
        /// </summary>
        public const string CstSubCode_Financial_BalanceSheet = "1_2";

        /// <summary>
        /// 财务指标_利润表
        /// </summary>
        public const string CstSubCode_Financial_ProfitTable = "1_3";

        /// <summary>
        /// 财务指标_现金流量表
        /// </summary>
        public const string CstSubCode_Financial_CashFlowTable = "1_4";

        /// <summary>
        /// 板块指标_概念板块
        /// </summary>
        public const string CstSubCode_Plate_Concept = "2_1";

        /// <summary>
        /// 板块指标_市场板块
        /// </summary>
        public const string CstSubCode_Plate_Market = "2_2";

        /// <summary>
        /// 板块指标_地域板块
        /// </summary>
        public const string CstSubCode_Plate_Regional = "2_3";

        /// <summary>
        /// 板块指标_行业板块
        /// </summary>
        public const string CstSubCode_Plate_Industry = "2_4";

        /// <summary>
        /// MACD技术指标下拉选项MACD
        /// </summary>
        public const string Cst_MACD_MACD = "MACD";

        /// <summary>
        /// MACD技术指标下拉选项DIF
        /// </summary>
        public const string Cst_MACD_DIF = "DIF";

        /// <summary>
        /// MACD技术指标下拉选项DEA
        /// </summary>
        public const string Cst_MACD_DEA = "DEA";

        /// <summary>
        /// KDJ技术指标下拉选项K
        /// </summary>
        public const string Cst_KDJ_K = "K";

        /// <summary>
        /// KDJ技术指标下拉选项D
        /// </summary>
        public const string Cst_KDJ_D = "D";

        /// <summary>
        /// KDJ技术指标下拉选项J
        /// </summary>
        public const string Cst_KDJ_J = "J";

        /// <summary>
        /// BOLL技术指标下拉选项UP
        /// </summary>
        public const string Cst_BOLL_UP = "UP";

        /// <summary>
        /// BOLL技术指标下拉选项MB
        /// </summary>
        public const string Cst_BOLL_MB = "MB";

        /// <summary>
        /// BOLL技术指标下拉选项DN
        /// </summary>
        public const string Cst_BOLL_DN = "DN";

        /// <summary>
        /// DMI技术指标下拉选项PDI
        /// </summary>
        public const string Cst_DMI_PDI = "PDI";

        /// <summary>
        /// DMI技术指标下拉选项MDI
        /// </summary>
        public const string Cst_DMI_MDI = "MDI";

        /// <summary>
        /// DMI技术指标下拉选项ADX
        /// </summary>
        public const string Cst_DMI_ADX = "ADX";

        //"MACD", "macd顶背离", "macd底背离", "macd金叉", "macd死叉"
        public const string Cst_MACD = "MACD";
        public const string Cst_MACD_TopDepart = "MACD_TopDepart";
        public const string Cst_MACD_DownDepart = "MACD_DownDepart";
        public const string Cst_MACD_GoldX = "MACD_GoldX";
        public const string Cst_MACD_BlackX = "MACD_BlackX";
        public const string Cst_MACD_TopDepart_Name = "macd顶背离";
        public const string Cst_MACD_DownDepart_Name = "macd底背离";
        public const string Cst_MACD_GoldX_Name = "macd金叉";
        public const string Cst_MACD_BlackX_Name = "macd死叉";
        public const double Cst_MACD_Short = 12;
        public const double Cst_MACD_Long = 26;
        public const double Cst_MACD_Mid = 9;

        public const string Cst_MA = "MA";
        public const double Cst_MA_Default = 20;

        //KDJ、KDJ金叉、KDJ死叉、KDJ低位金叉、KDJ顶背离、KDJ底背离、KDJ超卖、KDJ超买
        public const string Cst_KDJ = "KDJ";
        public const string Cst_KDJ_GoldX_Name = "kdj金叉";
        public const string Cst_KDJ_BlackX_Name = "kdj死叉";
        public const string Cst_KDJ_LowGoldX_Name = "kdj低位金叉";
        public const string Cst_KDJ_TopDepart_Name = "kdj顶背离";
        public const string Cst_KDJ_DownDepart_Name = "kdj底背离";
        public const string Cst_KDJ_Oversold_Name = "kdj超卖";
        public const string Cst_KDJ_Overbought_Name = "kdj超买";
        public const string Cst_KDJ_GoldX = "KDJ_GoldX";
        public const string Cst_KDJ_BlackX = "KDJ_BlackX";
        public const string Cst_KDJ_LowGoldX = "KDJ_LowGoldX";
        public const string Cst_KDJ_TopDepart = "KDJ_TopDepart";
        public const string Cst_KDJ_DownDepart = "KDJ_DownDepart";
        public const string Cst_KDJ_Oversold = "KDJ_Oversold";
        public const string Cst_KDJ_Overbought = "KDJ_Overbought";
        public const double Cst_KDJ_N = 9;
        public const double Cst_KDJ_M1 = 3;
        public const double Cst_KDJ_M2 = 3;

        //BOLL、BOLL突破上轨、BOLL突破中轨
        public const string Cst_BOLL = "BOLL";
        public const string Cst_BOLL_Upper_Name = "boll突破上轨";
        public const string Cst_BOLL_Mid_Name = "boll突破中轨";
        public const string Cst_BOLL_Upper = "BOLL_Upper";
        public const string Cst_BOLL_Mid = "BOLL_Mid";
        public const double Cst_BOLL_MA = 20;

        //WR
        public const string Cst_WR = "WR";
        public const double Cst_WR_Default = 14;

        //DMI、BOLL突破上轨、BOLL突破中轨
        public const string Cst_DMI = "DMI";
        public const string Cst_DMI_GoldX_Name = "dmi金叉";
        public const string Cst_DMI_BlackX_Name = "dmi死叉";
        public const string Cst_DMI_GoldX = "DMI_GoldX";
        public const string Cst_DMI_BlackX = "DMI_BlackX";
        public const double Cst_DMI_N = 14;
        public const double Cst_DMI_M = 6;

        //RSI、RSI金叉、RSI死叉
        public const string Cst_RSI = "RSI";
        public const string Cst_RSI_GoldX_Name = "rsi金叉";
        public const string Cst_RSI_BlackX_Name = "rsi死叉";
        public const string Cst_RSI_GoldX = "RSI_GoldX";
        public const string Cst_RSI_BlackX = "RSI_BlackX";
        public const double Cst_RSI_Default = 14;
    }

}