using GTA.PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GTA.PI.Utility
{
    public class CommonUtility
    {
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

            return listDate;
        }
    }
}