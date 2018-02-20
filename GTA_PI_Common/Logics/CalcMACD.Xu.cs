//using GTA.PI.Models;
//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.Collections.Generic;

//namespace GTA.PI.Logics
//{
//    public class CalcMACD_XU
//    {
//        /// <summary>
//        /// 计算Macd所有结果
//        /// </summary>
//        /// <param name="list">某个股票所有的日行情数据</param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        public static List<MACDResult> CalcAll(List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            // 判断MaTemp集合里面是否有今天的数据
//            // 判断
//            var temps = CalcMACD_XU.CalcMACDMain(list, shortPara, longPara, midPara);
//            for (int i = 0; i < temps.Count; i++)
//            {
//                CalcMACD_XU.CalcOneDayX(temps, i);
//                CalcMACD_XU.CalcOneDayDepart(temps, i);
//            }
//            return temps;
//        }
//        /// <summary>
//        /// 计算Macd一天的结果
//        /// </summary>
//        /// <param name="results"></param>
//        /// <param name="macdPara"></param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        public static List<MACDResult> CalcOneDayMACD(List<MACDResult> results, MACDPara macdPara, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            CalcMACD_XU.CalcOneDayMACDMain(results, macdPara, shortPara, longPara, midPara);
//            CalcMACD_XU.CalcOneDayX(results, results.Count - 1);
//            CalcMACD_XU.CalcOneDayDepart(results, results.Count - 1);
//            //while (results.Count > 5)
//            //{
//            //    results.RemoveAt(0);
//            //}
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
//        private static void CalcOneDayMACDMain(List<MACDResult> results, MACDPara macdPara, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            var r = new MACDResult
//            {
//                SecurityID = macdPara.SecurityID,
//                TradingDate = macdPara.TradingDate,
//                Symbol = macdPara.Symbol,
//                CP = macdPara.CP
//            };
//            if (results.Count == 0)
//            {
//                r.EMA12 = macdPara.CP;
//                r.EMA26 = macdPara.CP;
//                r.DIF = 0;
//                r.DEA = 0;
//                r.MACD = 0;
//                results.Add(r);
//            }
//            else
//            {
//                // 最后一个结果
//                var frontMaPara = results[results.Count - 1];
//                r.EMA12 = frontMaPara.EMA12 * (shortPara - 1) / (shortPara + 1) + macdPara.CP * 2 / (shortPara + 1);
//                r.EMA26 = frontMaPara.EMA26 * (longPara - 1) / (longPara + 1) + macdPara.CP * 2 / (longPara + 1);
//                r.DIF = r.EMA12 - r.EMA26;
//                r.DEA = frontMaPara.DEA * (midPara - 1) / (midPara + 1) + r.DIF * 2 / (midPara + 1);
//                r.MACD = 2 * (r.DIF - r.DEA);
//                results.Add(r);
//            }
//        }
//        /// <summary>
//        /// 计算DEA、DIF和MACD
//        /// </summary>
//        /// <param name="list"></param>
//        /// <param name="shortPara"></param>
//        /// <param name="longPara"></param>
//        /// <param name="midPara"></param>
//        /// <returns></returns>
//        private static List<MACDResult> CalcMACDMain(List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            var results = new List<MACDResult>();
//            for (int i = 0; i < list.Count; i++)
//            {
//                //var maPara = list[i];
//                var r = new MACDResult
//                {
//                    SecurityID = list[i].SecurityID,
//                    TradingDate = list[i].TradingDate,
//                    Symbol = list[i].Symbol,
//                    CP = list[i].CP
//                };
//                if (results.Count == 0)
//                {
//                    r.EMA12 = list[i].CP;
//                    r.EMA26 = list[i].CP;
//                    r.DIF = 0;
//                    r.DEA = 0;
//                    r.MACD = 0;
//                }
//                else
//                {
//                    // 最后一个计算结果
//                    var lastOne = results[results.Count - 1];
//                    r.EMA12 = lastOne.EMA12 * (shortPara - 1) / (shortPara + 1) + list[i].CP * 2 / (shortPara + 1);
//                    r.EMA26 = lastOne.EMA26 * (longPara - 1) / (longPara + 1) + list[i].CP * 2 / (longPara + 1);
//                    r.DIF = r.EMA12 - r.EMA26;
//                    r.DEA = lastOne.DEA * (midPara - 1) / (midPara + 1) + r.DIF * 2 / (midPara + 1);
//                    r.MACD = 2 * (r.DIF - r.DEA);
//                }
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
//        private static void CalcOneDayDepart(List<MACDResult> result, int i)
//        {
//            List<MACDResult> needList;
//            if (i - 4 >= 0)
//            {
//                needList = result.GetRange(i - 4, 5);
//            }
//            else
//            {
//                needList = result.GetRange(0, 1);
//            }
//            if (needList.Count != 5)
//            {
//                result[i].TopDepart = false;
//                result[i].DownDepart = false;
//            }
//            else
//            {
//                if (needList[4].CP > needList[3].CP && needList[3].CP > needList[2].CP && needList[2].CP > needList[1].CP && needList[1].CP > needList[0].CP)
//                {
//                    if (needList[4].MACD < needList[3].MACD && needList[4].MACD < needList[2].MACD && needList[4].MACD < needList[1].MACD && needList[4].MACD < needList[0].MACD)
//                    {
//                        result[i].TopDepart = true;
//                    }
//                    else
//                    {
//                        result[i].TopDepart = false;
//                    }
//                }
//                else
//                {
//                    result[i].TopDepart = false;
//                }


