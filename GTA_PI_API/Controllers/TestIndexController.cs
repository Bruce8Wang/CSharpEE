//using GTA.PI.API.Attributes;
//using GTA.PI.API.Utility;
//using GTA.PI.Logics;
//using GTA.PI.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using MongoDB.Driver.Linq;

//namespace GTA.PI.API.Controllers
//{
//    public class TestIndexController : ApiController
//    {
//        // GET: api/TestIndex
//        [HttpGet]
//        [Route(" TestIndex/CalcMACD")]
//        [LoggingFilter]
//        public IEnumerable<string> Calc()
//        {
//            var count = MongoDBHelper.Current.GetCollection<DataByTime>(typeof(DataByTime).Name).Count("{}");
//            var symbolList = MongoDBHelper.AsQueryable<SymbolInfo>().ToList();
//            var o = MongoDBHelper.AsQueryable<DataByTime>();
//            double costWithOut = 0;
//            double cost = 0;
//            double costInsert = 0;
//            var begin = DateTime.Now;
//            foreach (var symbol in symbolList)
//            {
//                var source = from info in o
//                             where info.SecurityID == symbol.SecurityID
//                             orderby info.TradingDate ascending
//                             select new MACDPara()
//                             {
//                                 SecurityID = info.SecurityID,
//                                 TradingDate = info.TradingDate,
//                                 Symbol = info.Symbol,
//                                 CP = info.CP
//                             };
//                var sList = source.ToList();
//                var beginWithOut = DateTime.Now;
//                var result = CalcMACD.CalcAll(sList);
//                costWithOut += (DateTime.Now - beginWithOut).TotalMilliseconds;

//                var beginInsert = DateTime.Now;
//                result.Reverse();
//                MongoDBHelper.InsertManyAsync<MACDResult>(result.Take(30));
//                costInsert += (DateTime.Now - beginInsert).TotalMilliseconds;
//            }

//            cost = (DateTime.Now - begin).TotalMilliseconds;

//            return new string[] { "含取数据：" + cost + "ms", "不含取数据" + costWithOut + "ms", "插入数据库" + costInsert + "ms" };
//        }

//        // GET: api/TestIndex
//        [HttpGet]
//        [Route(" TestIndex/TestCalcMACD")]
//        [LoggingFilter]
//        public IEnumerable<string> Get()
//        {
//            var count = MongoDBHelper.Current.GetCollection<DataByTime>(typeof(DataByTime).Name).Count("{}");
//            var symbolList = MongoDBHelper.AsQueryable<SymbolInfo>().Where(x => x.SecurityID == 201000000133).ToList();
//            var o = MongoDBHelper.AsQueryable<DataByTime>();
//            double costWithOut = 0;
//            double cost = 0;
//            double costInsert = 0;
//            var begin = DateTime.Now;
//            var date = new DateTime(1996, 9, 25);
//            foreach (var symbol in symbolList)
//            {
//                var source = from info in o
//                             where info.SecurityID == symbol.SecurityID && info.TradingDate >= date
//                             && info.CP.HasValue
//                             select new MACDPara()
//                             {
//                                 SecurityID = info.SecurityID,
//                                 TradingDate = info.TradingDate,
//                                 Symbol = info.Symbol,
//                                 CP = info.CP
//                             };
//                var sList = source.ToList();
//                var beginWithOut = DateTime.Now;
//                var result = CalcMACD.CalcAll(sList);
//                costWithOut += (DateTime.Now - beginWithOut).TotalMilliseconds;

//                var beginInsert = DateTime.Now;
//                MongoDBHelper.InsertManyAsync<MACDResult>(result);
//                costInsert += (DateTime.Now - beginWithOut).TotalMilliseconds;
//            }

//            cost = (DateTime.Now - begin).TotalMilliseconds;
            
//            return new string[] { "含取数据：" + cost + "ms", "不含取数据" + costWithOut + "ms", "插入数据库" + costInsert + "ms" };
//        }
//        // GET: api/TestIndex
//        [HttpGet]
//        [Route(" TestIndex/TransferData")]
//        [LoggingFilter]
//        public IEnumerable<string> TransferData()
//        {
//            var symbolList = MongoDBHelper.AsQueryable<SymbolInfo>().ToList();
//            var o = MongoDBHelper.AsQueryable<DataByTime>();
//            double cost = 0;
//            var begin = DateTime.Now;
//            foreach (var symbol in symbolList)
//            {
//                var source = from info in o
//                             where info.SecurityID == symbol.SecurityID
//                             select new MACDPara()
//                             {
//                                 SecurityID = info.SecurityID,
//                                 TradingDate = info.TradingDate,
//                                 Symbol = info.Symbol,
//                                 CP = info.CP,
//                                 //HIP = info.HIP,
//                                 //LOP = info.LOP,
//                                 //ChangeRatio = info.ChangeRatio
//                             };
//                MongoDBHelper.InsertManyAsync<MACDPara>(source.ToList());
//            }
//            cost = (DateTime.Now - begin).TotalMilliseconds;

