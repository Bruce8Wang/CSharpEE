using DspProto;
using GTA.Dsp.Client.Client;
using GTA.Dsp.ProtocolHandler;
using GTA.DSP.Prod.QN.DataDefine;
using GTA.PI.Models;
using GTA.Quantrader.DSP.ObjectDefine;
using Newtonsoft.Json;
using SyncDataJob.ProtoObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace SyncDataJob.Utility
{
    /// <summary>
    /// 访问DSP的工具包
    /// </summary>
    public class DSPHelper
    {
        private static int _retryTime = 10;   //重试次数
        public static int RetryTime
        {
            get { return _retryTime; }
        }

        private static int _waitTime = 30;  //单位为秒
        public static int WaitTime
        {
            get { return _waitTime; }
        }

        /// <summary>
        ///   连接dsp网关
        /// </summary>
        /// <returns></returns>
        public static bool Connect()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings["UpmsUrl"].Trim()
                + "?name=" + ConfigurationManager.AppSettings["UserCode"].Trim()
                + "&password=" + ConfigurationManager.AppSettings["UserPwd"].Trim()
                + "&mac=00-00-00-00-00-00&ip=&applicationmark=03").Result;
            if (response == null) throw new Exception("Upms登陆失败!");
            string JsonData = response.Content.ReadAsStringAsync().Result;
            AuthToken auth = JsonConvert.DeserializeObject<AuthToken>(JsonData);

            GatewayAdapter.Instance.SetToken(auth.result.Token);
            GatewayAdapter.Instance.Init();
            GatewayAdapter.Instance.Start();
            ConnectStatus status = GatewayAdapter.Instance.GetConnectStatus();

            if (status == ConnectStatus.ConnectSuccess)
            {
                return true;
            }
            else
            {
                throw new Exception("无法连接到DSP/MDP服务器.");
            }
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        public static bool ReConnect()
        {
            return Connect();
        }

        #region 财务数据

        /// <summary>
        /// 根据代码取财务数据的请求参数
        /// </summary>
        /// <returns></returns>
        public static ReqFinance GetReqFinance(ulong securityID, List<string> lstIndexCode, DateTime lastDate)
        {
            ReqFinance req = new ReqFinance();
            req.securityIDs.Add(securityID);
            req.fields.Add("Symbol");
            req.fields.Add("EndDate");
            req.fields.AddRange(lstIndexCode);

            //List<DateTime> listDate = TransferHelper.GetFinanceDate();
            //DateTime maxDate = listDate.Max();
            //DateTime startDate = maxDate;
            //foreach(DateTime date in listDate)
            //{
            //    if(date >= lastDate)
            //    {
            //        startDate = date;
            //        break;
            //    }
            //}
            List<DateTime> listDate = TransferHelper.GetFinanceDate();
            DateTime maxDate = listDate.Max();
            if (lastDate >= maxDate)
            {
                return null;
            }

            DateTime startDate = listDate[0];
            foreach (DateTime date in listDate)
            {
                if (date > lastDate)
                {
                    startDate = date;
                    break;
                }
            }
            req.dateBegin = TransferHelper.DateTimeToString(startDate);
            req.dateEnd = TransferHelper.DateTimeToString(maxDate);

            // 财报统计的截止日期
            req.dateType = ERptDateType.ERptDateClose;
            // 合并本期
            req.reportType = EReportType.MergeCur;
            // TTM，12个月滚动累计 或者是 季度累计
            req.trailType = ETrailType.TrailAddup;
            req.page = new DataPage() { begin = 0, end = 0 };

            return req;
        }

        /// <summary>
        /// 根据代码取财务数据
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static List<FinanceIndexInfo> TransferFinance(object rep, object securityID)
        {
            return TransferHelper.Transfer2ListFinanceIndexInfo(rep, Convert.ToUInt64(securityID));
        }

        #endregion

        #region 行情数据

        /// <summary>
        /// 从指定开始日开始，获取行情数据
        /// </summary>
        /// <returns></returns>
        public static List<DataByTimeIndexInfo> GetDataByTimeIndexInfo(TimePeriod period, int retryTime = 0)
        {
            var securityIDs = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID);
            var req = new ReqDataByTime();
            req.securityIDs.AddRange(securityIDs);
            //从json文件取行情指标
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFileName = Path.Combine(appDataPath, "Data", "JsonData", "行情数据的字段.json");
            string jsonData = File.ReadAllText(jsonFileName);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            req.fields.AddRange(dic.Keys);
            req.fields.Add("Symbol");
            req.fields.Add("SecurityID");
            req.fields.Add("TradingDate");
            req.fields.Add("Filling");
            req.quoteType = EQuoteType.Day;
            req.interval = 1;
            req.timePeriods.Add(period);
            req.page = new DataPage() { begin = 0, end = 0 };
            req.priceAdj = EPriceAdjust.AdjNone;
            var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);

            if (rep is Exception)
            {
                if (retryTime > RetryTime)
                {
                    string exMsg = string.Format("同步行情异常. 重试{0}次仍未取到值.begin:{1}; end:{2}.", DSPHelper.RetryTime, period.begin, period.end);
                    LogHelper.Error("异常", exMsg);
                    return null;
                }

                retryTime += 1;
                string errMsg = string.Format("获取行情:begin:{0}; end:{1}异常！重试{2}次！", period.begin, period.end, retryTime);
                LogHelper.Info("总运行", errMsg);
                return GetDataByTimeIndexInfo(period, retryTime);
            }

            var list = TransferHelper.Transfer2ListDataByTimeIndexInfo(rep);
            list = list.Where(m => m.Filling == "0").ToList();
            return list;
        }

        /// <summary>
        /// 获取所有的行情数据的请求条件
        /// </summary>
        /// <returns></returns>
        public static ReqDataByTime GetReqDataByTimeAll(ulong securityID)
        {
            return GetReqDataByTime(securityID, Convert.ToDateTime("1992-01-01"), DateTime.Today);
        }

        /// <summary>
        /// 获取新的行情数据的请求条件
        /// </summary>
        /// <returns></returns>
        public static ReqDataByTime GetReqDataByTimeLast(ulong securityID, DateTime beginDate)
        {
            return GetReqDataByTime(securityID, beginDate, DateTime.Today);
        }

        /// <summary>
        /// 获取行情数据的请求条件
        /// </summary>
        /// <returns></returns>
        public static ReqDataByTime GetReqDataByTime(ulong securityID, DateTime beginDate, DateTime endDate)
        {
            ReqDataByTime req = new ReqDataByTime();

            req.securityIDs.Add(securityID);
            req.fields.Add("*");
            req.quoteType = EQuoteType.Day;
            req.interval = 1;
            req.timePeriods.Add(new TimePeriod()
            {
                begin = TransferHelper.DateTimeToString(beginDate), // 只取在此时间之后的数据
                end = TransferHelper.DateTimeToString(endDate)
            });
            req.page = new DataPage() { begin = 0, end = 0 };
            req.priceAdj = EPriceAdjust.AdjNone;

            return req;
        }

        /// <summary>
        /// 根据代码取行情数据
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static List<DataByTime> TransferDataByTime(object rep, object securityID)
        {
            var list = TransferHelper.Transfer2List<DataByTime>(rep);
            list = list.Where(m => m.Filling == "0").ToList();
            return list;
        }

        /// <summary>
        /// 获取行情的快照数据
        /// </summary>
        /// <returns></returns>
        public static List<L1QuoteInfo> GetQuickData(IEnumerable<SymbolInfo> symbols)
        {
            List<L1QuoteInfo> list = new List<L1QuoteInfo>();

            if (symbols == null)
            {
                return list;
            }

            // 异常信息
            List<ulong> lstErrorSecurityID = new List<ulong>();
            foreach (SymbolInfo symbol in symbols)
            {
                try
                {
                    var securityID = symbol.SecurityID;
                    var msgID = "";
                    if (symbol.Market.Equals("SZSE"))
                    {
                        // 深圳L1分笔
                        msgID = "8208";
                    }
                    else
                    {
                        // 上海L1分笔
                        msgID = "4112";
                    }

                    var bytes = RedisOperation.Instance.GetRedisValue(securityID + "$" + msgID + "$0");
                    if (bytes == null)
                    {
                        lstErrorSecurityID.Add(securityID);
                        continue;
                    }

                    QuoteDataInfo info = new QuoteDataInfo();
                    info.msgId = msgID;
                    info.data = bytes;
                    var request = new RequestDataProtoAnalyser();
                    MessageHeader msgHeader2 = new MessageHeader()
                    {
                        TypeID = request.TypeID,
                        MessageID = uint.Parse(info.msgId),
                        Ver = request.Ver
                    };

                    L1QuoteInfo quoteInfo;
                    object result = ProtoBodyAnalyzer.Instance.UnPack(msgHeader2, info.data);
                    if (symbol.Market.Equals("SZSE"))
                    {
                        SubSZSEL1Quote obj = (SubSZSEL1Quote)result;
                        quoteInfo = new L1QuoteInfo
                        {
                            SecurityID = obj.SecurityID[0],
                            Symbol = obj.Symbol,
                            ShortName = obj.ShortName,
                            LastPrice = obj.LastPrice,
                            ChangeRatio = obj.ChangeRatio,
                            OpenPrice = obj.OpenPrice,
                            PreClosePrice = obj.PreClosePrice,
                            TotalVolume = obj.TotalVolume,
                            TotalAmount = obj.TotalAmount
                        };
                    }
                    else
                    {
                        SubSSEL1Quote obj = (SubSSEL1Quote)result;
                        quoteInfo = new L1QuoteInfo
                        {
                            SecurityID = obj.SecurityID[0],
                            Symbol = obj.Symbol,
                            ShortName = obj.ShortName,
                            LastPrice = obj.LastPrice,
                            ChangeRatio = obj.ChangeRatio,
                            OpenPrice = obj.OpenPrice,
                            PreClosePrice = obj.PreClosePrice,
                            TotalVolume = obj.TotalVolume,
                            TotalAmount = obj.TotalAmount
                        };
                    }

                    list.Add(quoteInfo);
                }
                catch (Exception ex)
                {
                    string errMsg = string.Format("SecurityID：{0}. ", symbol.SecurityID);
                    LogHelper.Error("最新行情", errMsg, ex);
                }
            }

            //if (lstErrorSecurityID.Count > 0)
            //{
            //    string errMsg = string.Format("这些股票无法取到最新行情：{0}. ", string.Join("，", lstErrorSecurityID));
            //    LogHelper.Error("最新行情", errMsg);
            //}

            return list;
        }

        #endregion

        #region 基本信息

        /// <summary>
        /// 获取全部A股的代码列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SymbolInfo> GetA_PlateSymbols()
        {
            // 全部A股板块1001001
            return GetPlateSymbols(1001001);
        }

        /// <summary>
        /// 获取指定板块的代码列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SymbolInfo> GetPlateSymbols(ulong plateID, int retryTime = 0)
        {
            var req = new ReqPlateSymbols();
            req.plateIDs.Add(plateID);
            req.setOper = ESetOper.Union;
            req.page = new DataPage() { begin = 0, end = 0 };
            var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);

            if (rep is Exception)
            {
                if (retryTime > RetryTime)
                {
                    string exMsg = string.Format("同步板块股票异常. 重试{0}次仍未取到值. plateID:{1}.", DSPHelper.RetryTime, plateID);
                    LogHelper.Error("异常", exMsg);
                    return null;
                }

                retryTime += 1;
                string errMsg = string.Format("获取板块:{0}的股票发生异常！重试{1}次！", plateID, retryTime);
                LogHelper.Info("总运行", errMsg);
                return GetPlateSymbols(plateID, retryTime);
            }

            var obj = rep as RepPlateSymbols;
            if (obj.columns.Count == 0)
            {
                //string msg = string.Format("板块：{0}，取不到对应的股票。", plateID);
                //LogHelper.Info("板块取股票", msg);
                return null;
            }

            var list = TransferHelper.Transfer2List<SymbolInfo>(rep);
            return list;
        }

        /// <summary>
        /// 获取交易日历
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TradeCalendarInfo> GetTradingDay(int retryTime = 0)
        {
            var req = new ReqTradeCalendar();
            req.exchangeCode.Add("SZSE");
            req.fields.Add("*");
            req.securityType = "S0101";
            req.dateBegin = TransferHelper.DateTimeToString(DateTime.Today.AddMonths(-1));
            req.dateEnd = TransferHelper.DateTimeToString(DateTime.Today);
            req.page = new DataPage() { begin = 0, end = 0 };
            var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);

            if (rep is Exception)
            {
                if (retryTime > RetryTime)
                {
                    string exMsg = string.Format("同步交易日历异常. 重试{0}次仍未取到值.", DSPHelper.RetryTime);
                    LogHelper.Error("异常", exMsg);
                    return new List<TradeCalendarInfo>();
                }

                retryTime += 1;
                string errMsg = string.Format("获取交易日历发生异常！重试{0}次！", retryTime);
                LogHelper.Info("总运行", errMsg);
                return GetTradingDay(retryTime);
            }

            return TransferHelper.Transfer2List<TradeCalendarInfo>(rep);
        }

        /// <summary>
        /// 获取板块数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GTA.PI.Models.PlateInfo> GetPlates(int retryTime = 0)
        {
            var req = new ReqPlates();
            req.securityType.Add("S0101");
            req.plateType.Add("P4901");
            req.plateType.Add("P4911"); //概念类
            req.plateType.Add("P4910");
            req.plateType.Add("P4906");
            req.page = new DataPage() { begin = 0, end = 0 };
            var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);

            if (rep is Exception)
            {
                if (retryTime > RetryTime)
                {
                    string exMsg = string.Format("同步板块数据异常. 重试{0}次仍未取到值.", DSPHelper.RetryTime);
                    LogHelper.Error("异常", exMsg);
                    return null;
                }

                retryTime += 1;
                string errMsg = string.Format("获取交易日历发生异常！重试{0}次！", retryTime);
                LogHelper.Info("总运行", errMsg);
                return GetPlates(retryTime);
            }

            return TransferHelper.Transfer2List<GTA.PI.Models.PlateInfo>(rep);
        }

        #endregion

        #region Test

        //public static void TestGetQuickData()
        //{
        //    var symbols = MongoDBHelper.AsQueryable<SymbolInfo>();
        //    if (symbols == null)
        //    {
        //        return;
        //    }

        //    // 异常信息
        //    List<string> lstErrorKey = new List<string>();
        //    List<L1QuoteInfo> list = new List<L1QuoteInfo>();
        //    foreach (SymbolInfo symbol in symbols)
        //    {
        //        try
        //        {
        //            var securityID = symbol.SecurityID;
        //            var msgID = "";
        //            if (symbol.Market.Equals("SZSE"))
        //            {
        //                // 深圳L1分笔
        //                msgID = "8208";
        //            }
        //            else
        //            {
        //                // 上海L1分笔
        //                msgID = "4112";
        //            }

        //            var bytes = RedisOperation.Instance.GetRedisValue(securityID + "$" + msgID + "$0");
        //            if (bytes == null)
        //            {
        //                lstErrorKey.Add(securityID + "$" + msgID + "$0");
        //                continue;
        //            }

        //            QuoteDataInfo info = new QuoteDataInfo();
        //            info.msgId = msgID;
        //            info.data = bytes;
        //            var request = new RequestDataProtoAnalyser();
        //            MessageHeader msgHeader2 = new MessageHeader()
        //            {
        //                TypeID = request.TypeID,
        //                MessageID = uint.Parse(info.msgId),
        //                Ver = request.Ver
        //            };

        //            L1QuoteInfo quoteInfo;
        //            object result = ProtoBodyAnalyzer.Instance.UnPack(msgHeader2, info.data);
        //            if (symbol.Market.Equals("SZSE"))
        //            {
        //                SubSZSEL1Quote obj = (SubSZSEL1Quote)result;
        //                quoteInfo = new L1QuoteInfo
        //                {
        //                    SecurityID = obj.SecurityID[0],
        //                    Symbol = obj.Symbol,
        //                    ShortName = obj.ShortName,
        //                    LastPrice = obj.LastPrice,
        //                    ChangeRatio = obj.ChangeRatio,
        //                    OpenPrice = obj.OpenPrice,
        //                    PreClosePrice = obj.PreClosePrice,
        //                    TotalVolume = obj.TotalVolume,
        //                    TotalAmount = obj.TotalAmount
        //                };
        //            }
        //            else
        //            {
        //                SubSSEL1Quote obj = (SubSSEL1Quote)result;
        //                quoteInfo = new L1QuoteInfo
        //                {
        //                    SecurityID = obj.SecurityID[0],
        //                    Symbol = obj.Symbol,
        //                    ShortName = obj.ShortName,
        //                    LastPrice = obj.LastPrice,
        //                    ChangeRatio = obj.ChangeRatio,
        //                    OpenPrice = obj.OpenPrice,
        //                    PreClosePrice = obj.PreClosePrice,
        //                    TotalVolume = obj.TotalVolume,
        //                    TotalAmount = obj.TotalAmount
        //                };
        //            }

        //            list.Add(quoteInfo);
        //        }
        //        catch (Exception ex)
        //        {
        //            string errMsg = string.Format("SecurityID：{0}. ", symbol.SecurityID);
        //            LogHelper.Error("最新行情", errMsg, ex);
        //        }
        //    }

        //    if (lstErrorKey.Count > 0)
        //    {
        //        string errMsg = string.Format("这些股票无法取到最新行情：{0}. ", string.Join("，", lstErrorKey));
        //        LogHelper.Error("最新行情", errMsg);
        //    }
        //}

        //public static void TestFinance()
        //{
        //    DspRequest dspRequest = new DspRequest();
        //    dspRequest.DealData = TransferFinance;
        //    //GatewayAdapter.Instance.OnRequestResponse += Instance_ReceiveData;

        //    List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();
        //    int index = 1;
        //    foreach (ulong securityID in lstSecurityID)
        //    {
        //        try
        //        {
        //            var req = new ReqFinance();
        //            req.securityIDs.Add(securityID);
        //            //req.securityIDs.AddRange(lstSecurityID.Take(100));
        //            req.fields.Add("Symbol");
        //            req.fields.Add("EndDate");

        //            //1个指标
        //            //string indexCode = "UDPPS";
        //            //10个指标
        //            //string indexCode = "UDPPS,CASHR,QUKR,CRPS,LTOTAR,CURR,PB,PCF,MVA,EBIT";
        //            //100个指标
        //            //string indexCode = "UDPPS,CASHR,QUKR,CRPS,LTOTAR,CURR,PB,PCF,MVA,EBIT,OPL,TOTL,FDEPS,NAPS,EPS,RLTBTA,GRNPA,SELEXPR,ROAA,ARTOVA,INVTOVA,RARI,RCAI,RFAI,RINVI,GROPA,GRTPA,NOPPMAR,GOPMAR,OPMAR,CAR,CAR5Y,CPTTAR,CRIR,FCFTE,FCFTF,OCFTS,OPIND,RCETDA,RCFTOI,RCTOI,SURCC,SURCCTTM,BUSTR,EFFTR,RETETAR,ROETCI,CURAR,CURLR,FAR,LLIAR,NCURAR,OELLFALI,SEFAR,SHEQR,TANGAR,WCCAR,WCNAR,WCTAR,CFICR,DCR,RCFTCL,RCTMD,DLTCR,EQDBR,EQMTPL,EQR,LMVSER,REBITL,RLTLOC,ROCB,TIEA,TIEB,TOTLNTAR,SQUKR,WORKCAP,ACCIT,CACCRA,CACCRB,CPARA,CPARB,GRAEPSA,GRAEPSB,GRCFPSOA,GRCFPSOB,GRDEPSA,GRDEPSB,GRFAA,GRFAB,GRNCFFAA,GRNCFFAB,GRNCFIAA,GRNCFIAB,GRNCFOAA,GRNCFOAB,GRNPB,GROPB,GRORA,GRORB,GRROEA";
        //            //250个指标
        //            //string indexCode = "UDPPS,CASHR,QUKR,CRPS,LTOTAR,CURR,PB,PCF,MVA,EBIT,OPL,TOTL,FDEPS,NAPS,EPS,RLTBTA,GRNPA,SELEXPR,ROAA,ARTOVA,INVTOVA,RARI,RCAI,RFAI,RINVI,GROPA,GRTPA,NOPPMAR,GOPMAR,OPMAR,CAR,CAR5Y,CPTTAR,CRIR,FCFTE,FCFTF,OCFTS,OPIND,RCETDA,RCFTOI,RCTOI,SURCC,SURCCTTM,BUSTR,EFFTR,RETETAR,ROETCI,CURAR,CURLR,FAR,LLIAR,NCURAR,OELLFALI,SEFAR,SHEQR,TANGAR,WCCAR,WCNAR,WCTAR,CFICR,DCR,RCFTCL,RCTMD,DLTCR,EQDBR,EQMTPL,EQR,LMVSER,REBITL,RLTLOC,ROCB,TIEA,TIEB,TOTLNTAR,SQUKR,WORKCAP,ACCIT,CACCRA,CACCRB,CPARA,CPARB,GRAEPSA,GRAEPSB,GRCFPSOA,GRCFPSOB,GRDEPSA,GRDEPSB,GRFAA,GRFAB,GRNCFFAA,GRNCFFAB,GRNCFIAA,GRNCFIAB,GRNCFOAA,GRNCFOAB,GRNPB,GROPB,GRORA,GRORB,GRROEA,GRROEB,GRTAA,GRTAB,GRTPB,SUSGR,CDIVC,DIVCOV,DIVPAY,RETEARR,EBITA,EBITATTM,EBITTTM,ERDSP,ERDSPTTM,FEXPR,FEXPRTTM,GOPMARTTM,MEXPR,MEXPRTTM,NOPPMARTTM,NPMCAA,NPMCAB,NPMCAC,NPMCATTM,NPMFAA,NPMFAB,NPMFAC,NPMFATTM,OPMARTTM,PBIAT,REBITOPI,REBITOPITTM,REBITTA,RETAA,RETAB,RETAC,RETATTM,RNPTP,ROAB,ROAC,ROATTM,ROEA,ROEB,ROEC,ROETTM,ROIC,ROLC,RPRC,RPRCTTM,RSC,RSCTTM,RTPEBIT,SELEXPRTTM,APTOVA,APTOVB,APTOVC,APTOVTTM,ARTOVB,ARTOVC,ARTOVDA,ARTOVDB,ARTOVDC,ARTOVDTTM,ARTOVTTM,BUSCA,BUSCB,BUSCC,BUSCTTM,CAPP,CCETOVA,CCETOVB,CCETOVC,CURATOVA,CURATOVB,CURATOVC,CURATOVTTM,FATOVA,FATOVB,FATOVC,FATOVTTM,INVTOVB,INVTOVC,INVTOVDA,INVTOVDB,INVTOVDC,INVTOVDTTM,INVTOVTTM,LATOVA,LATOVB,LATOVC,LATOVTTM,SETOVA,SETOVB,SETOVC,SETOVTTM,TOTATOVA,TOTATOVB,TOTATOVC,TOTATOVTTM,WKCPTOVA,WKCPTOVB,WKCPTOVC,WKCPTOVTTM,NCFPS,NCFPSFFA,NCFPSFFATTM,NCFPSFIA,NCFPSFIATTM,NCFPSFOA,NCFPSFOATTM,FCFFPS,FCFTEPS,OPIPS,OPIPSTTM,PPSBIT,PPSBITTTM,REPS,SRPS,WAEPS,BVRA,BVRB,DIVOCSA,DIVOCSB,MVB,PE,PETTM,PS,PSTTM,RDYOCS,TOBQA,TOBQB,TOBQC,TOBQD,FINL,TOTALASSET,TOTALCURASSETS,TOTALCURLIAB,TOTALLIAB,TOTALLTLIAB,LIABANDOWNEREQUITY,TOTALNCURASSET,TOTALNCURLIAB,TOTALSHINT,ADVREC,ACCTPAYABLE,CASHASSET,CPAITALRESERVES,DIVPAYABLE,LTDEBT,LTPAYABLE";
        //            //456
        //            string indexCode = "UDPPS,CASHR,QUKR,CRPS,LTOTAR,CURR,PB,PCF,MVA,EBIT,OPL,TOTL,FDEPS,NAPS,EPS,RLTBTA,GRNPA,SELEXPR,ROAA,ARTOVA,INVTOVA,RARI,RCAI,RFAI,RINVI,GROPA,GRTPA,NOPPMAR,GOPMAR,OPMAR,CAR,CAR5Y,CPTTAR,CRIR,FCFTE,FCFTF,OCFTS,OPIND,RCETDA,RCFTOI,RCTOI,SURCC,SURCCTTM,BUSTR,EFFTR,RETETAR,ROETCI,CURAR,CURLR,FAR,LLIAR,NCURAR,OELLFALI,SEFAR,SHEQR,TANGAR,WCCAR,WCNAR,WCTAR,CFICR,DCR,RCFTCL,RCTMD,DLTCR,EQDBR,EQMTPL,EQR,LMVSER,REBITL,RLTLOC,ROCB,TIEA,TIEB,TOTLNTAR,SQUKR,WORKCAP,ACCIT,CACCRA,CACCRB,CPARA,CPARB,GRAEPSA,GRAEPSB,GRCFPSOA,GRCFPSOB,GRDEPSA,GRDEPSB,GRFAA,GRFAB,GRNCFFAA,GRNCFFAB,GRNCFIAA,GRNCFIAB,GRNCFOAA,GRNCFOAB,GRNPB,GROPB,GRORA,GRORB,GRROEA,GRROEB,GRTAA,GRTAB,GRTPB,SUSGR,CDIVC,DIVCOV,DIVPAY,RETEARR,EBITA,EBITATTM,EBITTTM,ERDSP,ERDSPTTM,FEXPR,FEXPRTTM,GOPMARTTM,MEXPR,MEXPRTTM,NOPPMARTTM,NPMCAA,NPMCAB,NPMCAC,NPMCATTM,NPMFAA,NPMFAB,NPMFAC,NPMFATTM,OPMARTTM,PBIAT,REBITOPI,REBITOPITTM,REBITTA,RETAA,RETAB,RETAC,RETATTM,RNPTP,ROAB,ROAC,ROATTM,ROEA,ROEB,ROEC,ROETTM,ROIC,ROLC,RPRC,RPRCTTM,RSC,RSCTTM,RTPEBIT,SELEXPRTTM,APTOVA,APTOVB,APTOVC,APTOVTTM,ARTOVB,ARTOVC,ARTOVDA,ARTOVDB,ARTOVDC,ARTOVDTTM,ARTOVTTM,BUSCA,BUSCB,BUSCC,BUSCTTM,CAPP,CCETOVA,CCETOVB,CCETOVC,CURATOVA,CURATOVB,CURATOVC,CURATOVTTM,FATOVA,FATOVB,FATOVC,FATOVTTM,INVTOVB,INVTOVC,INVTOVDA,INVTOVDB,INVTOVDC,INVTOVDTTM,INVTOVTTM,LATOVA,LATOVB,LATOVC,LATOVTTM,SETOVA,SETOVB,SETOVC,SETOVTTM,TOTATOVA,TOTATOVB,TOTATOVC,TOTATOVTTM,WKCPTOVA,WKCPTOVB,WKCPTOVC,WKCPTOVTTM,NCFPS,NCFPSFFA,NCFPSFFATTM,NCFPSFIA,NCFPSFIATTM,NCFPSFOA,NCFPSFOATTM,FCFFPS,FCFTEPS,OPIPS,OPIPSTTM,PPSBIT,PPSBITTTM,REPS,SRPS,WAEPS,BVRA,BVRB,DIVOCSA,DIVOCSB,MVB,PE,PETTM,PS,PSTTM,RDYOCS,TOBQA,TOBQB,TOBQC,TOBQD,FINL,TOTALASSET,TOTALCURASSETS,TOTALCURLIAB,TOTALLIAB,TOTALLTLIAB,LIABANDOWNEREQUITY,TOTALNCURASSET,TOTALNCURLIAB,TOTALSHINT,ADVREC,ACCTPAYABLE,CASHASSET,CPAITALRESERVES,DIVPAYABLE,LTDEBT,LTPAYABLE,LTPREPAIDEXP,MINSHINT,OTHERASSET,OTHERCURLIAB,OTHERCURASSETS,OTHERLIAB,OTHERNCURASSET,OTHERNCURLIAB,OTHERPAYABLE,SPECIALPAYABLE,RETAINPROFIT,FAPUATS,ASUATR,AVFSFINASSET,BORRFROMCENTRALBANK,BONDPAYABLE,CASHBALWCENTRALBANK,CLAIMPAYABLE,STATUTORYDEPOSITS,CONSTMATERIAL,ESTLIAB,DUETOBAOFI,BALWFININS,CBDEPANDDUETOBAOFI,DEFTAXLIAB,DEFINCOMETAXASSET,CUSTOMERDEPOSITS,FINASSETFORDERIV,DEVFINLIAB,DISPOSALOFFIXEDASSET,CBDEPOSIT,DEVEXP,STAFFREMUPAYABLE,COMMISSIONPAYABLE,REFUNDABLEDEPOSITS,FORECURTRANSDIFF,GENERALRISKPROVISION,HTMINVT,INTPAYABLE,POLIRESFORLTHEALTHINSUR,POLIRESFORLIFEINSUR,PLEDGEBORROWING,ACCTREC,PRODBIOASSET,NONCURASSETSDUEONEYR,NCURLIABDUEONEYR,UNDERCONSTPROJECT,DIVREC,FIXEDASSET,GOODWILL,INTANGIBLEASSET,INVTPROPERTY,INVENTORY,INTREC,LTREC,LTDEBTINVT,LTEQUITYINVT,LTINVT,LOANANDADVANCES,NOTESREC,OILGASASSET,OTHERREC,NOTESPAYABLE,NETFUNDSLENDING,PREPAY,INSPREMIUMREC,REINSREC,REINSCONTRESERVESREC,SUBROGATIONREC,STINVT,PLACEMENTFROMBAOFI,NIIDFP,POLIDIVPAYABLE,POLIPLEDGELOAN,NOBELMETAL,SECBROKDEPO,PREMIUMRECINADV,CLAIMRESERVES,CRRFR,LTRFHIRFR,PRRLIRFR,UPRRFR,RECFROMREINSUR,INSURCONTRESERVES,CUSTOMERBROKDEPO,SEPACCOUNTASSET,SEPACCTLIAB,PAIDUPCAPITAL,STBORROWING,BALWCLEARCO,CUSTOMEREXRESERVES,SURPLUSRESERVES,TOEATP,TERMDEPOSITS,TAXPAYABLE,FINASSETFORTRADING,HFTFINLIAB,MEMBERSHIPFEE,TREASURYSTOCK,UNEARNPREMIUMRESERVES,UNCONFIRMINVTLOSS,BEPS,COMPROAOPC,DEPS,GLFVC,INVSTG,MININT,NINTINC,NOPEXP,NOPINC,NPAOPC,OPEXP,OPPRO,OPREV,OTHOINC,OTHOPEXP,SELLEXP,TAFO,TCOMINCO,TOLPRO,TOTOEXP,TOTOR,UNCONFIRMINVTLOSS,BME,CLMEXP,CLMRCVR,COMPROAMS,EXPRCRI,EXPRISAC,FCEXP,FCINC,FOREXG,GADMEXP,IGAJV,IMPLOS,INCTEXP,INSRRR,INSUINC,INTEXP,INTINC,NCLMEXP,NEPRMM,NFCINC,NIAMC,NINCSB,NINCSU,NLDNCA,NPIR,OTHCOMPRO,OTHIANP,OTHOPPRO,PAYSRD,PLCDIV,PRECR,PROIR,PROUEPR,RINSPINC,BBCCE,CLMPAID,CPACFILA,CPDDPI,CPEMP,CPGS,CPINVST,CPIS,CPISMSS,CRAI,CRDRI,CRDTFA,CREPBOR,CRRI,CRSGRS,DPDMSS,EBCCE,EFERCC,EOIC,ICRPLCL,IFCP,IFCR,INCLOAC,INCREREP,IPBOFI,IPDI,IPOFI,NCBCB,NCFFA,NCFIA,NCFOP,NCPASOBU,NCRDFILA,NCRDSOBU,NCRR,NICDDBFI,NIDCBFI,OCPRFA,OCPRIA,OCPROA,OCRRIA,OCRROA,OPRFA,PLCDIVP,PRMMRCV,PROBOR,PROISBD,TAXREF,VTAXPAID";
        //            string[] indexCodes = indexCode.Split(',');
        //            req.fields.AddRange(indexCodes);

        //            req.dateBegin = "2014-12-31";
        //            req.dateEnd = "2017-02-13";

        //            // 财报统计的截止日期
        //            req.dateType = ERptDateType.ERptDateClose;

        //            // 合并本期
        //            req.reportType = EReportType.MergeCur;
        //            // 12个月滚动累计 或者是 季度累计
        //            req.trailType = ETrailType.TrailAddup;
        //            req.page = new DataPage() { begin = 0, end = 0 };

        //            DateTime startTime = DateTime.Now;
        //            //var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);
        //            //int reqNum = GatewayAdapter.Instance.Request(req, _waitTime);
        //            dspRequest.AsyncSendData(req, _waitTime, securityID);
        //            System.Threading.Thread.Sleep(100);

        //            DateTime endTime = DateTime.Now;

        //            TimeSpan ts = endTime - startTime;
        //            Console.WriteLine(index + "用时：" + ts.Milliseconds);
        //            index++;
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //}

        //public static void TestFinance2()
        //{
        //    //List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();

        //    var req = new ReqFinance();
        //    //req.securityIDs.AddRange(lstSecurityID);
        //    req.securityIDs.Add(201004171975);
        //    req.fields.Add("Symbol");
        //    req.fields.Add("EndDate");

        //    //指标
        //    //string indexCode = "BEPS";
        //    //string[] indexCodes = indexCode.Split(',');

        //    string indexCode = "UDPPS,CASHR,QUKR,CRPS,LTOTAR,CURR,PB,PCF,MVA,EBIT,OPL,TOTL,FDEPS,NAPS,EPS,RLTBTA,GRNPA,SELEXPR,ROAA,ARTOVA,INVTOVA,RARI,RCAI,RFAI,RINVI,GROPA,GRTPA,NOPPMAR,GOPMAR,OPMAR,CAR,CAR5Y,CPTTAR,CRIR,FCFTE,FCFTF,OCFTS,OPIND,RCETDA,RCFTOI,RCTOI,SURCC,SURCCTTM,BUSTR,EFFTR,RETETAR,ROETCI,CURAR,CURLR,FAR,LLIAR,NCURAR,OELLFALI,SEFAR,SHEQR,TANGAR,WCCAR,WCNAR,WCTAR,CFICR,DCR,RCFTCL,RCTMD,DLTCR,EQDBR,EQMTPL,EQR,LMVSER,REBITL,RLTLOC,ROCB,TIEA,TIEB,TOTLNTAR,SQUKR,WORKCAP,ACCIT,CACCRA,CACCRB,CPARA,CPARB,GRAEPSA,GRAEPSB,GRCFPSOA,GRCFPSOB,GRDEPSA,GRDEPSB,GRFAA,GRFAB,GRNCFFAA,GRNCFFAB,GRNCFIAA,GRNCFIAB,GRNCFOAA,GRNCFOAB,GRNPB,GROPB,GRORA,GRORB,GRROEA,GRROEB,GRTAA,GRTAB,GRTPB,SUSGR,CDIVC,DIVCOV,DIVPAY,RETEARR,EBITA,EBITATTM,EBITTTM,ERDSP,ERDSPTTM,FEXPR,FEXPRTTM,GOPMARTTM,MEXPR,MEXPRTTM,NOPPMARTTM,NPMCAA,NPMCAB,NPMCAC,NPMCATTM,NPMFAA,NPMFAB,NPMFAC,NPMFATTM,OPMARTTM,PBIAT,REBITOPI,REBITOPITTM,REBITTA,RETAA,RETAB,RETAC,RETATTM,RNPTP,ROAB,ROAC,ROATTM,ROEA,ROEB,ROEC,ROETTM,ROIC,ROLC,RPRC,RPRCTTM,RSC,RSCTTM,RTPEBIT,SELEXPRTTM,APTOVA,APTOVB,APTOVC,APTOVTTM,ARTOVB,ARTOVC,ARTOVDA,ARTOVDB,ARTOVDC,ARTOVDTTM,ARTOVTTM,BUSCA,BUSCB,BUSCC,BUSCTTM,CAPP,CCETOVA,CCETOVB,CCETOVC,CURATOVA,CURATOVB,CURATOVC,CURATOVTTM,FATOVA,FATOVB,FATOVC,FATOVTTM,INVTOVB,INVTOVC,INVTOVDA,INVTOVDB,INVTOVDC,INVTOVDTTM,INVTOVTTM,LATOVA,LATOVB,LATOVC,LATOVTTM,SETOVA,SETOVB,SETOVC,SETOVTTM,TOTATOVA,TOTATOVB,TOTATOVC,TOTATOVTTM,WKCPTOVA,WKCPTOVB,WKCPTOVC,WKCPTOVTTM,NCFPS,NCFPSFFA,NCFPSFFATTM,NCFPSFIA,NCFPSFIATTM,NCFPSFOA,NCFPSFOATTM,FCFFPS,FCFTEPS,OPIPS,OPIPSTTM,PPSBIT,PPSBITTTM,REPS,SRPS,WAEPS,BVRA,BVRB,DIVOCSA,DIVOCSB,MVB,PE,PETTM,PS,PSTTM,RDYOCS,TOBQA,TOBQB,TOBQC,TOBQD,FINL,TOTALASSET,TOTALCURASSETS,TOTALCURLIAB,TOTALLIAB,TOTALLTLIAB,LIABANDOWNEREQUITY,TOTALNCURASSET,TOTALNCURLIAB,TOTALSHINT,ADVREC,ACCTPAYABLE,CASHASSET,CPAITALRESERVES,DIVPAYABLE,LTDEBT,LTPAYABLE,LTPREPAIDEXP,MINSHINT,OTHERASSET,OTHERCURLIAB,OTHERCURASSETS,OTHERLIAB,OTHERNCURASSET,OTHERNCURLIAB,OTHERPAYABLE,SPECIALPAYABLE,RETAINPROFIT,FAPUATS,ASUATR,AVFSFINASSET,BORRFROMCENTRALBANK,BONDPAYABLE,CASHBALWCENTRALBANK,CLAIMPAYABLE,STATUTORYDEPOSITS,CONSTMATERIAL,ESTLIAB,DUETOBAOFI,BALWFININS,CBDEPANDDUETOBAOFI,DEFTAXLIAB,DEFINCOMETAXASSET,CUSTOMERDEPOSITS,FINASSETFORDERIV,DEVFINLIAB,DISPOSALOFFIXEDASSET,CBDEPOSIT,DEVEXP,STAFFREMUPAYABLE,COMMISSIONPAYABLE,REFUNDABLEDEPOSITS,FORECURTRANSDIFF,GENERALRISKPROVISION,HTMINVT,INTPAYABLE,POLIRESFORLTHEALTHINSUR,POLIRESFORLIFEINSUR,PLEDGEBORROWING,ACCTREC,PRODBIOASSET,NONCURASSETSDUEONEYR,NCURLIABDUEONEYR,UNDERCONSTPROJECT,DIVREC,FIXEDASSET,GOODWILL,INTANGIBLEASSET,INVTPROPERTY,INVENTORY,INTREC,LTREC,LTDEBTINVT,LTEQUITYINVT,LTINVT,LOANANDADVANCES,NOTESREC,OILGASASSET,OTHERREC,NOTESPAYABLE,NETFUNDSLENDING,PREPAY,INSPREMIUMREC,REINSREC,REINSCONTRESERVESREC,SUBROGATIONREC,STINVT,PLACEMENTFROMBAOFI,NIIDFP,POLIDIVPAYABLE,POLIPLEDGELOAN,NOBELMETAL,SECBROKDEPO,PREMIUMRECINADV,CLAIMRESERVES,CRRFR,LTRFHIRFR,PRRLIRFR,UPRRFR,RECFROMREINSUR,INSURCONTRESERVES,CUSTOMERBROKDEPO,SEPACCOUNTASSET,SEPACCTLIAB,PAIDUPCAPITAL,STBORROWING,BALWCLEARCO,CUSTOMEREXRESERVES,SURPLUSRESERVES,TOEATP,TERMDEPOSITS,TAXPAYABLE,FINASSETFORTRADING,HFTFINLIAB,MEMBERSHIPFEE,TREASURYSTOCK,UNEARNPREMIUMRESERVES,UNCONFIRMINVTLOSS,BEPS,COMPROAOPC,DEPS,GLFVC,INVSTG,MININT,NINTINC,NOPEXP,NOPINC,NPAOPC,OPEXP,OPPRO,OPREV,OTHOINC,OTHOPEXP,SELLEXP,TAFO,TCOMINCO,TOLPRO,TOTOEXP,TOTOR,UNCONFIRMINVTLOSS,BME,CLMEXP,CLMRCVR,COMPROAMS,EXPRCRI,EXPRISAC,FCEXP,FCINC,FOREXG,GADMEXP,IGAJV,IMPLOS,INCTEXP,INSRRR,INSUINC,INTEXP,INTINC,NCLMEXP,NEPRMM,NFCINC,NIAMC,NINCSB,NINCSU,NLDNCA,NPIR,OTHCOMPRO,OTHIANP,OTHOPPRO,PAYSRD,PLCDIV,PRECR,PROIR,PROUEPR,RINSPINC,BBCCE,CLMPAID,CPACFILA,CPDDPI,CPEMP,CPGS,CPINVST,CPIS,CPISMSS,CRAI,CRDRI,CRDTFA,CREPBOR,CRRI,CRSGRS,DPDMSS,EBCCE,EFERCC,EOIC,ICRPLCL,IFCP,IFCR,INCLOAC,INCREREP,IPBOFI,IPDI,IPOFI,NCBCB,NCFFA,NCFIA,NCFOP,NCPASOBU,NCRDFILA,NCRDSOBU,NCRR,NICDDBFI,NIDCBFI,OCPRFA,OCPRIA,OCPROA,OCRRIA,OCRROA,OPRFA,PLCDIVP,PRMMRCV,PROBOR,PROISBD,TAXREF,VTAXPAID";
        //    string[] indexCodes = indexCode.Split(',');
        //    req.fields.AddRange(indexCodes);

        //    req.dateBegin = "2016-09-30";
        //    req.dateEnd = "2016-09-30";

        //    // 财报统计的截止日期
        //    req.dateType = ERptDateType.ERptDateClose;

        //    // 合并本期
        //    req.reportType = EReportType.MergeCur;
        //    // 12个月滚动累计 或者是 季度累计
        //    req.trailType = ETrailType.TrailAddup;
        //    req.page = new DataPage() { begin = 0, end = 0 };

        //    //DateTime startTime = DateTime.Now;
        //    var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);

        //    //DateTime endTime = DateTime.Now;
        //    //TimeSpan ts = endTime - startTime;
        //    //Console.WriteLine("用时：" + ts.Milliseconds);

        //    //var list = TransferHelper.Transfer2ListFinanceIndexInfo(rep, 1001);
        //}

        //public static void TestFinance2()
        //{
        //    var req = new ReqFinance();
        //    //req.securityIDs.Add(201004171975);
        //    req.securityIDs.Add(201000001303);

        //    req.fields.Add("Symbol");
        //    req.fields.Add("ENDDATE");
        //    //req.fields.Add("UpdateTime");

        //    //指标
        //    string indexCode = "BEPS";
        //    req.fields.Add(indexCode);

        //    req.dateBegin = "2016-10-10";
        //    req.dateEnd = "2016-10-11";

        //    // 财报统计的截止日期
        //    //req.dateType = ERptDateType.ERptDateClose;
        //    req.dateType = ERptDateType.ERptDateIssue;

        //    // 合并本期
        //    req.reportType = EReportType.MergeCur;
        //    // 12个月滚动累计 或者是 季度累计
        //    req.trailType = ETrailType.TrailAddup;
        //    req.page = new DataPage() { begin = 0, end = 0 };
        //    var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);

        //    var list = TransferHelper.Transfer2ListFinanceIndexInfo(rep, 201000001303);
        //}

        //public static void TestPlateSymbols()
        //{
        //    //1011001:3G
        //    ulong plateID = 1011001;
        //    var req = new ReqPlateSymbols();
        //    req.plateIDs.Add(plateID);
        //    req.setOper = ESetOper.Union;
        //    req.page = new DataPage() { begin = 0, end = 0 };
        //    var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);
        //    var obj = rep as RepPlateSymbols;
        //    if (obj.columns.Count == 0) //3G板块，取不到股票
        //    {
        //        string msg = string.Format("板块：{0}，取不到对应的股票。", plateID);
        //        Console.WriteLine(msg);
        //    }
        //}

        //private static void Instance_ReceiveData(object sender, RequestEventArgs e)
        //{

        //}

        //public static void TestNewFinanceIndex()
        //{
        //    List<ulong> lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();
        //    List<string> lstCode = DspToMongoHelper.GetAllFinaceIndexCode();

        //    DspRequest dspRequest = new DspRequest();
        //    dspRequest.DealData = SaveFinaceIndex;
        //    foreach(ulong securityID in lstSecurityID)
        //    {
        //        ReqFinance req = GetReqFinance(securityID, lstCode, new DateTime(1990, 1, 1));
        //        var rep = SyncRequestEx.Instance.SyncSendData(req, _waitTime);
        //        dspRequest.AsyncSendData(req, DSPHelper.WaitTime, req);
        //    }
        //}

        ///// <summary>
        ///// 保存财务指标
        ///// </summary>
        ///// <returns></returns>
        //public static object SaveFinaceIndex(object rep, object reqObj)
        //{
        //    try
        //    {
        //        ReqFinance req = reqObj as ReqFinance;
        //        ulong securityID = Convert.ToUInt64(req.securityIDs[0]);

        //        List<FinanceIndex> lstIndex = TransferHelper.Transfer2ListFinanceIndex(rep, securityID);
        //        if (lstIndex.Count > 0)
        //        {
        //            MongoDBHelper.InsertMany<FinanceIndex>(lstIndex);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}

        #endregion
    }
}