//                if (needList[4].CP < needList[3].CP && needList[3].CP < needList[2].CP && needList[2].CP < needList[1].CP && needList[1].CP < needList[0].CP)
//                {
//                    if (needList[4].MACD > needList[3].MACD && needList[4].MACD > needList[2].MACD && needList[4].MACD > needList[1].MACD && needList[4].MACD > needList[0].MACD)
//                    {
//                        result[i].DownDepart = true;
//                    }
//                    else
//                    {
//                        result[i].DownDepart = false;
//                    }
//                }
//                else
//                {
//                    result[i].DownDepart = false;
//                }
//            }
//        }
//        /// <summary>
//        /// 计算一天的金叉和死叉
//        /// </summary>
//        /// <param name="temps"></param>
//        /// <param name="r"></param>
//        /// <param name="i"></param>
//        /// <returns></returns>
//        private static void CalcOneDayX(List<MACDResult> result, int i)
//        {
//            List<MACDResult> xNeedList;
//            if (i - 2 >= 0)
//            {
//                xNeedList = result.GetRange(i - 2, 3);
//            }
//            else
//            {
//                xNeedList = result.GetRange(0, 1);
//            }
//            if (xNeedList.Count != 3)
//            {
//                result[i].GoldX = false;
//                result[i].BlackX = false;
//            }
//            else
//            {
//                if (xNeedList[1].DIF == xNeedList[1].DEA)
//                {
//                    if (xNeedList[0].DIF < xNeedList[0].DEA && xNeedList[2].DIF > xNeedList[2].DEA)
//                    {
//                        result[i].GoldX = true;
//                    }
//                    else
//                    {
//                        result[i].GoldX = false;
//                    }

//                    if (xNeedList[0].DIF > xNeedList[0].DEA && xNeedList[2].DIF < xNeedList[2].DEA)
//                    {
//                        result[i].BlackX = true;
//                    }
//                    else
//                    {
//                        result[i].BlackX = false;
//                    }
//                }
//                else
//                {
//                    if (xNeedList[1].DIF < xNeedList[1].DEA && xNeedList[2].DIF > xNeedList[2].DEA)
//                    {
//                        result[i].GoldX = true;
//                    }
//                    else
//                    {
//                        result[i].GoldX = false;
//                    }

//                    if (xNeedList[1].DIF > xNeedList[1].DEA && xNeedList[2].DIF < xNeedList[2].DEA)
//                    {
//                        result[i].BlackX = true;
//                    }
//                    else
//                    {
//                        result[i].BlackX = false;
//                    }
//                }
//            }
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
//        ///// <summary>
//        ///// 涨跌幅
//        ///// </summary>
//        //public double? ChangeRatio { get; set; }
//        ///// <summary>
//        ///// 最高价
//        ///// </summary>
//        //public double? HIP { get; set; }
//        ///// <summary>
//        ///// 最低价
//        ///// </summary>
//        //public double? LOP { get; set; }
//    }
//}
