using GTA.PI.Logics;
using GTA.PI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SyncDataJob.Utility
{
    class MongodbCacheHelper
    {
        #region 缓存页面的DTO

        #region 缓存指标树菜单

        /// <summary>
        /// 缓存指标菜单
        /// </summary>
        public static void CacheIndexMenu()
        {
            LogHelper.Info("总运行", "开始：生成指标菜单");

            // 获取板块指标信息
            List<PlateInfo> plates = MongoDBHelper.AsQueryable<PlateInfo>().ToList();
            //生成指标树信息
            List<IndexTreeDTO> lstIndexTree = CreateIndexTreeList(plates);

            //缓存指标树
            CacheIndexTree(lstIndexTree);

            LogHelper.Info("总运行", "结束：生成指标菜单");
        }

        /// <summary>
        /// 缓存指标树
        /// </summary>
        /// <param name="lstIndexTree">指标树信息</param>
        private static void CacheIndexTree(List<IndexTreeDTO> lstIndexTree)
        {
            //删除原有指标树
            MongoDBHelper.DeleteMany<IndexTreeDTO>("{}");

            //插入指标树信息
            MongoDBHelper.InsertMany<IndexTreeDTO>(lstIndexTree);
        }

        /// <summary>
        /// 生成指标树信息
        /// </summary>
        /// <param name="lstPlate">概念板块指标</param>
        private static List<IndexTreeDTO> CreateIndexTreeList(IEnumerable<PlateInfo> lstPlate)
        {
            //从json文件取除概念板块以外的其他指标信息
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFileName = Path.Combine(appDataPath, "Data", "JsonData", "IndexTreeDTO.json");
            string jsonData = File.ReadAllText(jsonFileName);
            List<IndexTreeDTO> lstIndexTree = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IndexTreeDTO>>(jsonData);

            //生成概念板块指标
            List<IndexTreeDTO> lstConcept = new List<IndexTreeDTO>();
            var lstConceptPlate = lstPlate.Where(m => m.PlateTreeID == 1011); //只取概念板块指标
            IndexTreeDTO dto3G = null;
            foreach (PlateInfo plate in lstConceptPlate)
            {
                IndexTreeDTO dto = new IndexTreeDTO();

                dto.Code = Convert.ToString(plate.PlateID);
                dto.Name = plate.PlateTitle;
                dto.TypeCode = ConstDefine.CstCode_PlateIndex;
                dto.TypeName = "板块指标";
                dto.SubTypeCode = ConstDefine.CstSubCode_Plate_Concept;
                dto.SubTypeName = "概念板块";
                dto.Desc = string.Empty;

                //产品说把3G放后面
                if (dto.Name == "3G")
                {
                    dto3G = dto;
                    continue;
                }

                lstConcept.Add(dto);
            }

            //产品说把3G放后面
            if (dto3G != null)
            {
                lstConcept.Add(dto3G);
            }
           
            lstIndexTree.AddRange(lstConcept);

            //最近常用指标中插入3个概念板块指标（插到第21、22、23位置）
            int insertIndex = lstIndexTree.Count > 20 ? 20 : lstIndexTree.Count;
            for (int i = 0; i < 3; i++)
            {
                IndexTreeDTO dtoConcept = lstConcept[i];
                IndexTreeDTO dto = new IndexTreeDTO();
                lstIndexTree.Insert(insertIndex + i, dto);

                dto.Code = dtoConcept.Code;
                dto.Name = dtoConcept.Name;
                dto.TypeCode = ConstDefine.CstCode_CommonlyUsedIndex;
                dto.TypeName = "最近常用指标";
                dto.SubTypeCode = ConstDefine.CstSubCode_None;
                dto.SubTypeName = string.Empty;
                dto.Desc = dtoConcept.Desc;
            }

            return lstIndexTree;
        }

        #endregion

        #region 缓存股票指标的明细

        /// <summary>
        /// 缓存股票指标的明细
        /// </summary>
        public static void CacheIndexDetail()
        {
            //缓存财务指标的明细
            CacheFinanceIndex();
            //缓存行情指标的明细
            CacheDataByTimeIndex();
            //缓存板块指标的明细
            CachePlateIndex();
            //缓存技术指标的明细
            CacheTechnicalIndex();
        }

        /// <summary>
        /// 缓存财务指标的明细
        /// </summary>
        public static void CacheFinanceIndex()
        {
            LogHelper.Info("总运行", "开始：生成财务指标明细");

            //取得财务指标编号、名称
            var lstIndexLite = from dto in MongoDBHelper.AsQueryable<IndexTreeDTO>()
                               where dto.TypeCode == ConstDefine.CstCode_FinancialIndex
                               select new IndexLiteDTO()
                               {
                                   Code = dto.Code,
                                   Name = dto.Name 
                               };

            List<DateTime> listDate = TransferHelper.GetFinanceDate();
            //遍历指标编号，生成选股条件信息
            foreach (IndexLiteDTO indexLite in lstIndexLite)
            {
                //取指定指标的所有指标股票信息
                var lstFinanceIndex = MongoDBHelper.AsQueryable<FinanceIndexInfo>().Where(m => m.IndexCode == indexLite.Code && listDate.Contains(m.EndDate)).ToList();
                if (lstFinanceIndex.Count == 0)
                {
                    continue;
                }

                //按截止日期分组
                var groupByEndDateIndex = (from index in lstFinanceIndex
                                           group index by index.EndDate into g
                                           orderby g.Key descending
                                           select new
                                           {
                                               EndDate = g.Key,
                                               EndDateName = g.First().EndDateName,
                                               groupList = g
                                           }).Take(4);
                //截止日期列表(降序)
                var lstEndDate = from groupIndex in groupByEndDateIndex
                                 orderby groupIndex.EndDate descending
                                 select new SelectOption()
                                 {
                                     SelectItem = TransferHelper.DateTimeToString(groupIndex.EndDate),
                                     SelectDisplay = groupIndex.EndDateName
                                 };
                List<SelectOption> selectTermList = lstEndDate.ToList();

                //指标明细
                List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
                //按截止日期遍历
                foreach (var groupByEndDate in groupByEndDateIndex)
                {
                    IndexDetailDTO dto = new IndexDetailDTO();
                    lstDto.Add(dto);

                    dto.Code = indexLite.Code;
                    dto.Name = indexLite.Name;
                    dto.SelectTerm = TransferHelper.DateTimeToString(groupByEndDate.EndDate);
                    dto.SelectTermDisplay = groupByEndDate.EndDateName;
                    dto.SelectTermList = selectTermList;

                    //取分布值
                    var arrayIndexValues = groupByEndDate.groupList.Select(m => m.IndexValue).OrderBy(m => m).ToArray();
                    //设置最大值、最小值、分布值
                    SetMaxMinArrValue(dto, arrayIndexValues, true);
                }

                //设置默认选项(第二个)
                if (lstDto.Count > 1) lstDto[1].IsDefault = true;

                //删除原有的
                MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:'" + indexLite.Code + "'}");
                //插入财务指标的明细
                MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
            }

            LogHelper.Info("总运行", "结束：生成财务指标明细");
        }

        /// <summary>
        /// 缓存行情指标的明细
        /// </summary>
        public static void CacheDataByTimeIndex()
        {
            LogHelper.Info("总运行", "开始：生成行情指标明细");

            //取得财务指标编号、名称
            var lstIndexLite = from dto in MongoDBHelper.AsQueryable<IndexTreeDTO>()
                               where dto.TypeCode == ConstDefine.CstCode_MarketIndex
                               select new IndexLiteDTO()
                               {
                                   Code = dto.Code,
                                   Name = dto.Name
                               };

            //遍历指标编号，生成选股条件信息
            foreach (IndexLiteDTO indexLite in lstIndexLite)
            {
                //取指定指标的所有指标股票信息
                var lstDataByTimeIndex = MongoDBHelper.AsQueryable<DataByTimeIndexInfo>().Where(m => m.IndexCode == indexLite.Code).ToList();
                if (lstDataByTimeIndex.Count == 0)
                {
                    continue;
                }

                //按交易日分组
                var groupByTradingDateIndex = (from index in lstDataByTimeIndex
                                               group index by index.TradingDate into g
                                               orderby g.Key descending
                                               select new
                                               {
                                                   TradingDate = g.Key,
                                                   TradingDateName = g.First().TradingDateName,
                                                   groupList = g
                                               }).Take(5);

                //交易日分组列表
                var lstSelectTerm = from groupIndex in groupByTradingDateIndex
                                    orderby groupIndex.TradingDate descending
                                    select new SelectOption()
                                    {
                                        SelectItem = TransferHelper.DateTimeToString(groupIndex.TradingDate),
                                        SelectDisplay = groupIndex.TradingDateName
                                    };
                List<SelectOption> selectTermList = lstSelectTerm.ToList();

                //指标明细
                List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
                //按交易日遍历
                foreach (var groupByEndDate in groupByTradingDateIndex)
                {
                    IndexDetailDTO dto = new IndexDetailDTO();
                    lstDto.Add(dto);

                    dto.Code = indexLite.Code;
                    dto.Name = indexLite.Name;
                    dto.SelectTerm = TransferHelper.DateTimeToString(groupByEndDate.TradingDate);
                    dto.SelectTermDisplay = groupByEndDate.TradingDateName;
                    dto.SelectTermList = selectTermList;

                    //取分布值
                    var arrayIndexValues = groupByEndDate.groupList.Select(m => m.IndexValue).OrderBy(m => m).ToArray();
                    //设置最大值、最小值、分布值
                    SetMaxMinArrValue(dto, arrayIndexValues);
                }

                //设置默认选项(第一个)
                if (lstDto.Count > 0) lstDto[0].IsDefault = true;

                //删除原有的
                MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:'" + indexLite.Code + "'}");
                //插入行情指标的明细
                MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
            }

            LogHelper.Info("总运行", "结束：生成行情指标明细");
        }

        /// <summary>
        /// 缓存板块指标的明细
        /// </summary>
        public static void CachePlateIndex()
        {
            LogHelper.Info("总运行", "开始：生成板块指标明细");

            //取得财务指标编号、名称
            var lstIndexLite = from dto in MongoDBHelper.AsQueryable<IndexTreeDTO>()
                               where dto.TypeCode == ConstDefine.CstCode_PlateIndex
                               select new IndexLiteDTO()
                               {
                                   Code = dto.Code,
                                   Name = dto.Name
                               };

            //指标明细
            List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();

            //遍历指标编号，生成选股条件信息
            foreach (IndexLiteDTO indexLite in lstIndexLite)
            {
                IndexDetailDTO dto = new IndexDetailDTO();
                lstDto.Add(dto);

                dto.Code = indexLite.Code;
                dto.Name = indexLite.Name;
                dto.IsDefault = true;
            }

            //删除原有的
            string[] lstCode = lstDto.Select(m => m.Code).ToArray();
            string codes = string.Join("','", lstCode);
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:{$in:['" + codes + "']}}");
            //插入板块指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);

            LogHelper.Info("总运行", "结束：生成板块指标明细");
        }

        #region 技术指标

        /// <summary>
        /// 缓存技术指标的明细
        /// </summary>
        public static void CacheTechnicalIndex()
        {
            LogHelper.Info("总运行", "开始：生成技术指标明细");

            //缓存MACD分布值
            CacheTechnicalIndex_MACD();
            //缓存MA分布值
            CacheTechnicalIndex_MA();
            //缓存KDJ分布值
            CacheTechnicalIndex_KDJ();
            //缓存BOLL分布值
            CacheTechnicalIndex_BOLL();
            //缓存WR分布值
            CacheTechnicalIndex_WR();
            //缓存DMI分布值
            CacheTechnicalIndex_DMI();
            //缓存RSI分布值
            CacheTechnicalIndex_RSI();

            LogHelper.Info("总运行", "结束：生成技术指标明细");
        }

        /// <summary>
        /// 缓存MACD分布值
        /// </summary>
        private static void CacheTechnicalIndex_MACD()
        {
            var lstMACD = MongoDBHelper.AsQueryable<MACDResult>().ToList(); ;

            //指标明细
            List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
            List<SelectOption> lstLeft = GetLeftSelectOption(ConstDefine.Cst_MACD);
            foreach (SelectOption option in lstLeft)
            {
                IndexDetailDTO dtoMACD = new IndexDetailDTO();
                lstDto.Add(dtoMACD);

                dtoMACD.Code = ConstDefine.Cst_MACD;
                dtoMACD.Name = ConstDefine.Cst_MACD;
                dtoMACD.ParamsValues = GetInputParams(ConstDefine.Cst_MACD);

                dtoMACD.SelectTerm = option.SelectItem;
                dtoMACD.SelectTermDisplay = option.SelectItem;
                dtoMACD.SelectTermList = lstLeft;

                //取分布值
                double[] arrayIndexValues = null;
                switch(option.SelectItem)
                {
                    case ConstDefine.Cst_MACD_MACD:
                        dtoMACD.IsDefault = true;
                        arrayIndexValues = lstMACD.Select(m => m.MACD).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_MACD_DIF:
                        arrayIndexValues = lstMACD.Select(m => m.DIF).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_MACD_DEA:
                        arrayIndexValues = lstMACD.Select(m => m.DEA).OrderBy(m => m).ToArray();
                        break;
                }
                //设置最大值、最小值、分布值
                SetMaxMinArrValue(dtoMACD, arrayIndexValues);
            }

            Dictionary<string, string> dicCodeName = new Dictionary<string, string>();
            dicCodeName[ConstDefine.Cst_MACD_TopDepart] = ConstDefine.Cst_MACD_TopDepart_Name;
            dicCodeName[ConstDefine.Cst_MACD_DownDepart] = ConstDefine.Cst_MACD_DownDepart_Name;
            dicCodeName[ConstDefine.Cst_MACD_GoldX] = ConstDefine.Cst_MACD_GoldX_Name;
            dicCodeName[ConstDefine.Cst_MACD_BlackX] = ConstDefine.Cst_MACD_BlackX_Name;
            foreach (string indexCode in dicCodeName.Keys)
            {
                IndexDetailDTO dto = new IndexDetailDTO();
                lstDto.Add(dto);

                dto.Code = indexCode;
                dto.Name = dicCodeName[indexCode];
                dto.ParamsValues = GetInputParams(ConstDefine.Cst_MACD);
                dto.IsDefault = true;
            }

            //删除原有的
            List<string> lstIndexCode = new List<string>();
            lstIndexCode.Add(ConstDefine.Cst_MACD);
            lstIndexCode.AddRange(dicCodeName.Keys);
            string codes = string.Join("','", lstIndexCode);
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:{$in:['" + codes + "']}}");

            //插入MACD技术指标指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
        }

        /// <summary>
        /// 缓存MA分布值
        /// </summary>
        private static void CacheTechnicalIndex_MA()
        {
            var lstMA = MongoDBHelper.AsQueryable<MAResult>().ToList();

            IndexDetailDTO dtoMA = new IndexDetailDTO();
            dtoMA.Code = ConstDefine.Cst_MA;
            dtoMA.Name = ConstDefine.Cst_MA;
            dtoMA.ParamsValues = GetInputParams(ConstDefine.Cst_MA);
            dtoMA.IsDefault = true;

            //取分布值
            var arrayIndexValues = lstMA.Select(m => m.MA).OrderBy(m => m).ToArray();
            //设置最大值、最小值、分布值
            SetMaxMinArrValue(dtoMA, arrayIndexValues);

            //删除原有的
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:'" + ConstDefine.Cst_MA + "'}");
            //插入行情指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(new List<IndexDetailDTO>() { dtoMA });
        }

        /// <summary>
        /// 缓存KDJ分布值
        /// </summary>
        private static void CacheTechnicalIndex_KDJ()
        {
            var lstKDJ = MongoDBHelper.AsQueryable<KDJResult>().ToList(); ;

            //指标明细
            List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
            List<SelectOption> lstLeft = GetLeftSelectOption(ConstDefine.Cst_KDJ);
            foreach (SelectOption option in lstLeft)
            {
                IndexDetailDTO dtoKDJ = new IndexDetailDTO();
                lstDto.Add(dtoKDJ);

                dtoKDJ.Code = ConstDefine.Cst_KDJ;
                dtoKDJ.Name = ConstDefine.Cst_KDJ;
                dtoKDJ.ParamsValues = GetInputParams(ConstDefine.Cst_KDJ);

                dtoKDJ.SelectTerm = option.SelectItem;
                dtoKDJ.SelectTermDisplay = option.SelectItem;
                dtoKDJ.SelectTermList = lstLeft;

                //取分布值
                double[] arrayIndexValues = null;
                switch (option.SelectItem)
                {
                    case ConstDefine.Cst_KDJ_K:
                        dtoKDJ.IsDefault = true;
                        arrayIndexValues = lstKDJ.Select(m => m.K).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_KDJ_D:
                        arrayIndexValues = lstKDJ.Select(m => m.D).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_KDJ_J:
                        arrayIndexValues = lstKDJ.Select(m => m.J).OrderBy(m => m).ToArray();
                        break;
                }

                //设置最大值、最小值、分布值
                SetMaxMinArrValue(dtoKDJ, arrayIndexValues);
            }

            Dictionary<string, string> dicCodeName = new Dictionary<string, string>();
            dicCodeName[ConstDefine.Cst_KDJ_GoldX] = ConstDefine.Cst_KDJ_GoldX_Name;
            dicCodeName[ConstDefine.Cst_KDJ_BlackX] = ConstDefine.Cst_KDJ_BlackX_Name;
            dicCodeName[ConstDefine.Cst_KDJ_LowGoldX] = ConstDefine.Cst_KDJ_LowGoldX_Name;
            dicCodeName[ConstDefine.Cst_KDJ_TopDepart] = ConstDefine.Cst_KDJ_TopDepart_Name;
            dicCodeName[ConstDefine.Cst_KDJ_DownDepart] = ConstDefine.Cst_KDJ_DownDepart_Name;
            dicCodeName[ConstDefine.Cst_KDJ_Oversold] = ConstDefine.Cst_KDJ_Oversold_Name;
            dicCodeName[ConstDefine.Cst_KDJ_Overbought] = ConstDefine.Cst_KDJ_Overbought_Name;
            foreach (string indexCode in dicCodeName.Keys)
            {
                IndexDetailDTO dto = new IndexDetailDTO();
                lstDto.Add(dto);

                dto.Code = indexCode;
                dto.Name = dicCodeName[indexCode];
                dto.ParamsValues = GetInputParams(ConstDefine.Cst_KDJ);
                dto.IsDefault = true;
            }

            //删除原有的
            List<string> lstIndexCode = new List<string>();
            lstIndexCode.Add(ConstDefine.Cst_KDJ);
            lstIndexCode.AddRange(dicCodeName.Keys);
            string codes = string.Join("','", lstIndexCode);
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:{$in:['" + codes + "']}}");

            //插入MACD技术指标指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
        }

        /// <summary>
        /// 缓存BOLL分布值
        /// </summary>
        private static void CacheTechnicalIndex_BOLL()
        {
            var lstBOLL = MongoDBHelper.AsQueryable<BOLLResult>().ToList(); ;

            //指标明细
            List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
            List<SelectOption> lstLeft = GetLeftSelectOption(ConstDefine.Cst_BOLL);
            foreach (SelectOption option in lstLeft)
            {
                IndexDetailDTO dtoBOLL = new IndexDetailDTO();
                lstDto.Add(dtoBOLL);

                dtoBOLL.Code = ConstDefine.Cst_BOLL;
                dtoBOLL.Name = ConstDefine.Cst_BOLL;
                dtoBOLL.ParamsValues = GetInputParams(ConstDefine.Cst_BOLL);

                dtoBOLL.SelectTerm = option.SelectItem;
                dtoBOLL.SelectTermDisplay = option.SelectItem;
                dtoBOLL.SelectTermList = lstLeft;

                //取分布值
                double[] arrayIndexValues = null;
                switch (option.SelectItem)
                {
                    case ConstDefine.Cst_BOLL_UP:
                        dtoBOLL.IsDefault = true;
                        arrayIndexValues = lstBOLL.Select(m => m.UP).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_BOLL_MB:
                        arrayIndexValues = lstBOLL.Select(m => m.MB).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_BOLL_DN:
                        arrayIndexValues = lstBOLL.Select(m => m.DN).OrderBy(m => m).ToArray();
                        break;
                }
                //设置最大值、最小值、分布值
                SetMaxMinArrValue(dtoBOLL, arrayIndexValues);
            }

            Dictionary<string, string> dicCodeName = new Dictionary<string, string>();
            dicCodeName[ConstDefine.Cst_BOLL_Upper] = ConstDefine.Cst_BOLL_Upper_Name;
            dicCodeName[ConstDefine.Cst_BOLL_Mid] = ConstDefine.Cst_BOLL_Mid_Name;
            foreach (string indexCode in dicCodeName.Keys)
            {
                IndexDetailDTO dto = new IndexDetailDTO();
                lstDto.Add(dto);

                dto.Code = indexCode;
                dto.Name = dicCodeName[indexCode];
                dto.ParamsValues = GetInputParams(ConstDefine.Cst_BOLL);
                dto.IsDefault = true;
            }

            //删除原有的
            List<string> lstIndexCode = new List<string>();
            lstIndexCode.Add(ConstDefine.Cst_BOLL);
            lstIndexCode.AddRange(dicCodeName.Keys);
            string codes = string.Join("','", lstIndexCode);
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:{$in:['" + codes + "']}}");

            //插入MACD技术指标指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
        }

        /// <summary>
        /// 缓存WR分布值
        /// </summary>
        private static void CacheTechnicalIndex_WR()
        {
            var lstWR = MongoDBHelper.AsQueryable<WRResult>().ToList(); ;

            //指标明细
            IndexDetailDTO dtoWR = new IndexDetailDTO();
            dtoWR.Code = ConstDefine.Cst_WR;
            dtoWR.Name = ConstDefine.Cst_WR;
            dtoWR.ParamsValues = GetInputParams(ConstDefine.Cst_WR);
            dtoWR.IsDefault = true;

            //取分布值
            double[] arrayIndexValues = null;
            arrayIndexValues = lstWR.Select(m => m.WR).OrderBy(m => m).ToArray();
            //设置最大值、最小值、分布值
            SetMaxMinArrValue(dtoWR, arrayIndexValues);

            //删除原有的
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:'" + ConstDefine.Cst_WR + "'}");
            //插入WR技术指标指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(new List<IndexDetailDTO>() { dtoWR });
        }

        /// <summary>
        /// 缓存DMI分布值
        /// </summary>
        private static void CacheTechnicalIndex_DMI()
        {
            var lstDMI = MongoDBHelper.AsQueryable<DMIResult>().ToList(); ;

            //指标明细
            List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
            List<SelectOption> lstLeft = GetLeftSelectOption(ConstDefine.Cst_DMI);
            foreach (SelectOption option in lstLeft)
            {
                IndexDetailDTO dtoBOLL = new IndexDetailDTO();
                lstDto.Add(dtoBOLL);

                dtoBOLL.Code = ConstDefine.Cst_DMI;
                dtoBOLL.Name = ConstDefine.Cst_DMI;
                dtoBOLL.ParamsValues = GetInputParams(ConstDefine.Cst_DMI);

                dtoBOLL.SelectTerm = option.SelectItem;
                dtoBOLL.SelectTermDisplay = option.SelectItem;
                dtoBOLL.SelectTermList = lstLeft;

                //取分布值
                double[] arrayIndexValues = null;
                switch (option.SelectItem)
                {
                    case ConstDefine.Cst_DMI_PDI:
                        dtoBOLL.IsDefault = true;
                        arrayIndexValues = lstDMI.Select(m => m.PDI).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_DMI_MDI:
                        arrayIndexValues = lstDMI.Select(m => m.MDI).OrderBy(m => m).ToArray();
                        break;
                    case ConstDefine.Cst_DMI_ADX:
                        arrayIndexValues = lstDMI.Where(m => m.ADX.HasValue).Select(m => m.ADX.Value).OrderBy(m => m).ToArray();
                        break;
                }
                //设置最大值、最小值、分布值
                SetMaxMinArrValue(dtoBOLL, arrayIndexValues);
            }

            Dictionary<string, string> dicCodeName = new Dictionary<string, string>();
            dicCodeName[ConstDefine.Cst_DMI_GoldX] = ConstDefine.Cst_DMI_GoldX_Name;
            dicCodeName[ConstDefine.Cst_DMI_BlackX] = ConstDefine.Cst_DMI_BlackX_Name;
            foreach (string indexCode in dicCodeName.Keys)
            {
                IndexDetailDTO dto = new IndexDetailDTO();
                lstDto.Add(dto);

                dto.Code = indexCode;
                dto.Name = dicCodeName[indexCode];
                dto.ParamsValues = GetInputParams(ConstDefine.Cst_DMI);
                dto.IsDefault = true;
            }

            //删除原有的
            List<string> lstIndexCode = new List<string>();
            lstIndexCode.Add(ConstDefine.Cst_DMI);
            lstIndexCode.AddRange(dicCodeName.Keys);
            string codes = string.Join("','", lstIndexCode);
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:{$in:['" + codes + "']}}");

            //插入MACD技术指标指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
        }

        /// <summary>
        /// 缓存RSI分布值
        /// </summary>
        private static void CacheTechnicalIndex_RSI()
        {
            var lstRSI = MongoDBHelper.AsQueryable<RSIResult>().ToList(); ;

            //指标明细
            List<IndexDetailDTO> lstDto = new List<IndexDetailDTO>();
            IndexDetailDTO dtoRSI = new IndexDetailDTO();
            lstDto.Add(dtoRSI);

            dtoRSI.Code = ConstDefine.Cst_RSI;
            dtoRSI.Name = ConstDefine.Cst_RSI;
            dtoRSI.ParamsValues = GetInputParams(ConstDefine.Cst_RSI);
            dtoRSI.IsDefault = true;

            //取分布值
            double[] arrayIndexValues = lstRSI.Select(m => m.RSI).OrderBy(m => m).ToArray();
            //设置最大值、最小值、分布值
            SetMaxMinArrValue(dtoRSI, arrayIndexValues);

            Dictionary<string, string> dicCodeName = new Dictionary<string, string>();
            dicCodeName[ConstDefine.Cst_RSI_GoldX] = ConstDefine.Cst_RSI_GoldX_Name;
            dicCodeName[ConstDefine.Cst_RSI_BlackX] = ConstDefine.Cst_RSI_BlackX_Name;
            foreach (string indexCode in dicCodeName.Keys)
            {
                IndexDetailDTO dto = new IndexDetailDTO();
                lstDto.Add(dto);

                dto.Code = indexCode;
                dto.Name = dicCodeName[indexCode];
                dto.ParamsValues = GetInputParams(ConstDefine.Cst_RSI);
                dto.IsDefault = true;
            }

            //删除原有的
            List<string> lstIndexCode = new List<string>();
            lstIndexCode.Add(ConstDefine.Cst_RSI);
            lstIndexCode.AddRange(dicCodeName.Keys);
            string codes = string.Join("','", lstIndexCode);
            MongoDBHelper.DeleteMany<IndexDetailDTO>("{Code:{$in:['" + codes + "']}}");

            //插入MACD技术指标指标的明细
            MongoDBHelper.InsertManyAsync<IndexDetailDTO>(lstDto);
        }

        /// <summary>
        /// 取得技术指标选项
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        private static List<SelectOption> GetLeftSelectOption(string indexCode)
        {
            List<SelectOption> list = new List<SelectOption>();

            switch (indexCode)
            {
                case "MACD":
                    {
                        SelectOption option = new SelectOption();
                        option.SelectItem = ConstDefine.Cst_MACD_MACD;
                        option.SelectDisplay = ConstDefine.Cst_MACD_MACD;
                        list.Add(option);

                        SelectOption option2 = new SelectOption();
                        option2.SelectItem = ConstDefine.Cst_MACD_DIF;
                        option2.SelectDisplay = ConstDefine.Cst_MACD_DIF;
                        list.Add(option2);

                        SelectOption option3 = new SelectOption();
                        option3.SelectItem = ConstDefine.Cst_MACD_DEA;
                        option3.SelectDisplay = ConstDefine.Cst_MACD_DEA;
                        list.Add(option3);
                    }

                    break;
                case "KDJ":
                    {
                        SelectOption option1 = new SelectOption();
                        option1.SelectItem = ConstDefine.Cst_KDJ_K;
                        option1.SelectDisplay = ConstDefine.Cst_KDJ_K;
                        list.Add(option1);

                        SelectOption option2 = new SelectOption();
                        option2.SelectItem = ConstDefine.Cst_KDJ_D;
                        option2.SelectDisplay = ConstDefine.Cst_KDJ_D;
                        list.Add(option2);

                        SelectOption option3 = new SelectOption();
                        option3.SelectItem = ConstDefine.Cst_KDJ_J;
                        option3.SelectDisplay = ConstDefine.Cst_KDJ_J;
                        list.Add(option3);
                    }

                    break;
                case "BOLL":
                    {
                        SelectOption option1 = new SelectOption();
                        option1.SelectItem = ConstDefine.Cst_BOLL_UP;
                        option1.SelectDisplay = ConstDefine.Cst_BOLL_UP;
                        list.Add(option1);

                        SelectOption option2 = new SelectOption();
                        option2.SelectItem = ConstDefine.Cst_BOLL_MB;
                        option2.SelectDisplay = ConstDefine.Cst_BOLL_MB;
                        list.Add(option2);

                        SelectOption option3 = new SelectOption();
                        option3.SelectItem = ConstDefine.Cst_BOLL_DN;
                        option3.SelectDisplay = ConstDefine.Cst_BOLL_DN;
                        list.Add(option3);
                    }

                    break;
                case "DMI":
                    {
                        SelectOption option1 = new SelectOption();
                        option1.SelectItem = ConstDefine.Cst_DMI_PDI;
                        option1.SelectDisplay = ConstDefine.Cst_DMI_PDI;
                        list.Add(option1);

                        SelectOption option2 = new SelectOption();
                        option2.SelectItem = ConstDefine.Cst_DMI_MDI;
                        option2.SelectDisplay = ConstDefine.Cst_DMI_MDI;
                        list.Add(option2);

                        SelectOption option3 = new SelectOption();
                        option3.SelectItem = ConstDefine.Cst_DMI_ADX;
                        option3.SelectDisplay = ConstDefine.Cst_DMI_ADX;
                        list.Add(option3);
                    }

                    break;
            }

            return list;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        private static List<InputParams> GetInputParams(string indexCode)
        {
            List<InputParams> list = new List<InputParams>();
            double[] values = null;
            switch (indexCode)
            {
                case "MACD":
                    values = new double[] { 12, 26, 9 };
                    break;
                case "MA":
                    values = new double[] { 20 };
                    break;
                case "KDJ":
                    values = new double[] { 9, 3, 3 };
                    break;
                case "BOLL":
                    values = new double[] { 20 };
                    break;
                case "RSI":
                    values = new double[] { 14 };
                    break;
                case "WR":
                    values = new double[] { 14 };
                    break;
                case "DMI":
                    values = new double[] { 14, 6 };
                    break;
            }

            if (values == null)
            {
                return list;
            }

            foreach (double value in values)
            {
                InputParams param = new InputParams();
                param.InputValue = value;
                list.Add(param);
            }

            return list;
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// 取得指定天数的前交易日(不包括今天,降序)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static List<DateTime> GetPreTradeDateDescending(int days)
        {
            List<DateTime> lstTradeDate = MongoDBHelper.AsQueryable<TradeCalendarInfo>()
                                                        .Where(m => m.IsOpen == "Y" && m.Market == "SZSE" && m.CalendarDate != DateTime.Today)
                                                        .OrderByDescending(m => m.CalendarDate)
                                                        .Take(days)
                                                        .Select(m => m.CalendarDate)
                                                        .ToList();

            return lstTradeDate;
        }

        /// <summary>
        /// 退一法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double Floor(double value, int decimals)
        {
            int factor = (int)Math.Pow(10, decimals);
            return Math.Floor(value * factor) / factor;
        }

        /// <summary>
        /// 进一法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double Ceiling(double value, int decimals)
        {
            int factor = (int)Math.Pow(10, decimals);
            return Math.Ceiling(value * factor) / factor;
        }

        /// <summary>
        /// 设置最大值、最小值、分布值
        /// </summary>
        private static void SetMaxMinArrValue(IndexDetailDTO dto, double[] arrayIndexValues, bool isFinanceIndex = false)
        {
            if (arrayIndexValues == null || arrayIndexValues.Length <= 0)
            {
                return;
            }

            //财务指标如果超过100只股票，舍弃前后5个股票
            if (isFinanceIndex && arrayIndexValues.Length > 100)
            {
                arrayIndexValues = arrayIndexValues.Skip(5).Take(arrayIndexValues.Length - 10).ToArray();
            }

            arrayIndexValues[0] = Floor(arrayIndexValues[0], 2);    //退一法
            int arrMaxIndex = arrayIndexValues.Length - 1;
            arrayIndexValues[arrMaxIndex] = Ceiling(arrayIndexValues[arrMaxIndex], 2);  //进一法

            dto.MinValue = arrayIndexValues[0];
            dto.MaxValue = arrayIndexValues[arrayIndexValues.Length - 1];
            dto.Value = arrayIndexValues;
        }
    }
}
