using GTA.PI.Models;
using GTA.Quantrader.DSP.ObjectDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SyncDataJob.Utility
{
    public class TransferHelper
    {
        /// <summary>
        /// 转换成指定的模板
        /// (类型必须完全一致)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> _Transfer2List<T>(object obj)
        {
            List<T> lstModel = new List<T>();

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (pro.Name.EndsWith("Specified"))
                {
                    continue;
                }

                if (pro.PropertyType.IsGenericType)
                {
                    if (pro.PropertyType.GetGenericArguments()[0] == typeof(DataColumn))
                    {
                        List<DataColumn> lstCol = pro.GetValue(obj, null) as List<DataColumn>;
                        Dictionary<string, string[]> dicStr = new Dictionary<string, string[]>();

                        PropertyInfo[] modelProperties = typeof(T).GetProperties();
                        var modelPropertieNames = modelProperties.Select(m => m.Name);

                        Dictionary<string, int> dicNameIndex = GetColIndexAndStringValue(lstCol, ref dicStr, modelPropertieNames.ToArray());

                        uint count = lstCol[0].count;
                        for (int i = 0; i < count; i++)
                        {
                            T model = Activator.CreateInstance<T>();
                            lstModel.Add(model);
                            Type typeModel = model.GetType();

                            foreach (string colName in dicNameIndex.Keys)
                            {
                                int colIndex = dicNameIndex[colName];
                                DataColumn col = lstCol[colIndex];

                                string[] strValues = dicStr.ContainsKey(colName) ? dicStr[colName] : null;
                                object objValue = GetValueObj(i, col.type, col.data, strValues);
                                if (IsInvalidValue(col.type, objValue)) //判断是否非法数值
                                {
                                    continue;
                                }
                                typeModel.GetProperty(colName).SetValue(model, objValue);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("未处理的类型");
                    }
                }
            }

            return lstModel;
        }

        /// <summary>
        /// 转换成指定的模板
        /// (自适应T的string型)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> Transfer2List<T>(object obj)
        {
            List<T> lstModel = new List<T>();

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (pro.Name.EndsWith("Specified"))
                {
                    continue;
                }
                
                if (pro.PropertyType.IsGenericType)
                {
                    if (pro.PropertyType.GetGenericArguments()[0] == typeof(DataColumn))
                    {
                        List<DataColumn> lstCol = pro.GetValue(obj, null) as List<DataColumn>;
                        Dictionary<string, string[]> dicStr = new Dictionary<string, string[]>();

                        PropertyInfo[] modelProperties = typeof(T).GetProperties();
                        var modelPropertieNames = modelProperties.Select(m => m.Name);

                        Dictionary<string, int> dicNameIndex = GetColIndexAndStringValue(lstCol, ref dicStr, modelPropertieNames.ToArray());

                        uint count = lstCol[0].count;
                        for (int i = 0; i < count; i++)
                        {
                            T model = Activator.CreateInstance<T>();
                            lstModel.Add(model);
                            Type typeModel = model.GetType();

                            foreach (string colName in dicNameIndex.Keys)
                            {
                                int colIndex = dicNameIndex[colName];
                                DataColumn col = lstCol[colIndex];

                                string[] strValues = dicStr.ContainsKey(colName) ? dicStr[colName] : null;
                                object objValue = GetValueObj(i, col.type, col.data, strValues);

                                if (IsInvalidValue(col.type, objValue)) //判断是否非法数值
                                {
                                    continue;
                                }

                                if (col.type != EDataType.TypeChar && typeModel.GetProperty(colName).PropertyType == typeof(string))
                                {
                                    if(col.type == EDataType.TypeDateTime)
                                    {
                                        string strValue = TransferHelper.DateTimeToString(Convert.ToDateTime(objValue));
                                        typeModel.GetProperty(colName).SetValue(model, strValue);
                                    }
                                    else
                                    {
                                        string strValue = Convert.ToString(objValue);
                                        typeModel.GetProperty(colName).SetValue(model, strValue);
                                    }
                                }
                                else if (col.type == EDataType.TypeInt64 && typeModel.GetProperty(colName).PropertyType == typeof(ulong))
                                {
                                    ulong ulongValue = Convert.ToUInt64(objValue);
                                    typeModel.GetProperty(colName).SetValue(model, ulongValue);
                                }
                                else
                                {
                                    typeModel.GetProperty(colName).SetValue(model, objValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("未处理的类型");
                    }
                }
            }

            return lstModel;
        }

        /// <summary>
        /// 财务指标信息转成指标股票信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<FinanceIndexInfo> Transfer2ListFinanceIndexInfo(object obj, ulong securityID)
        {
            List<FinanceIndexInfo> lstInfo = new List<FinanceIndexInfo>();
            List<DateTime> listDate = GetFinanceDate();

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (pro.Name.EndsWith("Specified"))
                {
                    continue;
                }

                if (pro.PropertyType.IsGenericType)
                {
                    if (pro.PropertyType.GetGenericArguments()[0] == typeof(DataColumn))
                    {
                        List<DataColumn> lstCol = pro.GetValue(obj, null) as List<DataColumn>;
                        if (lstCol.Count == 0)
                        {
                            continue;
                        }

                        Dictionary<string, string[]> dicStr = GetStringValue(lstCol);

                        DataColumn colSymbol = null;        //股票编码列
                        DataColumn colEndDate = null;       //截止日期列
                        foreach (DataColumn col in lstCol)
                        {
                            if (col.name == "Symbol")
                            {
                                colSymbol = col;
                            }
                            else if (col.name == "EndDate")
                            {
                                colEndDate = col;
                            }
                        }

                        uint count = lstCol[0].count;
                        for (int i = 0; i < count; i++)
                        {
                            //股票编号
                            string symbol = Convert.ToString(GetValueObj(i, colSymbol.type, colSymbol.data, dicStr[colSymbol.name]));
                            //截止日期
                            DateTime endDate = Convert.ToDateTime(GetValueObj(i, colEndDate.type, colEndDate.data, null));
                            if (listDate.Contains(endDate) == false)
                            {
                                continue;
                            }

                            foreach (DataColumn col in lstCol)
                            {
                                if (col == colSymbol || col == colEndDate)
                                {
                                    //股票安全码列、股票列、截止日期列跳过，只处理指标列
                                    continue;
                                }

                                object objValue = GetValueObj(i, col.type, col.data, null);
                                if (IsInvalidDecValue(col.type, objValue) == false) //判断是否非法数值
                                {
                                    FinanceIndexInfo info = new FinanceIndexInfo();
                                    lstInfo.Add(info);

                                    info.SecurityID = securityID;
                                    info.Symbol = symbol;
                                    info.EndDate = endDate;
                                    info.EndDateName = GetTermDisplay_Quarter(endDate);
                                    info.IndexCode = col.name;
                                    info.IndexValue = Convert.ToDouble(objValue);
                                }
                            }
                        }
                    }
                }
            }

            return lstInfo;
        }

        /// <summary>
        /// 行情指标信息转成指标股票信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<DataByTimeIndexInfo> Transfer2ListDataByTimeIndexInfo(object obj)
        {
            //这些字段名不是指标名
            List<string> lstNotIndexCode = new List<string>() { "SecurityID", "TradingDate", "Symbol", "ShortName", "Filling" };
            List<DataByTimeIndexInfo> lstInfo = new List<DataByTimeIndexInfo>();

            //前5个交易日
            List<DateTime> lstTradeDate = MongodbCacheHelper.GetPreTradeDateDescending(5);

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (pro.Name.EndsWith("Specified"))
                {
                    continue;
                }

                if (pro.PropertyType.IsGenericType)
                {
                    if (pro.PropertyType.GetGenericArguments()[0] == typeof(DataColumn))
                    {
                        List<DataColumn> lstCol = pro.GetValue(obj, null) as List<DataColumn>;
                        if (lstCol.Count == 0)
                        {
                            continue;
                        }

                        DataColumn colSecurityID = null;    //安全码列
                        DataColumn colSymbol = null;        //股票列
                        DataColumn colTradingDate = null;   //交易日列
                        DataColumn colFilling = null;       //填充判断列 Filling=0：正常数据；其他：填充数据。

                        foreach (DataColumn col in lstCol)
                        {
                            if (col.name == "Symbol")
                            {
                                colSymbol = col;
                            }
                            else if (col.name == "SecurityID")
                            {
                                colSecurityID = col;
                            }
                            else if (col.name == "TradingDate")
                            {
                                colTradingDate = col;
                            }
                            else if (col.name == "Filling")
                            {
                                colFilling = col;
                            }
                        }

                        Dictionary<string, string[]> dicStr = GetStringValue(lstCol);
                        uint count = lstCol[0].count;
                        for (int i = 0; i < count; i++)
                        {
                            string filling = Convert.ToString(GetValueObj(i, colSymbol.type, colSymbol.data, dicStr[colFilling.name]));
                            if (filling != "0")
                            {
                                continue;//不是填充数据不处理
                            }

                            //安全码
                            ulong securityID = Convert.ToUInt64(GetValueObj(i, colSecurityID.type, colSecurityID.data, null));
                            //股票编号
                            string symbol = Convert.ToString(GetValueObj(i, colSymbol.type, colSymbol.data, dicStr[colSymbol.name]));
                            //交易日
                            DateTime tradingDate = Convert.ToDateTime(GetValueObj(i, colTradingDate.type, colTradingDate.data, null));

                            foreach (DataColumn col in lstCol)
                            {
                                if (lstNotIndexCode.Contains(col.name))
                                {
                                    //只处理指标列
                                    continue;
                                }

                                object objValue = GetValueObj(i, col.type, col.data, null);
                                if (IsInvalidDecValue(col.type, objValue) == false) //判断是否非法数值
                                {
                                    DataByTimeIndexInfo info = new DataByTimeIndexInfo();
                                    lstInfo.Add(info);

                                    info.SecurityID = securityID;
                                    info.Symbol = symbol;
                                    info.TradingDate = tradingDate;
                                    info.TradingDateName = GetTermDisplay_Date(tradingDate, lstTradeDate);
                                    info.IndexCode = col.name;
                                    info.IndexValue = Convert.ToDouble(objValue);
                                    info.Filling = filling;
                                }
                            }
                        }
                    }
                }
            }

            return lstInfo;
        }

        /// <summary>
        /// 取季报显示文本
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetTermDisplay_Quarter(DateTime date)
        {
            if (date.Month <= 3)
            {
                return string.Format("{0}年 第一季度报", date.Year);
            }
            else if (date.Month >= 4 && date.Month <= 6)
            {
                return string.Format("{0}年 中报", date.Year);
            }
            else if (date.Month >= 7 && date.Month <= 9)
            {
                return string.Format("{0}年 第三季度报", date.Year);
            }
            else if (date.Month >= 10)
            {
                return string.Format("{0}年 年报", date.Year);
            }

            return "";
        }

        /// <summary>
        /// 取交易日显示文本
        /// </summary>
        /// <param name="tradingDate"></param>
        /// <returns></returns>
        public static string GetTermDisplay_Date(DateTime tradingDate, List<DateTime> lstTradingDate)
        {
            int index = lstTradingDate.IndexOf(tradingDate);
            if (index == -1)
            {
                //不可能进这里
                return "当前交易日";
            }

            string[] dateDisplays = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            return string.Format("前{0}交易日", dateDisplays[index]);
        }

        /// <summary>
        /// 取得指定列的索引及字符串内容（如果是类型是字符串）
        /// </summary>
        /// <param name="lstCol"></param>
        /// <param name="dicStrValue"></param>
        /// <param name="colNames"></param>
        /// <returns></returns>
        private static Dictionary<string, int> GetColIndexAndStringValue(List<DataColumn> lstCol, ref Dictionary<string, string[]> dicStrValue, params string[] colNames)
        {
            Dictionary<string, int> dicNameIndex = new Dictionary<string, int>();
            List<string> lstColName = new List<string>(colNames);
            for (int i = 0; i < lstCol.Count; i++)
            {
                DataColumn col = lstCol[i];
                if (lstColName.Contains(col.name))
                {
                    dicNameIndex[col.name] = i;

                    if (col.type == EDataType.TypeChar)
                    {
                        string[] items = Encoding.UTF8.GetString(col.data).Split('\0');
                        dicStrValue.Add(col.name, items);
                    }
                }
            }

            return dicNameIndex;
        }

        /// <summary>
        /// 取得字符串内容（如果是类型是字符串）
        /// </summary>
        /// <param name="lstCol"></param>
        /// <returns></returns>
        private static Dictionary<string, string[]> GetStringValue(List<DataColumn> lstCol)
        {
            Dictionary<string, string[]> dicStrValue = new Dictionary<string, string[]>();
            for (int i = 0; i < lstCol.Count; i++)
            {
                DataColumn col = lstCol[i];
                if (col.type == EDataType.TypeChar)
                {
                    string[] items = Encoding.UTF8.GetString(col.data).Split('\0');
                    dicStrValue.Add(col.name, items);
                }
            }

            return dicStrValue;
        }

        /// <summary>
        /// 从字节数据中取值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="temp"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static object GetValueObj(int index, EDataType type, byte[] temp, string[] dic)
        {
            try
            {
                switch (type)
                {
                    case EDataType.TypeBool:
                        return System.BitConverter.ToBoolean(temp, index * 1);
                    case EDataType.TypeByte: return temp[index * 1];
                    case EDataType.TypeChar:
                        if (dic == null)
                            return null;
                        return dic[index] ?? null;
                    case EDataType.TypeDouble: return System.BitConverter.ToDouble(temp, index * 8);
                    case EDataType.TypeFloat: return System.BitConverter.ToSingle(temp, index * 4);
                    case EDataType.TypeInt16: return System.BitConverter.ToInt16(temp, index * 2);
                    case EDataType.TypeInt32: return System.BitConverter.ToInt32(temp, index * 4);
                    case EDataType.TypeInt64: return System.BitConverter.ToInt64(temp, index * 8);
                    case EDataType.TypeDateTime:
                        ulong dtlong = System.BitConverter.ToUInt64(temp, index * 8);
                        if (dtlong == 0)
                        {
                            return null;
                        }
                        else
                        {
                            DateTime startTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1));
                            DateTime time = startTime.AddMilliseconds(dtlong);
                            time = TimeZone.CurrentTimeZone.ToLocalTime(time);

                            return time;
                        }
                    case EDataType.TypeInt8: return temp[index * 1];

                    case EDataType.TypeUInt16: return System.BitConverter.ToUInt16(temp, index * 2);
                    case EDataType.TypeUInt32: return System.BitConverter.ToUInt32(temp, index * 4);
                    case EDataType.TypeUInt64: return System.BitConverter.ToUInt64(temp, index * 8);
                    case EDataType.TypeUInt8:
                        return temp[index * 1];
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断是否非法数值
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        private static bool IsInvalidDecValue(EDataType type, object value)
        {
            //各数据类型的最大值表示无效值/数据不存在
            //#define MaxByte     0xff,
            //#define MaxInt8     0x7f,
            //#define MaxUInt8    0xff,
            //#define MaxInt16    0x7fff,
            //#define MaxUInt16   0xffff,
            //#define MaxInt32    0x7fffffff,
            //#define MaxUInt32   0xffffffff,
            //#define MaxInt64    0x7fffffffffffffff,
            //#define MaxUInt64   0xffffffffffffffff,
            //#define MaxFloat    (0x7f7fffff)f,        //3.402823466e+38f
            //#define MaxDouble   0x7fefffffffffffff,   //1.7976931348623157e+308

            switch (type)
            {
                case EDataType.TypeDouble:
                    if (double.IsNaN((double)value)) return true;
                    return value.Equals(double.MaxValue);
                case EDataType.TypeByte:
                    return value.Equals(byte.MaxValue);
                case EDataType.TypeInt8:
                    return value.Equals((int)(byte.MaxValue / 2));
                case EDataType.TypeUInt8:
                    return value.Equals(byte.MaxValue);
                case EDataType.TypeInt16:
                    return value.Equals(Int16.MaxValue);
                case EDataType.TypeUInt16:
                    return value.Equals(UInt16.MaxValue);
                case EDataType.TypeInt32:
                    return value.Equals(Int32.MaxValue);
                case EDataType.TypeUInt32:
                    return value.Equals(UInt32.MaxValue);
                case EDataType.TypeInt64:
                    return value.Equals(Int64.MaxValue);
                case EDataType.TypeUInt64:
                    return value.Equals(UInt64.MaxValue);
                case EDataType.TypeFloat:
                    if (float.IsNaN((float)value)) return true;
                    return value.Equals(float.MaxValue);
            }

            return true;
        }

        /// <summary>
        /// 判断是否非法值
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        private static bool IsInvalidValue(EDataType type, object value)
        {
            switch (type)
            {
                case EDataType.TypeDouble:
                case EDataType.TypeByte:
                case EDataType.TypeInt8:
                case EDataType.TypeUInt8:
                case EDataType.TypeInt16:
                case EDataType.TypeUInt16:
                case EDataType.TypeInt32:
                case EDataType.TypeUInt32:
                case EDataType.TypeInt64:
                case EDataType.TypeUInt64:
                case EDataType.TypeFloat:
                    return IsInvalidDecValue(type, value);
            }

            return false;
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DateTimeToString(DateTime date, string format = "yyyy-MM-dd")
        {
            return date.ToString(format);
        }

        /// <summary>
        /// 财务指标只显示最近3个年报和最近一个季度
        /// </summary>
        /// <returns></returns>
        public static List<DateTime> GetFinanceDate()
        {
            DateTime today = DateTime.Today;

            List<DateTime> listDate = new List<DateTime>();
            listDate.Add(new DateTime(today.Year - 3, 12, 31));
            listDate.Add(new DateTime(today.Year - 2, 12, 31));
            listDate.Add(new DateTime(today.Year - 1, 12, 31));

            if (today.Month <= 3)
            {
                listDate.Add(new DateTime(today.Year - 1, 9, 30));
            }
            else if (today.Month <= 6)
            {
                listDate.Add(new DateTime(today.Year, 3, 31));
            }
            else if (today.Month <= 9)
            {
                listDate.Add(new DateTime(today.Year, 6, 30));
            }
            else
            {
                listDate.Add(new DateTime(today.Year, 9, 30));
            }

            listDate.Sort();
            return listDate;
        }
    }
}