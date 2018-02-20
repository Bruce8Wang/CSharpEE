using GTA.PI.API.Attributes;
using GTA.PI.API.Models;
using GTA.PI.API.Utility;
using GTA.PI.Logics;
using GTA.PI.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Extensions;

namespace GTA.PI.API.Controllers
{
    /// <summary>
    /// 股票指标
    /// </summary>
    public class IndexController : ApiController
    {
        private GTAPIContext db = new GTAPIContext();
        private IMongoDatabase db_ = MongoDBHelper.Current;

        /// <summary>
        /// 用于指标检索智能感知
        /// </summary>
        /// <returns></returns>
        [LoggingFilter]
        [Route("Index/Lite/{Name}")]
        public IQueryable<IndexLiteDTO> GetIndexLite(string Name)
        {
            IQueryable<IndexLiteDTO> data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "IndexLiteDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<IndexLiteDTO>>(jsonData).AsQueryable<IndexLiteDTO>();
            }
            else
            {
                //从mongodb获取
                string jsonFilter = string.Format("{{TypeCode:{{$ne:'{0}'}},Name: {{$regex: '{1}', $options:'i'}}}}", ConstDefine.CstCode_CommonlyUsedIndex, Name);
                string jsonFields = "{'Code':1,'Name':1}";
                data = MongoDBHelper.QueryNeedFields<IndexTreeDTO, IndexLiteDTO>(jsonFilter, jsonFields).AsQueryable();
            }
            return data;
        }

