using System;
using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    public class CalcDMI
    {
        /// <summary>
        /// 计算一个股票的MACD
        /// </summary>
        /// <param name="lstRet">计算数据</param>
        /// <param name="newIndex">需要计算记录的索引号</param>
        /// <param name="N">入参</param>
        /// <param name="M">入参</param>
        public static void CalcOneStock(List<DMIHistory> lstRet, int newIndex, int N = 14, int M = 6)
        {
            if (lstRet == null || lstRet.Count == 0) return;

            DMIHistory pre;
            if (newIndex == 0)//相当于全部重算
            {
                pre = lstRet[0];
                newIndex += 1;
            }

            for (int i = newIndex; i < lstRet.Count; i++)
            {
                pre = lstRet[i - 1];
                DMIHistory cur = lstRet[i];
                cur.TR1 = Math.Max(Math.Max(cur.HIP - cur.LOP, Math.Abs(cur.HIP - pre.CP)), Math.Abs(cur.LOP - pre.CP));
                cur.HD = cur.HIP - pre.HIP;
                cur.LD = pre.LOP - cur.LOP;

                if (i < N) continue;

                int skip = i - N + 1;
                if (skip < 0) skip = 0;
                int take = N > i ? i + 1 : N;
                var lstTemp = lstRet.Skip(skip).Take(take);

                cur.TR = lstTemp.Sum(m => m.TR1);
                cur.DMP = lstTemp.Where(m => m.HD > 0 && m.HD > m.LD).Sum(m => m.HD);
                cur.DMM = lstTemp.Where(m => m.LD > 0 && m.LD > m.HD).Sum(m => m.LD);
                cur.PDI = cur.DMP * 100 / cur.TR;
                cur.MDI = cur.DMM * 100 / cur.TR;

                //消除非数值问题(把一般是除数是0引起的，把其值设置成其前一天的值)
                if (double.NaN.Equals(cur.PDI)) cur.PDI = pre.PDI;
                if (double.NaN.Equals(cur.MDI)) cur.MDI = pre.MDI;

                int take2 = M;
                if (i < N + take2 - 1) continue;
                if (cur.PDI == null || cur.MDI == null) continue;

                int skip2 = i - take2 + 1;
                var lstTemp2 = lstRet.Skip(skip2).Take(take2);
                cur.ADX = lstTemp2.Average(m => Math.Abs(m.MDI - m.PDI) / (m.MDI + m.PDI) * 100);

                //消除非数值问题(把一般是除数是0引起的，把其值设置成其前一天的值)
                if (double.NaN.Equals(cur.ADX)) cur.ADX = pre.ADX;

                int take3 = M;
                if (i < N + take2 + take3 - 2) continue;
                int skip3 = i - take3 + 1;
                DMIHistory preSkip3 = lstRet[skip3];
                cur.ADXR = (preSkip3.ADX + cur.ADX) / 2;

                //消除非数值问题(把一般是除数是0引起的，把其值设置成其前一天的值)
                if (double.NaN.Equals(cur.ADXR)) cur.ADXR = pre.ADXR;
            }

            CalcOneStockBool(lstRet);
        }

        /// <summary>
        /// 计算一个股票的 DMI金叉、DMI死叉
        /// </summary>
        /// <param name="lstRet"></param>
        private static void CalcOneStockBool(List<DMIHistory> lstRet)
        {
             for(int i = 2; i < lstRet.Count; i++)
             {
                 DMIHistory cur18 = lstRet[i];
                 DMIHistory pre17 = lstRet[i - 1];
                 DMIHistory pre16 = lstRet[i - 2];

                 if (pre17.PDI == pre17.MDI)
                 {
                     cur18.GoldX = (pre16.PDI < pre16.MDI && cur18.PDI > cur18.MDI);
                     cur18.BlackX = (pre16.PDI > pre16.MDI && cur18.PDI < cur18.MDI);
                 }
                 else
                 {
                     cur18.GoldX = (pre17.PDI < pre17.MDI && cur18.PDI > cur18.MDI);
                     cur18.BlackX = (pre17.PDI > pre17.MDI && cur18.PDI < cur18.MDI);
                 }
             }
        }

        /// <summary>
        /// DMIHistory转DMIResult
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DMIResult> History2Result(List<DMIHistory> lstHistory)
        {
            var lstResult = from m in lstHistory.OrderBy(m => m.TradingDate)
                            group m by m.SecurityID into g
                            select new DMIResult()
                            {
                                SecurityID = g.Last().SecurityID,
                                TradingDate = g.Last().TradingDate,
                                Symbol = g.Last().Symbol,
                                PDI = g.Last().PDI,
                                MDI = g.Last().MDI,
                                ADX = g.Last().ADX,
                                GoldX = g.Last().GoldX,
                                BlackX = g.Last().BlackX
                            };

            return lstResult;
        }
    }
}
