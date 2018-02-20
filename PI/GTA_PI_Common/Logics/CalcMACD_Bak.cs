//using GTA.PI.Models;
//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.Collections.Generic;

//namespace GTA.PI.Logics_Bak
//{
//    public class CalcMACD_Bak
//    {
//        /// <summary>
//        /// 计算Macd所有结果
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        public static List<MACDResult> CalcAll(List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            var temps = CalcMACD_Bak.CalcMACDMain(list, shortPara, longPara, midPara);
//            var results = new List<MACDResult>();
//            for (int i = 0; i < temps.Count; i++)
//            {
//                var r = new MACDResult() { 
//                    SecurityID = temps[i].SecurityID,
//                    TradingDate = temps[i].TradingDate,
//                    Symbol = temps[i].Symbol,
//                    DEA = temps[i].DEA,
//                    DIF = temps[i].DIF,
//                    MACD = temps[i].MACD
//                };
//                r = CalcMACD_Bak.CalcX(temps, r, i);
//                r = CalcMACD_Bak.CalcDepart(temps, r, i);
//                results.Add(r);
//            }
//            return results;
//        }
//        /// <summary>
//        /// 计算Macd所有结果
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        public static List<MACDResult> CalcAllNew(List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            // 判断MaTemp集合里面是否有今天的数据
//            // 判断
//            var temps = CalcMACD_Bak.CalcMACDMain(list, shortPara, longPara, midPara);
//            var results = new List<MACDResult>();
//            for (int i = 0; i < temps.Count; i++)
//            {
//                var r = new MACDResult()
//                {
//                    SecurityID = temps[i].SecurityID,
//                    TradingDate = temps[i].TradingDate,
//                    Symbol = temps[i].Symbol,
//                    DEA = temps[i].DEA,
//                    DIF = temps[i].DIF,
//                    MACD = temps[i].MACD
//                };
//                List<MACDTemp> xNeedList;
//                if (i - 2 >= 0)
//                {
//                    xNeedList = temps.GetRange(i - 2, 3);
//                }
//                else
//                {
//                    xNeedList = temps.GetRange(0, 1);
//                }

//                r = CalcMACD_Bak.CalcOneDayX(xNeedList, r);

//                List<MACDTemp> departNeedList;
//                if (i - 4 >= 0)
//                {
//                    departNeedList = temps.GetRange(i - 4, 5);
//                }
//                else
//                {
//                    departNeedList = temps.GetRange(0, 1);
//                }
//                r = CalcMACD_Bak.CalcOneDayDepart(departNeedList, r);
//                results.Add(r);
//            }
//            return results;
//        }
//        /// <summary>
//        /// 计算DEA、DIF和MACD
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        private static List<MACDTemp> CalcOneDayMACDMain(List<MACDTemp> results, List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            var lastMaParaIndex = list.Count - 1;
//            var r = new MACDTemp
//            {
//                SecurityID = list[lastMaParaIndex].SecurityID,
//                TradingDate = list[lastMaParaIndex].TradingDate,
//                Symbol = list[lastMaParaIndex].Symbol,
//                CP = list[lastMaParaIndex].CP
//            };
//            if (results.Count == 0)
//            {
//                r.EMA12 = list[lastMaParaIndex].CP;
//                r.EMA26 = list[lastMaParaIndex].CP;
//                r.DIF = 0;
//                r.DEA = 0;
//                r.MACD = 0;
//                results.Add(r);
//            }
//            else
//            {
//                // 最后一个结果
//                var frontMaPara = results[results.Count - 1];
//                r.EMA12 = frontMaPara.EMA12 * (shortPara - 1) / (shortPara + 1) + list[lastMaParaIndex].CP * 2 / (shortPara + 1);
//                r.EMA26 = frontMaPara.EMA26 * (longPara - 1) / (longPara + 1) + list[lastMaParaIndex].CP * 2 / (longPara + 1);
//                r.DIF = r.EMA12 - r.EMA26;
//                r.DEA = frontMaPara.DEA * (midPara - 1) / (midPara + 1) + r.DIF * 2 / (midPara + 1);
//                r.MACD = 2 * (r.DIF - r.DEA);
//                results.Add(r);
//            }
//            return results;
//        }
//        /// <summary>
//        /// 计算DEA、DIF和MACD
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        private static List<MACDTemp> CalcMACDMain(List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            var results = new List<MACDTemp>();
//            for (int i = 0; i < list.Count; i++)
//            {
//                var maPara = list[i];
//                var r = new MACDTemp
//                {
//                    SecurityID = maPara.SecurityID,
//                    TradingDate = maPara.TradingDate,
//                    Symbol = maPara.Symbol,
//                    CP = maPara.CP
//                };
//                if (i == 0)
//                {
//                    r.EMA12 = maPara.CP;
//                    r.EMA26 = maPara.CP;
//                    r.DIF = 0;
//                    r.DEA = 0;
//                    r.MACD = 0;
//                    results.Add(r);
//                    continue;
//                }
//                // 前一个计算结果
//                var frontMaPara = results[i - 1];
//                r.EMA12 = frontMaPara.EMA12 * (shortPara - 1) / (shortPara + 1) + maPara.CP * 2 / (shortPara + 1);
//                r.EMA26 = frontMaPara.EMA26 * (longPara - 1) / (longPara + 1) + maPara.CP * 2 / (longPara + 1);
//                r.DIF = r.EMA12 - r.EMA26;
//                r.DEA = frontMaPara.DEA * (midPara - 1) / (midPara + 1) + r.DIF * 2 / (midPara + 1);
//                r.MACD = 2 * (r.DIF - r.DEA);
//                results.Add(r);
//            }
//            return results;
//        }
//        /// <summary>
//        /// 计算一天的顶和底背离
//        /// </summary>
//        /// <param name="temps"></param>
//        /// <param name="r"></param>
//        /// <param name="i">4</param>
//        /// <returns></returns>
//        private static MACDResult CalcOneDayDepart(List<MACDTemp> temps, MACDResult r)
//        {
//            if (temps.Count != 5)
//            {
//                r.TopDepart = false;
//                r.DownDepart = false;
//            }
//            else
//            {
//                if (temps[4].CP > temps[3].CP && temps[3].CP > temps[2].CP && temps[2].CP > temps[1].CP && temps[1].CP > temps[0].CP)
//                {
//                    if (temps[4].MACD < temps[3].MACD && temps[4].MACD < temps[2].MACD && temps[4].MACD < temps[1].MACD && temps[4].MACD < temps[0].MACD)
//                    {
//                        r.TopDepart = true;
//                    }
//                    else
//                    {
//                        r.TopDepart = false;
//                    }
//                }
//                else
//                {
//                    r.TopDepart = false;
//                }


//                if (temps[4].CP < temps[3].CP && temps[3].CP < temps[2].CP && temps[2].CP < temps[1].CP && temps[1].CP < temps[0].CP)
//                {
//                    if (temps[4].MACD > temps[3].MACD && temps[4].MACD > temps[2].MACD && temps[4].MACD > temps[1].MACD && temps[4].MACD > temps[0].MACD)
//                    {
//                        r.DownDepart = true;
//                    }
//                    else
//                    {
//                        r.DownDepart = false;
//                    }
//                }
//                else
//                {
//                    r.DownDepart = false;
//                }
//            }
//            return r;
//        }
//        /// <summary>
//        /// 计算顶和底背离
//        /// </summary>
//        /// <param name="temps"></param>
//        /// <param name="r"></param>
//        /// <param name="i"></param>
//        /// <returns></returns>
//        private static MACDResult CalcDepart(List<MACDTemp> temps, MACDResult r, int i)
//        {
//            if (i < 4)
//            {
//                r.TopDepart = false;
//                r.DownDepart = false;
//            }
//            else
//            {
//                if (temps[i].CP > temps[i - 1].CP && temps[i - 1].CP > temps[i - 2].CP && temps[i - 2].CP > temps[i - 3].CP && temps[i - 3].CP > temps[i - 4].CP)
//                {
//                    if (temps[i].MACD < temps[i - 1].MACD && temps[i].MACD < temps[i - 2].MACD && temps[i].MACD < temps[i - 3].MACD && temps[i].MACD < temps[i - 4].MACD)
//                    {
//                        r.TopDepart = true;
//                    }
//                    else
//                    {
//                        r.TopDepart = false;
//                    }
//                }
//                else
//                {
//                    r.TopDepart = false;
//                }


//                if (temps[i].CP < temps[i - 1].CP && temps[i - 1].CP < temps[i - 2].CP && temps[i - 2].CP < temps[i - 3].CP && temps[i - 3].CP < temps[i - 4].CP)
//                {
//                    if (temps[i].MACD > temps[i - 1].MACD && temps[i].MACD > temps[i - 2].MACD && temps[i].MACD > temps[i - 3].MACD && temps[i].MACD > temps[i - 4].MACD)
//                    {
//                        r.DownDepart = true;
//                    }
//                    else
//                    {
//                        r.DownDepart = false;
//                    }
//                }
//                else
//                {
//                    r.DownDepart = false;
//                }
//            }
//            return r;
//        }
//        /// <summary>
//        /// 计算一天的金叉和死叉
//        /// </summary>
//        /// <param name="temps"></param>
//        /// <param name="r"></param>
//        /// <param name="i"></param>
//        /// <returns></returns>
//        private static MACDResult CalcOneDayX(List<MACDTemp> temps, MACDResult r)
//        {
//            if (temps.Count != 3)
//            {
//                r.GoldX = false;
//                r.BlackX = false;
//            }
//            else
//            {
//                if (temps[1].DIF == temps[1].DEA)
//                {
//                    if (temps[0].DIF < temps[0].DEA && temps[2].DIF > temps[2].DEA)
//                    {
//                        r.GoldX = true;
//                    }
//                    else
//                    {
//                        r.GoldX = false;
//                    }

//                    if (temps[0].DIF > temps[0].DEA && temps[2].DIF < temps[2].DEA)
//                    {
//                        r.BlackX = true;
//                    }
//                    else
//                    {
//                        r.BlackX = false;
//                    }
//                }
//                else
//                {
//                    if (temps[1].DIF < temps[1].DEA && temps[2].DIF > temps[2].DEA)
//                    {
//                        r.GoldX = true;
//                    }
//                    else
//                    {
//                        r.GoldX = false;
//                    }

//                    if (temps[1].DIF > temps[1].DEA && temps[2].DIF < temps[2].DEA)
//                    {
//                        r.BlackX = true;
//                    }
//                    else
//                    {
//                        r.BlackX = false;
//                    }
//                }
//            }
//            return r;
//        }
//        /// <summary>
//        /// 计算金叉和死叉
//        /// </summary>
//        /// <param name="temps"></param>
//        /// <param name="r"></param>
//        /// <param name="i"></param>
//        /// <returns></returns>
//        private static MACDResult CalcX(List<MACDTemp> temps, MACDResult r, int i)
//        {
//            if (i < 3 || i + 1 == temps.Count)
//            {
//                r.GoldX = false;
//                r.BlackX = false;
//            }
//            else
//            {
//                if (temps[i - 1].DIF == temps[i - 1].DEA)
//                {
//                    if (temps[i - 2].DIF < temps[i - 2].DEA && temps[i].DIF > temps[i].DEA)
//                    {
//                        r.GoldX = true;
//                    }
//                    else
//                    {
//                        r.GoldX = false;
//                    }

//                    if (temps[i - 2].DIF > temps[i - 2].DEA && temps[i].DIF < temps[i].DEA)
//                    {
//                        r.BlackX = true;
//                    }
//                    else
//                    {
//                        r.BlackX = false;
//                    }
//                }
//                else
//                {
//                    if (temps[i - 1].DIF < temps[i - 1].DEA && temps[i].DIF > temps[i].DEA)
//                    {
//                        r.GoldX = true;
//                    }
//                    else
//                    {
//                        r.GoldX = false;
//                    }

//                    if (temps[i - 1].DIF > temps[i - 1].DEA && temps[i].DIF < temps[i].DEA)
//                    {
//                        r.BlackX = true;
//                    }
//                    else
//                    {
//                        r.BlackX = false;
//                    }
//                }
//            }
//            return r;
//        }
//    }
//    [BsonIgnoreExtraElements]
//    public class MACDPara
//    {
//        /// <summary>
//        /// 股票安全码
//        /// </summary>
//        public ulong SecurityID { get; set; }
//        /// <summary>
//        /// 交易日
//        /// </summary>
//        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
//        public DateTime TradingDate { get; set; }
//        /// <summary>
//        /// 股票编号
//        /// </summary>
//        public string Symbol { get; set; }
//        /// <summary>
//        /// 收盘价
//        /// </summary>
//        public double? CP { get; set; }
//        /// <summary>
//        /// 最低价
//        /// </summary>
//        public double? LOP { get; set; }

//        /// <summary>
//        /// 最高价
//        /// </summary>
//        public double? HIP { get; set; }
//    }
//    public class MACDTemp
//    {
//        /// <summary>
//        /// 股票安全码
//        /// </summary>
//        public ulong SecurityID { get; set; }
//        /// <summary>
//        /// 交易日
//        /// </summary>
//        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
//        public DateTime TradingDate { get; set; }
//        /// <summary>
//        /// 股票编号
//        /// </summary>
//        public string Symbol { get; set; }
//        /// <summary>
//        /// 收盘价
//        /// </summary>
//        public double? CP { get; set; }
//        public double? EMA12 { get; set; }
//        public double? EMA26 { get; set; }
//        public double? DIF { get; set; }
//        public double? DEA { get; set; }
//        public double? MACD { get; set; }
//    }
//    public class MACDResult
//    {
//        /// <summary>
//        /// 股票安全码
//        /// </summary>
//        public ulong SecurityID { get; set; }
//        /// <summary>
//        /// 交易日
//        /// </summary>
//        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
//        public DateTime TradingDate { get; set; }
//        /// <summary>
//        /// 股票编号
//        /// </summary>
//        public string Symbol { get; set; }
//        public double? DIF { get; set; }
//        public double? DEA { get; set; }
//        public double? MACD { get; set; }
//        public bool GoldX { get; set; }
//        public bool BlackX { get; set; }
//        public bool TopDepart { get; set; }
//        public bool DownDepart { get; set; }
//    }
//}
