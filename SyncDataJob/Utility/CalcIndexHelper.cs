using GTA.PI.Logics;
using GTA.PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncDataJob.Utility
{
    class CalcIndexHelper
    {
        /// <summary>
        /// 技术指标MACD
        /// </summary>
        public static void CalculateMACD()
        {
            //取得所有a股安全码
            List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

            List<MACDHistory> lstAllLast5 = new List<MACDHistory>();
            foreach (ulong securityID in lstSecurityID)
            {
                var lstRet = (from m in MongoDBHelper.AsQueryable<MACDHistory>()
                             where m.SecurityID == securityID
                             orderby m.TradingDate ascending
                             select m).ToList();

                List<MACDHistory> lstRaw;
                if(lstRet.Count == 0)
                {
                    lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.CP.HasValue
                              orderby m.TradingDate ascending
                              select new MACDHistory()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value
                              }).ToList();
                }
                else
                {
                    MACDHistory last = lstRet.Last();
                    lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.TradingDate > last.TradingDate && m.CP.HasValue 
                              orderby m.TradingDate ascending
                              select new MACDHistory()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value
                              }).ToList();
                }

                int newIndex = lstRet.Count;
                lstRet.AddRange(lstRaw);

                CalcMACD.CalcOneStock(lstRet, newIndex, 12, 26, 9);
                var lstLast5 = lstRet.Skip(lstRet.Count - 5).Take(5).ToList();
                lstAllLast5.AddRange(lstLast5);
            }

            if (lstAllLast5.Count > 0)
            {
                MongoDBHelper.DeleteMany<MACDHistory>("{}");
                MongoDBHelper.InsertMany<MACDHistory>(lstAllLast5);

                var lstResult = CalcMACD.History2Result(lstAllLast5);
                MongoDBHelper.DeleteMany<MACDResult>("{}");
                MongoDBHelper.InsertMany<MACDResult>(lstResult);
            }
        }

        /// <summary>
        /// 技术指标MA
        /// </summary>
        public static void CalculateMA()
        {
            //取得所有a股安全码
            List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

            //默认参数20
            int maParam = 20;
            //只要前一交易日
            int dataCount = maParam;

            List<MAResult> lstAll = new List<MAResult>();
            foreach (ulong securityID in lstSecurityID)
            {
                var lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.CP.HasValue
                              orderby m.TradingDate descending
                              select new MARawData()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value
                              }).Take(dataCount).OrderBy(m => m.TradingDate).ToList();

                List<MAResult> lstResult = CalcMA.CalcOneStock(lstRaw, maParam);
                lstAll.AddRange(lstResult);
            }

            if (lstAll.Count > 0)
            {
                MongoDBHelper.DeleteMany<MAResult>("{}");
                MongoDBHelper.InsertMany<MAResult>(lstAll);
            }
        }

        /// <summary>
        /// 技术指标KDJ
        /// </summary>
        public static void CalculateKDJ()
        {
            //默认参数
            int N = 9;

            //取得所有a股安全码
            List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

            List<KDJHistory> lstAllLastN = new List<KDJHistory>();
            foreach (ulong securityID in lstSecurityID)
            {
                var lstRet = (from m in MongoDBHelper.AsQueryable<KDJHistory>()
                              where m.SecurityID == securityID
                              orderby m.TradingDate ascending
                              select m).ToList();

                List<KDJHistory> lstRaw;
                if (lstRet.Count == 0)
                {
                    lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.CP.HasValue && m.HIP.HasValue && m.LOP.HasValue
                              orderby m.TradingDate ascending
                              select new KDJHistory()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value,
                                  HIP = m.HIP.Value,
                                  LOP = m.LOP.Value
                              }).ToList();
                }
                else
                {
                    KDJHistory last = lstRet.Last();
                    lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.TradingDate > last.TradingDate && m.CP.HasValue
                              orderby m.TradingDate ascending
                              select new KDJHistory()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value,
                                  HIP = m.HIP.Value,
                                  LOP = m.LOP.Value
                              }).ToList();
                }

                int newIndex = lstRet.Count;
                lstRet.AddRange(lstRaw);

                CalcKDJ.CalcOneStock(lstRet, newIndex, N, 3, 3);
                var lstLastN = lstRet.Skip(lstRet.Count - N).Take(N).ToList();
                lstAllLastN.AddRange(lstLastN);
            }

            if (lstAllLastN.Count > 0)
            {
                MongoDBHelper.DeleteMany<KDJHistory>("{}");
                MongoDBHelper.InsertMany<KDJHistory>(lstAllLastN);

                var lstResult = CalcKDJ.History2Result(lstAllLastN);
                MongoDBHelper.DeleteMany<KDJResult>("{}");
                MongoDBHelper.InsertMany<KDJResult>(lstResult);
            }
        }

        /// <summary>
        /// 技术指标BOLL
        /// </summary>
        public static void CalculateBOLL()
        {
            //取得所有a股安全码
            List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

            //默认参数20
            int maParam = 20;
            //只要前一交易日
            int dataCount = maParam + 3;

            List<BOLLResult> lstAll = new List<BOLLResult>();
            foreach (ulong securityID in lstSecurityID)
            {
                var lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.CP.HasValue
                              orderby m.TradingDate descending
                              select new BOLLResult()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value
                              }).Take(dataCount).OrderBy(m => m.TradingDate).ToList();

                CalcBOLL.CalcOneStock(lstRaw, maParam);
                lstAll.AddRange(lstRaw);
            }

            if (lstAll.Count > 0)
            {
                var lstResult = CalcBOLL.GetLastResult(lstAll);
                MongoDBHelper.DeleteMany<BOLLResult>("{}");
                MongoDBHelper.InsertMany<BOLLResult>(lstResult);
            }
        }

        /// <summary>
        /// 技术指标WR
        /// </summary>
        public static void CalculateWR()
        {
            //取得所有a股安全码
            List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

            //默认参数14
            int nParam = 14;
            //只要前一交易日
            int dataCount = nParam;

            List<WRResult> lstAll = new List<WRResult>();
            foreach (ulong securityID in lstSecurityID)
            {
                var lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.CP.HasValue && m.HIP.HasValue && m.LOP.HasValue
                              orderby m.TradingDate descending
                              select new WRResult()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value,
                                  HIP =m.HIP.Value,
                                  LOP = m.LOP.Value
                              }).Take(dataCount).OrderBy(m => m.TradingDate).ToList();

                List<WRResult> lstResult = CalcWR.CalcOneStock(lstRaw, nParam);
                lstAll.AddRange(lstResult);
            }

            if (lstAll.Count > 0)
            {
                var lstResult = CalcWR.GetLastResult(lstAll);
                MongoDBHelper.DeleteMany<WRResult>("{}");
                MongoDBHelper.InsertMany<WRResult>(lstResult);
            }
        }

        /// <summary>
        /// 技术指标DMI
        /// </summary>
        public static void CalculateDMI()
        {
            //默认参数
            int N = 14;

            //取得所有a股安全码
            List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

            List<DMIHistory> lstAllLastN = new List<DMIHistory>();
            foreach (ulong securityID in lstSecurityID)
            {
                var lstRet = (from m in MongoDBHelper.AsQueryable<DMIHistory>()
                              where m.SecurityID == securityID
                              orderby m.TradingDate ascending
                              select m).ToList();

                List<DMIHistory> lstRaw;
                if (lstRet.Count == 0)
                {
                    lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.CP.HasValue && m.HIP.HasValue && m.LOP.HasValue
                              orderby m.TradingDate ascending
                              select new DMIHistory()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value,
                                  HIP = m.HIP.Value,
                                  LOP = m.LOP.Value
                              }).ToList();
                }
                else
                {
                    DMIHistory last = lstRet.Last();
                    lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.TradingDate > last.TradingDate && m.CP.HasValue && m.HIP.HasValue && m.LOP.HasValue
                              orderby m.TradingDate ascending
                              select new DMIHistory()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  CP = m.CP.Value,
                                  HIP = m.HIP.Value,
                                  LOP = m.LOP.Value
                              }).ToList();
                }

                int newIndex = lstRet.Count;
                lstRet.AddRange(lstRaw);

                if (lstRet.Count >= N)
                {
                    CalcDMI.CalcOneStock(lstRet, newIndex, N, 6);
                    var lstLastN = lstRet.Skip(lstRet.Count - N).Take(N);
                    lstAllLastN.AddRange(lstLastN);
                }
            }

            if (lstAllLastN.Count > 0)
            {
                MongoDBHelper.DeleteMany<DMIHistory>("{}");
                MongoDBHelper.InsertMany<DMIHistory>(lstAllLastN);

                var lstResult = CalcDMI.History2Result(lstAllLastN);
                MongoDBHelper.DeleteMany<DMIResult>("{}");
                MongoDBHelper.InsertMany<DMIResult>(lstResult);
            }
        }

        ///// <summary>
        ///// 技术指标RSI
        ///// </summary>
        //public static void CalculateRSI()
        //{
        //    //取得所有a股安全码
        //    List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

        //    //默认参数14
        //    int param = 14;
        //    int dataCount = param;

        //    MongoDBHelper.DeleteMany<RSIHistory>("{}");
        //    MongoDBHelper.DeleteMany<RSIResult>("{}");

        //    foreach (ulong securityID in lstSecurityID)
        //    {
        //        var lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
        //                      where m.SecurityID == securityID && m.ChangeRatio.HasValue
        //                      orderby m.TradingDate descending
        //                      select new RSIHistory()
        //                      {
        //                          SecurityID = m.SecurityID,
        //                          TradingDate = m.TradingDate,
        //                          Symbol = m.Symbol,
        //                          ChangeRatio = m.ChangeRatio.Value
        //                      }).OrderBy(m => m.TradingDate).ToList();

        //        CalcRSI.CalcOneStock(lstRaw, param);

        //        if (lstRaw.Count > 0)
        //        {
        //            MongoDBHelper.InsertMany<RSIHistory>(lstRaw);
        //            var lstResult = CalcRSI.History2Result(lstRaw);
        //            MongoDBHelper.InsertMany<RSIResult>(lstResult);
        //        }
        //    }

        //    //if (lstAll.Count > 0)
        //    //{
        //    //    MongoDBHelper.DeleteMany<RSIHistory>("{}");
        //    //    MongoDBHelper.InsertMany<RSIHistory>(lstAll);

        //    //    //var lstResult = CalcRSI.GetLastResult(lstAll);
        //    //    //MongoDBHelper.DeleteMany<RSIResult>("{}");
        //    //    //MongoDBHelper.InsertMany<RSIResult>(lstResult);
        //    //}
        //}

        /// <summary>
        /// 技术指标RSI
        /// </summary>
        public static void CalculateRSI()
        {
            // 获取所有a股代码数据
            var securityIDs = DSPHelper.GetA_PlateSymbols().Select(x => x.SecurityID);
            //默认参数14
            int param = 14;
            int dataCount = Math.Max(param, 12) + 2;

            List<RSIResult> lstAll = new List<RSIResult>();
            foreach (ulong securityID in securityIDs)
            {
                var lstRaw = (from m in MongoDBHelper.AsQueryable<DataByTime>()
                              where m.SecurityID == securityID && m.ChangeRatio.HasValue
                              orderby m.TradingDate descending
                              select new RSIResult()
                              {
                                  SecurityID = m.SecurityID,
                                  TradingDate = m.TradingDate,
                                  Symbol = m.Symbol,
                                  ChangeRatio = m.ChangeRatio.Value
                              }).Take(dataCount).OrderBy(m => m.TradingDate).ToList();

                CalcRSI.CalcOneStock(lstRaw, param);
                lstAll.AddRange(lstRaw);
            }

            if (lstAll.Count > 0)
            {
                var lstResult = CalcRSI.GetLastResult(lstAll);
                MongoDBHelper.DeleteMany<RSIResult>("{}");
                MongoDBHelper.InsertMany<RSIResult>(lstResult);
            }
        }
    }
}