//            return new string[] { "转换数据的时间：" + cost + "ms" };
//        }
//        // GET: api/TestIndex
//        [HttpGet]
//        [Route(" TestIndex/CalcMACDNew")]
//        [LoggingFilter]
//        public IEnumerable<string> CalcMACDNew()
//        {
//            var symbolList = MongoDBHelper.AsQueryable<SymbolInfo>().Take(300).ToList();
//            var o = MongoDBHelper.AsQueryable<MACDPara>();
//            double costWithOut = 0;
//            double cost = 0;
//            var begin = DateTime.Now;
//            foreach (var symbol in symbolList)
//            {
//                //var source = from info in o
//                //             where info.SecurityID == symbol.SecurityID
//                //             select info;
//                //var result = CalcMACD.CalcMACDMain(source.ToList());
//                var beginWithOut = DateTime.Now;
//                var result = CalcMACD.CalcAll(dic[symbol.SecurityID]);
//                costWithOut += (DateTime.Now - beginWithOut).TotalMilliseconds;


//            }

//            cost = (DateTime.Now - begin).TotalMilliseconds;

//            return new string[] { "含取数据：" + cost + "ms", "不含取数据" + costWithOut + "ms" };
//        }
//        // GET: api/TestIndex
//        [HttpGet]
//        [Route(" TestIndex/TransferDataToDictionary")]
//        [LoggingFilter]
//        public IEnumerable<string> TransferDataToDictionary()
//        {
//            var symbolList = MongoDBHelper.AsQueryable<SymbolInfo>().Take(300).ToList();
//            var o = MongoDBHelper.AsQueryable<DataByTime>();
//            double cost = 0;          

//            //var listID = from index in symbolList
//            //             select index.SecurityID;
//            var begin = DateTime.Now;
//            //var source = from info in o
//            //             where listID.Contains(info.SecurityID)
//            //             select info;
//            //var ss = source.ToList();

//            foreach (var symbol in symbolList)
//            {
//                var source = from info in o
//                             where info.SecurityID == symbol.SecurityID
//                             && info.CP.HasValue
//                             select new MACDPara()
//                             {
//                                 SecurityID = info.SecurityID,
//                                 TradingDate = info.TradingDate,
//                                 Symbol = info.Symbol,
//                                 CP = info.CP
//                             };
//                dic.Add(symbol.SecurityID, source.ToList());
//            }
//            cost = (DateTime.Now - begin).TotalMilliseconds;

//            return new string[] { "转换数据的时间：" + cost + "ms" };
//        }
//        private static Dictionary<ulong, List<MACDPara>> dic = new Dictionary<ulong, List<MACDPara>>();

//        // GET: api/TestIndex
//        [HttpGet]
//        [Route(" TestIndex/TestTransferDataToDictionary")]
//        [LoggingFilter]
//        public IEnumerable<string> TestTransferDataToDictionary()
//        {
//            var symbolList = MongoDBHelper.AsQueryable<SymbolInfo>().ToList();
//            var o = MongoDBHelper.AsQueryable<DataByTime>();
//            double cost = 0;
            
//            var begin = DateTime.Now;
//            foreach (var symbol in symbolList)
//            {
//                var source = from info in o
//                             where info.SecurityID == symbol.SecurityID
//                             select new MACDPara()
//                             {
//                                 SecurityID = info.SecurityID,
//                                 TradingDate = info.TradingDate,
//                                 Symbol = info.Symbol,
//                                 CP = info.CP
//                             };
//                //for(int i = 0; i < source.Count<MAPara>(); i++)
//                //{
//                //    source.GetEnumerator();
//                //}
//                dicTest.Add(symbol.SecurityID, source.ToList());
//                //MongoDBHelper.InsertManyAsync<Dictionary<ulong, List<MAPara>>>(dic);
//            }
//            cost = (DateTime.Now - begin).TotalMilliseconds;

//            return new string[] { "转换数据的时间：" + cost + "ms" };
//        }
//        private Dictionary<ulong, IEnumerable<MACDPara>> dicTest = new Dictionary<ulong, IEnumerable<MACDPara>>();
//    }
//}
