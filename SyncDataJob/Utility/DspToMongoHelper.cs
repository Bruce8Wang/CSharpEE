using GTA.PI.Models;
using GTA.Quantrader.DSP.ObjectDefine;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SyncDataJob.Utility
{
    class DspToMongoHelper
    {
        #region 财务指标

        private static List<ReqFinance> LstRetryReqFinance = new List<ReqFinance>();

        /// <summary>
        /// 取得所有财务指标编码
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllFinaceIndexCode()
        {
            //从json文件取除概念板块以外的其他指标信息
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFileName = Path.Combine(appDataPath, "Data", "JsonData", "IndexTreeDTO.json");
            string jsonData = File.ReadAllText(jsonFileName);
            List<IndexTreeDTO> lstIndexTree = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IndexTreeDTO>>(jsonData);

            List<string> list = lstIndexTree.Where(m => m.TypeCode == ConstDefine.CstCode_FinancialIndex).Select(m => m.Code).Distinct().ToList();
            return list;
        }

        /// <summary>
        /// 同步财务指标信息
        /// </summary>
        public static void SyncFinaceIndex()
        {
            // 记日志
            LogHelper.Info("总运行", "开始：同步财务指标数据");

            DspRequest dspRequest = new DspRequest();
            dspRequest.DealData = SaveFinaceIndex;
            LstRetryReqFinance = new List<ReqFinance>();

            try
            {
                //取得所有a股安全码
                List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();
                //取得所有财务指标编码
                List<string> lstIndexCode = GetAllFinaceIndexCode();

                ////删除已过期的数据
                //DeleteOverdueFinaceIndex(lstSecurityID[0]);

                // 根据代码同步所有财务指标数据
                foreach (var securityID in lstSecurityID)
                {
                    try
                    {
                        FinanceIndexInfo info = MongoDBHelper.AsQueryable<FinanceIndexInfo>()
                                                            .Where(m => m.SecurityID == securityID)
                                                            .OrderByDescending(m => m.EndDate)
                                                            .FirstOrDefault();
                        DateTime lastDate = new DateTime(1990, 1, 1);
                        if(info != null)
                        {
                            lastDate = info.EndDate;
                        }

                        ReqFinance req = DSPHelper.GetReqFinance(securityID, lstIndexCode, lastDate);
                        if (req != null)
                        {
                            dspRequest.AsyncSendData(req, DSPHelper.WaitTime, req);
                        }
                    }
                    catch (Exception ex)
                    {
                        string errMsg = "同步财务指标异常.SecurityID：" + securityID;
                        LogHelper.Error("异常", errMsg, ex);
                    }
                }

                //等待DSP任务完成
                dspRequest.WaitFinished();
                if (LstRetryReqFinance.Count > 0)
                {
                    //重试
                    RetrySyncFinaceIndex(dspRequest, 1);
                    //等待DSP任务完成
                    dspRequest.WaitFinished();
                }

                ////给10秒钟mongodb。
                //Thread.Sleep(10000);
            }
            catch (Exception ex)
            {
                LogHelper.Error("异常", "同步财务指标发生异常.", ex);
            }
            finally
            {
                dspRequest.Dispose();
                LogHelper.Info("总运行", "结束：同步财务指标数据");
            }
        }

        /// <summary>
        /// 重试同步财务指标信息
        /// </summary>
        public static void RetrySyncFinaceIndex(DspRequest dspRequest, int retryTime)
        {
            List<ReqFinance> lstRetryReq = new List<ReqFinance>();
            lstRetryReq.AddRange(LstRetryReqFinance);
            LstRetryReqFinance.Clear();

            if (retryTime > DSPHelper.RetryTime)
            {
                foreach (ReqFinance req in lstRetryReq)
                {
                    string exMsg = string.Format("同步财务指标异常. 重试{0}次仍未取到值. SecurityID:{1}.", DSPHelper.RetryTime, req.securityIDs[0]);
                    LogHelper.Error("异常", exMsg);
                }
                return;
            }

            DSPHelper.ReConnect();

            foreach(ReqFinance req in lstRetryReq)
            {
                string msg = string.Format("SecurityID:{0}.第{1}次重试！", req.securityIDs[0], retryTime);
                LogHelper.Track("财务指标", msg);
                dspRequest.AsyncSendData(req, DSPHelper.WaitTime, req);
            }

            //等待DSP任务完成
            dspRequest.WaitFinished();
            if (LstRetryReqFinance.Count > 0)
            {
                //重试
                RetrySyncFinaceIndex(dspRequest, ++retryTime);
            }
        }

        /// <summary>
        /// 删除过期的财务指标
        /// （就是已经不用显示的季度）
        /// </summary>
        private static void DeleteOverdueFinaceIndex(ulong securityID)
        {
            List<DateTime> lstExistDate = MongoDBHelper.AsQueryable<FinanceIndexInfo>().Where(m => m.SecurityID == securityID).Select(m => m.EndDate).ToList();
            List<DateTime> listDate = TransferHelper.GetFinanceDate();
            List<DateTime> lstOverdueDate = lstExistDate.Except(listDate).ToList();
            foreach(DateTime overdueDate in lstOverdueDate)
            {
                string delFilter = "EndDate:ISODate(\"" + overdueDate.ToString("yyyy-MM-dd") + "T00:00:00.000+0800\")";
                MongoDBHelper.DeleteManyAsync<FinanceIndexInfo>(delFilter);
            }
        }

        /// <summary>
        /// 保存财务指标
        /// </summary>
        /// <returns></returns>
        public static object SaveFinaceIndex(object rep, object reqObj)
        {
            try
            {
                ReqFinance req = reqObj as ReqFinance;
                ulong securityID = Convert.ToUInt64(req.securityIDs[0]);
                if (rep is Exception)
                {
                    string errMsg = string.Format("SecurityID:{0}.{1}", securityID, (rep as Exception).Message);
                    LogHelper.Track("财务指标", errMsg);

                    LstRetryReqFinance.Add(req);
                    return null;
                }

                List<FinanceIndexInfo> lstInfo = DSPHelper.TransferFinance(rep, securityID);
                int count = lstInfo.Count;
                if (count > 0)
                {
                    string delFilter = "{SecurityID:" + securityID + ", EndDate:{$gte:ISODate(\"" + req.dateBegin + "T00:00:00.000+0800\")}}";
                    MongoDBHelper.DeleteMany<FinanceIndexInfo>(delFilter);
                    MongoDBHelper.InsertMany<FinanceIndexInfo>(lstInfo);
                }
                var tttttt = lstInfo.Where(m => m.IndexCode == "BEPS").ToList();
                LogHelper.Track("财务指标", string.Format("SecurityID:{0}.    Count:{1}", securityID, count));
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("SecurityID：{0}.{1}",((ReqFinance)reqObj).securityIDs[0], ex.Message);
                LogHelper.Track("财务指标", errMsg);

                LstRetryReqFinance.Add(reqObj as ReqFinance);
            }
            return null;
        }

        #endregion

        #region 行情数据

        /// <summary>
        /// 同步前5个交易日的日线行情数据
        /// </summary>
        public static void SyncDataByTimeIndexInfo()
        {
            LogHelper.Info("总运行", "开始：同步前5个交易日的日线行情");

            //前5个交易日
            List<DateTime> lstTradeDate = MongodbCacheHelper.GetPreTradeDateDescending(5);
            TimePeriod period = new TimePeriod();
            period.begin = TransferHelper.DateTimeToString(lstTradeDate[4]);
            period.end = TransferHelper.DateTimeToString(lstTradeDate[0]);
            try
            {
                var dataByTime = DSPHelper.GetDataByTimeIndexInfo(period);

                if (dataByTime.Count > 0)
                {
                    MongoDBHelper.DeleteMany<DataByTimeIndexInfo>("{}");
                    MongoDBHelper.InsertMany<DataByTimeIndexInfo>(dataByTime);
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("同步前5交易日行情发生异常.begin:{0}; end:{1}", period.begin, period.end);
                LogHelper.Error("异常", errMsg, ex);
            }
            finally
            {
                LogHelper.Info("总运行", "结束：同步前5个交易日的日线行情");
            }
        }

        private static List<ReqDataByTime> LstRetryReqDataByTime = new List<ReqDataByTime>();
        /// <summary>
        /// 同步所有日线行情数据
        /// </summary>
        public static void SyncDataByTime()
        {
            // 记日志
            LogHelper.Info("总运行", "开始：同步所有日线行情");

            DspRequest dspRequest = new DspRequest();
            dspRequest.DealData = SaveDataByTime;
            LstRetryReqDataByTime = new List<ReqDataByTime>();

            try
            {
                //取得所有a股安全码
                List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

                //根据代码同步所有日频行情数据
                foreach (var securityID in lstSecurityID)
                {
                    try
                    {
                        DataByTime info = MongoDBHelper.AsQueryable<DataByTime>()
                                                        .Where(m => m.SecurityID == securityID)
                                                        .OrderByDescending(m => m.TradingDate)
                                                        .FirstOrDefault();

                        ReqDataByTime req = null;
                        if (info == null)
                        {
                            req = DSPHelper.GetReqDataByTimeAll(securityID);
                        }
                        else
                        {
                            DateTime beginDate = Convert.ToDateTime(info.TradingDate).AddDays(1);
                            if (beginDate >= DateTime.Today)
                            {
                                continue;
                            }
                            req = DSPHelper.GetReqDataByTimeLast(securityID, beginDate);
                        }

                        dspRequest.AsyncSendData(req, DSPHelper.WaitTime, req);
                    }
                    catch (Exception ex)
                    {
                        string errMsg = string.Format("同步所有日线行情发生异常.securityID：{0}", securityID);
                        LogHelper.Error("异常", errMsg, ex);
                    }
                }

                //等待DSP任务完成
                dspRequest.WaitFinished();
                if (LstRetryReqDataByTime.Count > 0)
                {
                    //重试
                    RetrySyncDataByTime(dspRequest, 1);
                    //等待DSP任务完成
                    dspRequest.WaitFinished();
                }

                ////给10秒钟mongodb。
                //Thread.Sleep(10000);
            }
            catch (Exception ex)
            {
                LogHelper.Error("异常", "同步所有日线行情发生异常.", ex);
            }
            finally
            {
                dspRequest.Dispose();
                LogHelper.Info("总运行", "结束：同步所有日线行情");
            }
        }

        /// <summary>
        /// 重试同步行情指标信息
        /// </summary>
        public static void RetrySyncDataByTime(DspRequest dspRequest, int retryTime)
        {
            List<ReqDataByTime> lstRetryReq = new List<ReqDataByTime>();
            lstRetryReq.AddRange(LstRetryReqDataByTime);
            LstRetryReqDataByTime.Clear();

            if (retryTime > DSPHelper.RetryTime)
            {
                foreach (ReqDataByTime req in lstRetryReq)
                {
                    string exMsg = string.Format("同步行情指标异常. 重试{0}次仍未取到值. SecurityID:{1}.", DSPHelper.RetryTime, req.securityIDs[0]);
                    LogHelper.Error("异常", exMsg);
                }
                return;
            }

            DSPHelper.ReConnect();

            foreach (ReqDataByTime req in lstRetryReq)
            {
                string msg = string.Format("SecurityID:{0}.第{1}次重试！", req.securityIDs[0], retryTime);
                LogHelper.Track("所有日线行情", msg);
                dspRequest.AsyncSendData(req, DSPHelper.WaitTime, req);
            }

            //等待DSP任务完成
            dspRequest.WaitFinished();
            if (LstRetryReqDataByTime.Count > 0)
            {
                //重试
                RetrySyncDataByTime(dspRequest, ++retryTime);
            }
        }

        /// <summary>
        /// 保存日线行情数据
        /// </summary>
        /// <returns></returns>
        public static object SaveDataByTime(object rep, object reqObj)
        {
            ReqDataByTime req = reqObj as ReqDataByTime;

            try
            {
                ulong securityID = Convert.ToUInt64(req.securityIDs[0]);
                if (rep is Exception)
                {
                    string msg = string.Format("SecurityID:{0}.{1}", securityID, (rep as Exception).Message);
                    LogHelper.Track("所有日线行情", msg);

                    LstRetryReqDataByTime.Add(req);
                    return null;
                }

                List<DataByTime> lstInfo = DSPHelper.TransferDataByTime(rep, securityID);
                int count = lstInfo.Count;
                if (count > 0)
                {
                    MongoDBHelper.InsertMany<DataByTime>(lstInfo);
                }

                LogHelper.Track("所有日线行情", string.Format("SecurityID:{0}.    Count:{1}", securityID, count));
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("SecurityID：{0},{1}", (reqObj as ReqDataByTime).securityIDs[0], ex.Message);
                LogHelper.Track("所有日线行情", errMsg);

                LstRetryReqDataByTime.Add(req);
            }
            return null;
        }

        /// <summary>
        /// 同步行情快照
        /// </summary>
        public static void SyncQuickData()
        {
            // 记日志
            //LogHelper.Info("最新行情", "开始：同步最新行情");

            // 获取所有a股代码数据
            var symbols = MongoDBHelper.AsQueryable<SymbolInfo>();
            List<L1QuoteInfo> list = DSPHelper.GetQuickData(symbols);

            // 数据获取正常
            if (list.Count > 0)
            {
                SaveL1QuoteInfo(list);
            }

            //LogHelper.Info("最新行情", "结束：同步最新行情");
        }

        /// <summary>
        /// 保存快照信息
        /// </summary>
        /// <param name="list"></param>
        public static void SaveL1QuoteInfo(List<L1QuoteInfo> list)
        {
            foreach (L1QuoteInfo info in list)
            {
                L1QuoteInfo updateInfo = MongoDBHelper.FindOneAndReplace<L1QuoteInfo>(m => m.SecurityID == info.SecurityID, info);
                if (updateInfo == null)
                {
                    MongoDBHelper.InsertOne<L1QuoteInfo>(info);
                }
            }
        }


        #endregion

        #region 基本信息

        /// <summary>
        /// 同步所有a股代码
        /// </summary>
        public static void SyncA_PlateIndex()
        {
            // 记日志
            LogHelper.Info("总运行", "开始：同步a股代码");

            try
            {
                // 获取所有a股代码数据
                var symbols = DSPHelper.GetA_PlateSymbols();
                // 获取数据正常
                if (symbols.Count() > 0)
                {
                    MongoDBHelper.DeleteMany<SymbolInfo>("{}");
                    MongoDBHelper.InsertMany<SymbolInfo>(symbols);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("异常", "同步所有a股代码异常！", ex);
            }
            finally
            {
                LogHelper.Info("总运行", "结束：同步a股代码");
            }
        }

        /// <summary>
        /// 同步交易日历
        /// </summary>
        public static void SyncTradeCalendarIndex()
        {
            // 记日志
            LogHelper.Info("总运行", "开始：同步交易日历");

            try
            {
                var tradingDays = DSPHelper.GetTradingDay();
                if (tradingDays.Count() > 0)
                {
                    MongoDBHelper.DeleteMany<TradeCalendarInfo>("{}");
                    MongoDBHelper.InsertMany<TradeCalendarInfo>(tradingDays);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("异常", "同步交易日历异常！", ex);
            }
            finally
            {
                LogHelper.Info("总运行", "结束：同步交易日历");
            }
        }

        /// <summary>
        /// 同步板块数据
        /// </summary>
        public static void SyncPlatesIndex()
        {
            // 记日志
            LogHelper.Info("总运行", "开始：同步板块信息");

            try
            {
                var plates = DSPHelper.GetPlates();
                if (plates.Count() > 0)
                {
                    MongoDBHelper.DeleteMany<PlateInfo>("{}");
                    MongoDBHelper.InsertMany<PlateInfo>(plates);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("异常", "同步板块数据异常！", ex);
            }
            finally
            {
                LogHelper.Info("总运行", "结束：同步板块信息");
            }
        }

        /// <summary>
        /// 缓存所有板块其下的股票（Todo放MongodbCacheHelper）
        /// </summary>
        public static void CachePlatesSymbol()
        {
            // 记日志
            LogHelper.Info("总运行", "开始：同步板块股票信息");

            List<Int64> lstPlateID = MongoDBHelper.AsQueryable<IndexTreeDTO>()
                                                   .Where(m => m.TypeCode == ConstDefine.CstCode_PlateIndex)
                                                   .Select(m => m.Code)
                                                   .ToList()
                                                   .Select(m => Convert.ToInt64(m)).ToList();

            List<PlateSymbolInfo> plateSymbols = new List<PlateSymbolInfo>();
            foreach (Int64 plateID in lstPlateID)
            {
                try
                {
                    var symbols = DSPHelper.GetPlateSymbols(Convert.ToUInt64(plateID));
                    if (symbols == null)
                    {
                        continue;
                    }

                    var securityIDs = symbols.Select(x => x.SecurityID);
                    plateSymbols.Add(new PlateSymbolInfo
                    {
                        PlateID = plateID,
                        SecurityIDs = securityIDs
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.Error("异常", "板块ID:" + plateID, ex);
                }
            }

            if (plateSymbols.Count() > 0)
            {
                MongoDBHelper.DeleteMany<PlateSymbolInfo>("{}");
                MongoDBHelper.InsertMany<PlateSymbolInfo>(plateSymbols);
            }

            LogHelper.Info("总运行", "结束：同步板块股票信息");
        }

        #endregion
    }
}