        /// <summary>
        /// 用于最近使用的指标
        /// </summary>
        /// <returns></returns>        
        [Route("Index/Ranking")]
        [LoggingFilter]
        public IQueryable<IndexRankingDTO> GetIndexRanking()
        {
            //从json文件取
            IQueryable<IndexRankingDTO> data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "IndexRankingDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<IndexRankingDTO>>(jsonData).AsQueryable<IndexRankingDTO>();
            }
            else
            {
                //从mongodb取
                data = from l in db_.GetCollection<IndexTreeDTO>(typeof(IndexTreeDTO).Name).AsQueryable<IndexTreeDTO>().Where(m => m.TypeCode == ConstDefine.CstCode_CommonlyUsedIndex)
                       select new IndexRankingDTO()
                       {
                           Code = l.Code,
                           Name = l.Name
                       };
            }
            return data;
        }

        /// <summary>
        /// 用于“指标树”
        /// </summary>
        /// <returns></returns>        
        [Route("Index/Tree")]
        [LoggingFilter]
        public IQueryable<IndexTreeDTO> GetIndexTree()
        {
            IQueryable<IndexTreeDTO> data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "IndexTreeDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<IndexTreeDTO>>(jsonData).AsQueryable<IndexTreeDTO>();
            }
            else
            {
                //从mongodb取 
                data = db_.GetCollection<IndexTreeDTO>(typeof(IndexTreeDTO).Name).AsQueryable<IndexTreeDTO>();
            }
            return data;
        }

        /// <summary>
        /// 用于指标值选择（含最大值、最小值、释义）
        /// </summary>
        /// <returns></returns>        
        [Route("Index/Detail")]
        [LoggingFilter]
        public IQueryable<IndexDetailDTO> GetIndexDetail()
        {
            IQueryable<IndexDetailDTO> data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "IndexDetailDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<IndexDetailDTO>>(jsonData).AsQueryable<IndexDetailDTO>();
            }
            else
            {
                //从mongodb取            
                data = db_.GetCollection<IndexDetailDTO>(typeof(IndexDetailDTO).Name).AsQueryable<IndexDetailDTO>();
            }
            return data;
        }

        /// <summary>
        /// 保存选股方案
        /// </summary>
        /// <returns></returns>
        [Route("Index/SaveSolution")]
        [LoggingFilter]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> SaveMySolution(StockConditionVM solution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChooseSolution ChooseSolution = new ChooseSolution();
            ChooseSolution.Name = solution.Name;
            ChooseSolution.InputCondition = JsonConvert.SerializeObject(solution.InputCondition);
            ChooseSolution.UserId = null;
            ChooseSolution.CreateDate = DateTime.Now;

            db.ChooseSolutions.Add(ChooseSolution);
            await db.SaveChangesAsync();

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 我的选股方案
        /// </summary>
        /// <returns></returns>
        [Route("Index/MySolutions")]
        [LoggingFilter]
        public IQueryable<StockSolutionDTO> GetMySolutions()
        {
            IQueryable<StockSolutionDTO> data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件获取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "MySolutionDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<StockSolutionDTO>>(jsonData).AsQueryable<StockSolutionDTO>();
            }
            else
            {
                //从MySQL取
                data = from s in db.ChooseSolutions
                       //where s.User.UserName == User.Identity.Name
                       select new StockSolutionDTO()
                       {
                           Id = s.Id,
                           Name = s.Name
                       };
            }
            return data;
        }

        /// <summary>
        /// 我的选股条件
        /// </summary>
        /// <returns></returns>
        [Route("Index/MyConditions/{id}")]
        [LoggingFilter]
        public InputConditionVM GetMyConditions(int id)
        {
            InputConditionVM data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "StockConditionDTO_.json");
                string jsonData = File.ReadAllText(jsonFileName);
                IQueryable<StockConditionVM> o = JsonConvert.DeserializeObject<List<StockConditionVM>>(jsonData).AsQueryable<StockConditionVM>();
                data = (from s in o
                        where s.Id == id
                        select s.InputCondition).FirstOrDefault();
            }
            else
            {
                //从MySQL获取
                data = JsonConvert.DeserializeObject<InputConditionVM>((from s in db.ChooseSolutions
                                                                        where s.Id == id
                                                                        //&& s.User.UserName == User.Identity.Name
                                                                        select s
              ).FirstOrDefault().InputCondition);
            }

            ////重新设置分布值
            //if (data != null)
            //{
            //    if (data.IndexRangeConditions != null)
            //    {
            //        Parallel.ForEach(data.IndexRangeConditions, cond =>
            //        {
            //            string typeCode = this.GetTypeCodeByIndexCode(cond.Code);
            //            if (typeCode == ConstDefine.CstCode_MarketIndex)
            //            {
            //                cond.Value = GetIndexDetailValues(cond.Code, cond.SelectTermDisplay);
            //            }
            //        });
            //    }
            //}

            return data;
        }

        /// <summary>
        /// 取指标分布值
        /// </summary>
        /// <param name="indexCode"></param>
        /// <param name="selectTermDisplay"></param>
        /// <returns></returns>
        private List<double> GetIndexDetailValues(string indexCode, string selectTermDisplay)
        {
            var values = MongoDBHelper.AsQueryable<IndexDetailDTO>()
                                .Where(m => m.Code == indexCode && m.SelectTermDisplay == selectTermDisplay)
                                .Select(m => m.Value)
                                .FirstOrDefault().ToList();
            return values;
        }

        /// <summary>
        /// 刷新选股结果
        /// </summary>
        /// <returns></returns>
        [Route("Index/RefreshStockDetail")]
        [HttpPost]
        [LoggingFilter]
        public IQueryable<StockDetailDTO> RefreshStockDetail(string[] lstSymbol)
        {
            IQueryable<StockDetailDTO> data = null;

            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "StockDetailDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<StockDetailDTO>>(jsonData).AsQueryable<StockDetailDTO>();
            }
            else
            {
                var o = MongoDBHelper.AsQueryable<L1QuoteInfo>().Where(m => lstSymbol.Contains(m.Symbol));
                data = from info in o
                       select new StockDetailDTO()
                       {
                           Code = info.Symbol,
                           Name = info.ShortName,
                           RecentRice = info.LastPrice,
                           ChangeRate = info.ChangeRatio,
                           ChangeAmount = info.OpenPrice,
                           LastAmount = info.PreClosePrice,
                           volume = info.TotalVolume,
                           TodayAmount = info.TotalAmount
                       };
            }

            return data;
        }

        #region 选股查询结果

        /// <summary>
        /// 用于选股查询结果
        /// </summary>
        /// <returns></returns>
        [Route("Index/StockDetail")]
        [HttpPost]
        [LoggingFilter]
        public PageResult<StockDetailDTO> GetStockDetail(InputCondition Conditions, ODataQueryOptions<StockDetailDTO> queryOptions)
        {
            IQueryable<StockDetailDTO> data = null;
            if (ConfigHelper.IsUseStaticJson)
            {
                //从json文件取
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "JsonData");
                string jsonFileName = Path.Combine(jsonPath, "StockDetailDTO.json");
                string jsonData = File.ReadAllText(jsonFileName);
                data = JsonConvert.DeserializeObject<List<StockDetailDTO>>(jsonData).AsQueryable<StockDetailDTO>();
            }
            else
            {
                object objLock = new object();
                //查询MongoDB获取满足选股条件的股票代码
                List<ulong> lstSecurityID = null;

                if (Conditions.IndexRangeConditions != null && Conditions.IndexRangeConditions.Count > 0)
                {
                    if (lstSecurityID == null)
                    {
                        lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();
                    }

                    Parallel.ForEach(Conditions.IndexRangeConditions, (cond, state) =>
                    {
                        List<ulong> lstTemp = null;
                        string typeCode = this.GetTypeCodeByIndexCode(cond.Code);
                        switch (typeCode)
                        {
                            case ConstDefine.CstCode_FinancialIndex:
                                lstTemp = this.GetSecurityIDsByFinancialIndex(cond);
                                break;
                            case ConstDefine.CstCode_PlateIndex:
                                lstTemp = this.GetSecurityIDsByPlateIndex(cond.Code);
                                break;
                            case ConstDefine.CstCode_MarketIndex:
                                lstTemp = this.GetSecurityIDsByMarketIndex(cond);
                                break;
                            case ConstDefine.CstCode_TechnicalIndex:
                                lstTemp = this.GetSecurityIDsByTechnicalIndex(cond);
                                break;
                        }

                        if (lstTemp != null)
                        {
                            lock (objLock)
                            {
                                lstSecurityID = lstSecurityID.Intersect(lstTemp).ToList();
                                if (lstSecurityID.Count == 0)
                                {
                                    state.Stop();
                                }
                            }
                        }
                    });
                }

                if ((lstSecurityID == null || lstSecurityID.Count > 0) && Conditions.CustomIndexConditions != null && Conditions.CustomIndexConditions.Count > 0)
                {
                    if (lstSecurityID == null)
                    {
                        lstSecurityID = MongoDBHelper.AsQueryable<SymbolInfo>().Select(m => m.SecurityID).ToList();
                    }

                    Parallel.ForEach(Conditions.CustomIndexConditions, (cond, state) =>
                    {
                        if (this.CheckCustomIndexCondition(cond) == true)
                        {
                            List<ulong> list = this.GetSecurityIDsByCustomIndexCondition(cond);

                            if (list != null)
                            {
                                lock (objLock)
                                {
                                    lstSecurityID = lstSecurityID.Intersect(list).ToList();
                                    if (lstSecurityID.Count == 0)
                                    {
                                        state.Stop();
                                    }
                                }
                            }
                        }
                    });
                }

                if (lstSecurityID != null)
                {
                    var result = from info in MongoDBHelper.AsQueryable<L1QuoteInfo>()
                                 where lstSecurityID.Contains(info.SecurityID)
                                 select new StockDetailDTO
                                 {
                                     Code = info.Symbol,
                                     Name = info.ShortName,
                                     RecentRice = info.LastPrice,
                                     ChangeRate = info.ChangeRatio,
                                     ChangeAmount = info.OpenPrice,
                                     LastAmount = info.PreClosePrice,
                                     volume = info.TotalVolume,
                                     TodayAmount = info.TotalAmount
                                 };

                    int pageSize = queryOptions.Top == null ? 50 : queryOptions.Top.Value;
                    ODataQuerySettings settings = new ODataQuerySettings();
                    settings.PageSize = pageSize;

                    data = queryOptions.ApplyTo(result, settings) as IQueryable<StockDetailDTO>;
                }
            }

            if (data == null)
            {
                return new PageResult<StockDetailDTO>(new List<StockDetailDTO>(), null, 0);
            }
            else
            {
                return new PageResult<StockDetailDTO>(data, Request.ODataProperties().NextLink, Request.ODataProperties().TotalCount);
            }
        }

        #region 自定义选股

        /// <summary>
        /// 自定义选股
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByCustomIndexCondition(CustomIndexCondition cond)
        {
            //CustomItem item1 = this.CreateCustomItem(cond.first, cond.firstInputVal, lstSecurityID);
            //CustomItem item2 = this.CreateCustomItem(cond.second, cond.secondInputVal, lstSecurityID);
            //CustomItem item3 = this.CreateCustomItem(cond.third, cond.thirdInputVal, lstSecurityID);
            CustomItem item1 = null, item2 = null, item3 = null;
            Parallel.Invoke(
                () => { item1 = this.CreateCustomItem(cond.first, cond.firstInputVal); },
                () => { item2 = this.CreateCustomItem(cond.second, cond.secondInputVal); },
                () => { item3 = this.CreateCustomItem(cond.third, cond.thirdInputVal); });

            CustomItem itemOpe = item1.Calculation(item2, cond.operator1);
            List<ulong> list = itemOpe.Compare(item3, cond.operator2);

            return list;
        }

        /// <summary>
        /// 自定义选股描述
        /// </summary>
        private class CustomItem
        {
            /// <summary>
            /// key:SecurityID
            /// value:Index Valu
            /// </summary>
            public Dictionary<ulong, double> Dic;
            public double? Value;

            /// <summary>
            /// 是否为空
            /// </summary>
            /// <returns></returns>
            public bool IsNull()
            {
                return (Dic == null && Value == null);
            }

            /// <summary>
            /// 是否指标
            /// </summary>
            /// <returns></returns>
            public bool IsIndex()
            {
                return Dic != null;
            }

            /// <summary>
            /// 是否数值
            /// </summary>
            /// <returns></returns>
            public bool IsDecimal()
            {
                return Value != null;
            }

            /// <summary>
            /// 指定的股票安全码是否有效
            /// </summary>
            /// <param name="securityID"></param>
            /// <returns></returns>
            public bool IsSecurityIDValid(ulong securityID)
            {
                if (this.IsIndex())
                {
                    return this.Dic.ContainsKey(securityID);
                }

                return true;
            }

            /// <summary>
            /// 取值
            /// </summary>
            /// <param name="securityID"></param>
            /// <returns></returns>
            public double? GetValue(ulong securityID)
            {
                if (this.IsIndex())
                {
                    if (this.Dic.ContainsKey(securityID))
                    {
                        return this.Dic[securityID];
                    }

                    return null;
                }

                if (this.IsDecimal())
                {
                    return this.Value;
                }

                return null;
            }

            /// <summary>
            /// 计算
            /// </summary>
            /// <returns></returns>
            public CustomItem Calculation(CustomItem other, string ope)
            {
                if (this.IsNull()) return other;
                if (other.IsNull()) return this;

                CustomItem result = new CustomItem();
                if (this.IsIndex())
                {
                    result.Dic = new Dictionary<ulong, double>();
                    foreach (ulong securityID in this.Dic.Keys)
                    {
                        if (other.IsSecurityIDValid(securityID) == false)
                        {
                            continue;
                        }

                        double thisValue = this.Dic[securityID];
                        double? otherValue = other.GetValue(securityID);
                        double opeValue = otherValue == null ? thisValue : this.Calculation(thisValue, otherValue.Value, ope);

                        result.Dic[securityID] = opeValue;
                    }
                }
                else if (this.IsDecimal())
                {
                    double thisValue = this.Value.Value;

                    if (other.IsIndex())
                    {
                        result.Dic = new Dictionary<ulong, double>();

                        foreach (ulong securityID in other.Dic.Keys)
                        {
                            double otherValue = other.Dic[securityID];
                            double opeValue = this.Calculation(thisValue, otherValue, ope);

                            result.Dic[securityID] = opeValue;
                        }
                    }
                    else if (other.IsDecimal())
                    {
                        double otherValue = other.Value.Value;
                        double opeValue = this.Calculation(thisValue, otherValue, ope);

                        result.Value = opeValue;
                    }
                }

                return result;
            }

            /// <summary>
            /// 计算
            /// </summary>
            /// <returns></returns>
            public double Calculation(double value1, double value2, string ope)
            {
                //// 操作符定义
                //$scope.operation1=[
                //    {id:'1',op:'+'},
                //    {id:'2',op:'-'},
                //    {id:'3',op:'×'},
                //    {id:'4',op:'÷'}
                //];

                double opeValue = 0;
                switch (ope)
                {
                    case "1":
                        opeValue = value1 + value2;
                        break;
                    case "2":
                        opeValue = value1 - value2;
                        break;
                    case "3":
                        opeValue = value1 * value2;
                        break;
                    case "4":
                        opeValue = value1 / value2;
                        break;
                }

                return opeValue;
            }

            /// <summary>
            /// 比较
            /// </summary>
            /// <returns></returns>
            public List<ulong> Compare(CustomItem other, string ope)
            {
                List<ulong> list = new List<ulong>();
                if (this.IsNull() && other.IsNull()) return list;

                if (this.IsIndex())
                {
                    foreach (ulong securityID in this.Dic.Keys)
                    {
                        double thisValue = this.Dic[securityID];
                        double? otherValue = other.GetValue(securityID);
                        if (this.Compare(thisValue, otherValue, ope))
                        {
                            list.Add(securityID);
                        }
                    }
                }
                else if (this.IsDecimal())
                {
                    double thisValue = this.Value.Value;

                    if (other.IsIndex())
                    {
                        foreach (ulong securityID in other.Dic.Keys)
                        {
                            double otherValue = other.Dic[securityID];
                            if (this.Compare(thisValue, otherValue, ope))
                            {
                                list.Add(securityID);
                            }
                        }
                    }
                    //else if (other.IsDecimal())
                    //{
                    //    double otherValue = other.value.Value;
                    //    if (this.Compare(thisValue, otherValue, ope))
                    //    {
                    //        list.Add(???);
                    //    }
                    //}
                }

                return list.Distinct().ToList();
            }

            /// <summary>
            /// 计算
            /// </summary>
            /// <returns></returns>
            public bool Compare(double? value1, double? value2, string ope)
            {
                //// 操作符定义
                //$scope.operation2=[
                //    {id:'1',op:'≥'},
                //    {id:'2',op:'＞'},
                //    {id:'3',op:'≤'},
                //    {id:'4',op:'＜'},
                //    {id:'5',op:'='}
                //];

                switch (ope)
                {
                    case "1":
                        return value1 >= value2;
                    case "2":
                        return value1 > value2;
                    case "3":
                        return value1 <= value2;
                    case "4":
                        return value1 < value2;
                    case "5":
                        return value1 == value2;
                }

                return false;
            }
        }

        /// <summary>
        /// 生成选股条件对象
        /// </summary>
        /// <param name="custom"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private CustomItem CreateCustomItem(CustomIndex custom, double? value)
        {
            CustomItem item = new CustomItem();
            if (IsNullIndex(custom) == false)
            {
                Dictionary<ulong, double> dic = null;
                string typeCode = this.GetTypeCodeByIndexCode(custom.Code);
                switch (typeCode)
                {
                    case ConstDefine.CstCode_FinancialIndex:
                        dic = this.GetStocksByFinancialIndex(custom.Code, custom.SelectTermDisplay);
                        break;
                    case ConstDefine.CstCode_MarketIndex:
                        dic = this.GetStocksByMarketIndex(custom.Code, custom.SelectTermDisplay);
                        break;
                    case ConstDefine.CstCode_TechnicalIndex:
                        dic = this.GetStocksByTechnicalIndex(custom);
                        break;
                }

                item.Dic = dic;
            }
            else if (value != null)
            {
                item.Value = value.Value;
            }

            return item;
        }

        /// <summary>
        /// 判断指标是否为空
        /// </summary>
        /// <param name="custom"></param>
        /// <returns></returns>
        private bool IsNullIndex(CustomIndex custom)
        {
            if (custom == null || string.IsNullOrWhiteSpace(custom.Code))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断指标是否为无值指标
        /// </summary>
        /// <param name="custom"></param>
        /// <returns></returns>
        private bool IsNoValueIndex(CustomIndex custom)
        {
            switch (custom.Code)
            {
                case ConstDefine.Cst_MACD_TopDepart:
                case ConstDefine.Cst_MACD_DownDepart:
                case ConstDefine.Cst_MACD_GoldX:
                case ConstDefine.Cst_MACD_BlackX:

                case ConstDefine.Cst_KDJ_GoldX:
                case ConstDefine.Cst_KDJ_BlackX:
                case ConstDefine.Cst_KDJ_LowGoldX:
                case ConstDefine.Cst_KDJ_TopDepart:
                case ConstDefine.Cst_KDJ_DownDepart:
                case ConstDefine.Cst_KDJ_Oversold:
                case ConstDefine.Cst_KDJ_Overbought:

                case ConstDefine.Cst_BOLL_Upper:
                case ConstDefine.Cst_BOLL_Mid:

                case ConstDefine.Cst_DMI_GoldX:
                case ConstDefine.Cst_DMI_BlackX:

                case ConstDefine.Cst_RSI_GoldX:
                case ConstDefine.Cst_RSI_BlackX:
                    return true;
            }

            string typeCode = this.GetTypeCodeByIndexCode(custom.Code);
            if (typeCode == ConstDefine.CstCode_PlateIndex)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断自定义选股计算式是否合法
        /// </summary>
        /// <returns></returns>
        private bool CheckCustomIndexCondition(CustomIndexCondition cond)
        {
            //判断处理
            if (IsNullIndex(cond.third) && cond.thirdInputVal == null)
            {
                return false;
            }

            if (IsNullIndex(cond.first) && cond.firstInputVal == null && IsNullIndex(cond.second) && cond.secondInputVal == null)
            {
                return false;
            }

            if (IsNullIndex(cond.first) && IsNullIndex(cond.second) && IsNullIndex(cond.third))
            {
                return false;
            }

            if (IsNullIndex(cond.first) == false)
            {
                if (IsNoValueIndex(cond.first))
                {
                    return false;
                }
            }

            if (IsNullIndex(cond.second) == false)
            {
                if (IsNoValueIndex(cond.second))
                {
                    return false;
                }
            }

            if (IsNullIndex(cond.third) == false)
            {
                if (IsNoValueIndex(cond.third))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 根据财务指标取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByFinancialIndex(string indexCode, string endDateName)
        {
            var dic = (from info in MongoDBHelper.AsQueryable<FinanceIndexInfo>()
                       where info.IndexCode == indexCode && endDateName == info.EndDateName
                       select new
                       {
                           SecurityID = info.SecurityID,
                           IndexValue = info.IndexValue
                       }).Distinct().ToDictionary(k => k.SecurityID, v => v.IndexValue);
            return dic;
        }

        /// <summary>
        /// 根据行情指标取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByMarketIndex(string indexCode, string tradingDateName)
        {
            var dic = (from info in MongoDBHelper.AsQueryable<DataByTimeIndexInfo>()
                       where info.IndexCode == indexCode && info.TradingDateName == tradingDateName
                       select new
                       {
                           SecurityID = info.SecurityID,
                           IndexValue = info.IndexValue
                       }).Distinct().ToDictionary(k => k.SecurityID, v => v.IndexValue);
            return dic;
        }

        #region 技术指标

        /// <summary>
        /// 根据技术指标取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByTechnicalIndex(CustomIndex cond)
        {
            if (IsDefaultParam(cond.Code, cond.ParamsValues))
            {
                switch (cond.Code)
                {
                    case ConstDefine.Cst_MA:
                        return GetStocksByMA_Defalut();
                    case ConstDefine.Cst_MACD:
                        return GetStocksByMACD_Defalut(cond);
                    case ConstDefine.Cst_KDJ:
                        return GetStocksByKDJ_Defalut(cond);
                    case ConstDefine.Cst_BOLL:
                        return GetStocksByBOLL_Defalut(cond);
                    case ConstDefine.Cst_WR:
                        return GetStocksByWR_Defalut();
                    case ConstDefine.Cst_DMI:
                        return GetStocksByDMI_Defalut(cond);
                    case ConstDefine.Cst_RSI:
                        return GetStocksByRSI_Defalut();
                }
            }

            return new Dictionary<ulong, double>();
        }

        /// <summary>
        /// 根据技术指标MA取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByMA_Defalut()
        {
            var dic = MongoDBHelper.AsQueryable<MAResult>().Distinct().ToDictionary(k => k.SecurityID, v => v.MA);
            return dic;
        }

        /// <summary>
        /// 根据技术指标MACD取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByMACD_Defalut(CustomIndex cond)
        {
            var o = MongoDBHelper.AsQueryable<MACDResult>();

            Dictionary<ulong, double> dic = null;
            switch (cond.SelectTermDisplay)
            {
                case ConstDefine.Cst_MACD_MACD:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.MACD); ;
                    break;
                case ConstDefine.Cst_MACD_DIF:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.DIF); ;
                    break;
                case ConstDefine.Cst_MACD_DEA:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.DEA); ;
                    break;
                default:
                    dic = new Dictionary<ulong, double>();
                    break;
            }
            return dic;
        }

        /// <summary>
        /// 根据技术指标KDJ取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByKDJ_Defalut(CustomIndex cond)
        {
            var o = MongoDBHelper.AsQueryable<KDJResult>();

            Dictionary<ulong, double> dic = null;
            switch (cond.SelectTermDisplay)
            {
                case ConstDefine.Cst_KDJ_K:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.K); ;
                    break;
                case ConstDefine.Cst_KDJ_D:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.D); ;
                    break;
                case ConstDefine.Cst_KDJ_J:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.J); ;
                    break;
                default:
                    dic = new Dictionary<ulong, double>();
                    break;
            }
            return dic;
        }

        /// <summary>
        /// 根据技术指标BOLL取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByBOLL_Defalut(CustomIndex cond)
        {
            var o = MongoDBHelper.AsQueryable<BOLLResult>();

            Dictionary<ulong, double> dic = null;
            switch (cond.SelectTermDisplay)
            {
                case ConstDefine.Cst_BOLL_DN:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.DN); ;
                    break;
                case ConstDefine.Cst_BOLL_MB:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.MB); ;
                    break;
                case ConstDefine.Cst_BOLL_UP:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.UP); ;
                    break;
                default:
                    dic = new Dictionary<ulong, double>();
                    break;
            }
            return dic;
        }

        /// <summary>
        /// 根据技术指标WR取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByWR_Defalut()
        {
            var dic = MongoDBHelper.AsQueryable<WRResult>().Distinct().ToDictionary(k => k.SecurityID, v => v.WR);
            return dic;
        }

        /// <summary>
        /// 根据技术指标DMI取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByDMI_Defalut(CustomIndex cond)
        {
            var o = MongoDBHelper.AsQueryable<DMIResult>();

            Dictionary<ulong, double> dic = null;
            switch (cond.SelectTermDisplay)
            {
                case ConstDefine.Cst_DMI_PDI:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.PDI); ;
                    break;
                case ConstDefine.Cst_DMI_MDI:
                    dic = o.Distinct().ToDictionary(k => k.SecurityID, v => v.MDI); ;
                    break;
                case ConstDefine.Cst_DMI_ADX:
                    dic = o.Where(m => m.ADX.HasValue).Distinct().ToDictionary(k => k.SecurityID, v => v.ADX.Value); ;
                    break;
                default:
                    dic = new Dictionary<ulong, double>();
                    break;
            }
            return dic;
        }

        /// <summary>
        /// 根据技术指标RSI取股票
        /// </summary>
        /// <returns></returns>
        private Dictionary<ulong, double> GetStocksByRSI_Defalut()
        {
            var dic = MongoDBHelper.AsQueryable<RSIResult>().Distinct().ToDictionary(k => k.SecurityID, v => v.RSI);
            return dic;
        }

        #endregion

        #endregion

        #region 范围选股

        /// <summary>
        /// 根据财务指标取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByFinancialIndex(IndexRangeCondition cond)
        {
            var list = from info in MongoDBHelper.AsQueryable<FinanceIndexInfo>()
                       where info.IndexCode == cond.Code
                       && cond.SelectTermDisplay == info.EndDateName
                       && info.IndexValue >= cond.MinValue
                       && info.IndexValue <= cond.MaxValue
                       select info.SecurityID;
            return list.ToList();
        }

        /// <summary>
        /// 根据板块指标取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByPlateIndex(string indexCode)
        {
            long plateID;
            if (long.TryParse(indexCode, out plateID))
            {
                var list = from info in MongoDBHelper.AsQueryable<PlateSymbolInfo>()
                           where info.PlateID == plateID
                           select info.SecurityIDs;
                if (list.Count() > 0)
                {
                    return list.FirstOrDefault().ToList();
                }
            }

            return new List<ulong>();
        }

        /// <summary>
        /// 根据行情指标取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByMarketIndex(IndexRangeCondition cond)
        {
            var list = from info in MongoDBHelper.AsQueryable<DataByTimeIndexInfo>()
                       where info.IndexCode == cond.Code
                       && info.TradingDateName == cond.SelectTermDisplay
                       && info.IndexValue >= cond.MinValue
                       && info.IndexValue <= cond.MaxValue
                       select info.SecurityID;
            return list.ToList();
        }

        #region 技术指标

        /// <summary>
        /// 根据技术指标取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByTechnicalIndex(IndexRangeCondition cond)
        {
            if (IsDefaultParam(cond.Code, cond.ParamsValues))
            {
                switch (cond.Code)
                {
                    case ConstDefine.Cst_MA:
                        return GetSecurityIDsByMA_Default(cond);
                    case ConstDefine.Cst_MACD:
                    case ConstDefine.Cst_MACD_TopDepart:
                    case ConstDefine.Cst_MACD_DownDepart:
                    case ConstDefine.Cst_MACD_GoldX:
                    case ConstDefine.Cst_MACD_BlackX:
                        return GetSecurityIDsByMACD_Default(cond);
                    case ConstDefine.Cst_KDJ:
                    case ConstDefine.Cst_KDJ_GoldX:
                    case ConstDefine.Cst_KDJ_BlackX:
                    case ConstDefine.Cst_KDJ_LowGoldX:
                    case ConstDefine.Cst_KDJ_TopDepart:
                    case ConstDefine.Cst_KDJ_DownDepart:
                    case ConstDefine.Cst_KDJ_Oversold:
                    case ConstDefine.Cst_KDJ_Overbought:
                        return GetSecurityIDsByKDJ_Default(cond);
                    case ConstDefine.Cst_BOLL:
                    case ConstDefine.Cst_BOLL_Upper:
                    case ConstDefine.Cst_BOLL_Mid:
                        return GetSecurityIDsByBOLL_Default(cond);
                    case ConstDefine.Cst_WR:
                        return GetSecurityIDsByWR_Default(cond);
                    case ConstDefine.Cst_DMI:
                    case ConstDefine.Cst_DMI_GoldX:
                    case ConstDefine.Cst_DMI_BlackX:
                        return GetSecurityIDsByDMI_Default(cond);
                    case ConstDefine.Cst_RSI:
                    case ConstDefine.Cst_RSI_GoldX:
                    case ConstDefine.Cst_RSI_BlackX:
                        return GetSecurityIDsByRSI_Default(cond);
                }
            }

            return new List<ulong>();
        }

        /// <summary>
        /// 根据技术指标MA取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByMA_Default(IndexRangeCondition cond)
        {
            var list = from info in MongoDBHelper.AsQueryable<MAResult>()
                       where info.MA >= cond.MinValue && info.MA <= cond.MaxValue
                       select info.SecurityID;
            return list.ToList();
        }

        /// <summary>
        /// 根据技术指标MACD取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByMACD_Default(IndexRangeCondition cond)
        {
            var o = MongoDBHelper.AsQueryable<MACDResult>();

            List<ulong> list = null;
            switch (cond.Code)
            {
                case ConstDefine.Cst_MACD:
                    switch (cond.SelectTermDisplay)
                    {
                        case ConstDefine.Cst_MACD_MACD:
                            list = (from info in o
                                    where info.MACD >= cond.MinValue && info.MACD <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_MACD_DIF:
                            list = (from info in o
                                    where info.DIF >= cond.MinValue && info.DIF <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_MACD_DEA:
                            list = (from info in o
                                    where info.DEA >= cond.MinValue && info.DEA <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        default:
                            list = new List<ulong>();
                            break;
                    }
                    break;
                case ConstDefine.Cst_MACD_TopDepart:
                    list = (from info in o
                            where info.TopDepart == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_MACD_DownDepart:
                    list = (from info in o
                            where info.DownDepart == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_MACD_GoldX:
                    list = (from info in o
                            where info.GoldX == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_MACD_BlackX:
                    list = (from info in o
                            where info.BlackX == true
                            select info.SecurityID).ToList();
                    break;
            }

            return list;
        }

        /// <summary>
        /// 根据技术指标KDJ取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByKDJ_Default(IndexRangeCondition cond)
        {
            var o = MongoDBHelper.AsQueryable<KDJResult>();

            List<ulong> list = null;
            switch (cond.Code)
            {
                case ConstDefine.Cst_KDJ:
                    switch (cond.SelectTermDisplay)
                    {
                        case ConstDefine.Cst_KDJ_K:
                            list = (from info in o
                                    where info.K >= cond.MinValue && info.K <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_KDJ_D:
                            list = (from info in o
                                    where info.D >= cond.MinValue && info.D <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_KDJ_J:
                            list = (from info in o
                                    where info.J >= cond.MinValue && info.J <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        default:
                            list = new List<ulong>();
                            break;
                    }
                    break;
                case ConstDefine.Cst_KDJ_GoldX:
                    list = (from info in o
                            where info.GoldX == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_KDJ_BlackX:
                    list = (from info in o
                            where info.BlackX == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_KDJ_LowGoldX:
                    list = (from info in o
                            where info.LowGoldX == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_KDJ_TopDepart:
                    list = (from info in o
                            where info.TopDepart == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_KDJ_DownDepart:
                    list = (from info in o
                            where info.DownDepart == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_KDJ_Oversold:
                    list = (from info in o
                            where info.Oversold == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_KDJ_Overbought:
                    list = (from info in o
                            where info.Overbought == true
                            select info.SecurityID).ToList();
                    break;
            }

            return list;
        }

        /// <summary>
        /// 根据技术指标BOLL取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByBOLL_Default(IndexRangeCondition cond)
        {
            var o = MongoDBHelper.AsQueryable<BOLLResult>();

            List<ulong> list = null;
            switch (cond.Code)
            {
                case ConstDefine.Cst_BOLL:
                    switch (cond.SelectTermDisplay)
                    {
                        case ConstDefine.Cst_BOLL_DN:
                            list = (from info in o
                                    where info.DN >= cond.MinValue && info.DN <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_BOLL_MB:
                            list = (from info in o
                                    where info.MB >= cond.MinValue && info.MB <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_BOLL_UP:
                            list = (from info in o
                                    where info.UP >= cond.MinValue && info.UP <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        default:
                            list = new List<ulong>();
                            break;
                    }
                    break;
                case ConstDefine.Cst_BOLL_Upper:
                    list = (from info in o
                            where info.Upper == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_BOLL_Mid:
                    list = (from info in o
                            where info.Mid == true
                            select info.SecurityID).ToList();
                    break;
            }

            return list;
        }

        /// <summary>
        /// 根据技术指标WR取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByWR_Default(IndexRangeCondition cond)
        {
            var list = from info in MongoDBHelper.AsQueryable<WRResult>()
                       where info.WR >= cond.MinValue && info.WR <= cond.MaxValue
                       select info.SecurityID;
            return list.ToList();
        }

        /// <summary>
        /// 根据技术指标DMI取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByDMI_Default(IndexRangeCondition cond)
        {
            var o = MongoDBHelper.AsQueryable<DMIResult>();

            List<ulong> list = null;
            switch (cond.Code)
            {
                case ConstDefine.Cst_DMI:
                    switch (cond.SelectTermDisplay)
                    {
                        case ConstDefine.Cst_DMI_PDI:
                            list = (from info in o
                                    where info.PDI >= cond.MinValue && info.PDI <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_DMI_MDI:
                            list = (from info in o
                                    where info.MDI >= cond.MinValue && info.MDI <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        case ConstDefine.Cst_DMI_ADX:
                            list = (from info in o
                                    where info.ADX.HasValue && info.ADX >= cond.MinValue && info.ADX <= cond.MaxValue
                                    select info.SecurityID).ToList();
                            break;
                        default:
                            list = new List<ulong>();
                            break;
                    }
                    break;
                case ConstDefine.Cst_DMI_GoldX:
                    list = (from info in o
                            where info.GoldX == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_DMI_BlackX:
                    list = (from info in o
                            where info.BlackX == true
                            select info.SecurityID).ToList();
                    break;
            }

            return list;
        }

        /// <summary>
        /// 根据技术指标RSI取股票SecurityID
        /// </summary>
        /// <returns></returns>
        private List<ulong> GetSecurityIDsByRSI_Default(IndexRangeCondition cond)
        {
            var o = MongoDBHelper.AsQueryable<RSIResult>();

            List<ulong> list = null;
            switch (cond.Code)
            {
                case ConstDefine.Cst_RSI:
                    list = (from info in o
                            where info.RSI >= cond.MinValue && info.RSI <= cond.MaxValue
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_RSI_GoldX:
                    list = (from info in o
                            where info.RSIGoldX == true
                            select info.SecurityID).ToList();
                    break;
                case ConstDefine.Cst_RSI_BlackX:
                    list = (from info in o
                            where info.RSIBlackX == true
                            select info.SecurityID).ToList();
                    break;
            }

            return list;
        }

        #endregion

        #endregion

        /// <summary>
        /// 判断是否是默认参数
        /// </summary>
        /// <returns></returns>
        private bool IsDefaultParam(string indexCode, List<InputParams> paramsValues)
        {
            switch (indexCode)
            {
                case ConstDefine.Cst_MACD_TopDepart:
                case ConstDefine.Cst_MACD_DownDepart:
                case ConstDefine.Cst_MACD_GoldX:
                case ConstDefine.Cst_MACD_BlackX:

                case ConstDefine.Cst_KDJ_GoldX:
                case ConstDefine.Cst_KDJ_BlackX:
                case ConstDefine.Cst_KDJ_LowGoldX:
                case ConstDefine.Cst_KDJ_TopDepart:
                case ConstDefine.Cst_KDJ_DownDepart:
                case ConstDefine.Cst_KDJ_Oversold:
                case ConstDefine.Cst_KDJ_Overbought:

                case ConstDefine.Cst_BOLL_Upper:
                case ConstDefine.Cst_BOLL_Mid:

                case ConstDefine.Cst_DMI_GoldX:
                case ConstDefine.Cst_DMI_BlackX:

                case ConstDefine.Cst_RSI_GoldX:
                case ConstDefine.Cst_RSI_BlackX:
                    return true;
            }


            if (paramsValues == null || paramsValues.Count == 0)
            {
                return false;
            }

            switch (indexCode)
            {
                case ConstDefine.Cst_MA:
                    return (paramsValues[0].InputValue == ConstDefine.Cst_MA_Default);
                case ConstDefine.Cst_MACD:
                    if (paramsValues.Count >= 3 &&
                        paramsValues[0].InputValue == ConstDefine.Cst_MACD_Short &&
                        paramsValues[1].InputValue == ConstDefine.Cst_MACD_Long &&
                        paramsValues[2].InputValue == ConstDefine.Cst_MACD_Mid)
                    {
                        return true;
                    }
                    break;
                case ConstDefine.Cst_KDJ:
                    if (paramsValues.Count >= 3 &&
                        paramsValues[0].InputValue == ConstDefine.Cst_KDJ_N &&
                        paramsValues[1].InputValue == ConstDefine.Cst_KDJ_M1 &&
                        paramsValues[2].InputValue == ConstDefine.Cst_KDJ_M2)
                    {
                        return true;
                    }
                    break;
                case ConstDefine.Cst_BOLL:
                    return (paramsValues[0].InputValue == ConstDefine.Cst_BOLL_MA);
                case ConstDefine.Cst_WR:
                    return (paramsValues[0].InputValue == ConstDefine.Cst_WR_Default);
                case ConstDefine.Cst_DMI:
                    if (paramsValues.Count >= 2 &&
                        paramsValues[0].InputValue == ConstDefine.Cst_DMI_N &&
                        paramsValues[1].InputValue == ConstDefine.Cst_DMI_M)
                    {
                        return true;
                    }
                    break;
                case ConstDefine.Cst_RSI:
                    return (paramsValues[0].InputValue == ConstDefine.Cst_RSI_Default);
            }

            return false;
        }

        /// <summary>
        /// 根据指标编码取其对应的菜单类型编码
        /// </summary>
        /// <returns></returns>
        private string GetTypeCodeByIndexCode(string indexCode, List<IndexTreeDTO> list = null)
        {
            if (list == null)
            {
                var dbList = MongoDBHelper.AsQueryable<IndexTreeDTO>().Where(m => m.TypeCode != ConstDefine.CstCode_CommonlyUsedIndex && m.Code == indexCode);
                string tpeCode = dbList.Where(m => m.Code == indexCode).Select(m => m.TypeCode).FirstOrDefault();
                return tpeCode;
            }
            else
            {
                string tpeCode = list.Where(m => m.Code == indexCode).Select(m => m.TypeCode).FirstOrDefault();
                return tpeCode;
            }
        }

        #endregion
    }
}
