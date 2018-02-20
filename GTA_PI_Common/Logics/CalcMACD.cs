using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    public class CalcMACD
    {
        /// <summary>
        /// 计算一个股票的MACD
        /// </summary>
        /// <param name="lstRet">计算数据</param>
        /// <param name="newIndex">需要计算记录的索引号</param>
        /// <param name="shortParam">入参</param>
        /// <param name="longParam">入参</param>
        /// <param name="midParam">入参</param>
        public static void CalcOneStock(List<MACDHistory> lstRet, int newIndex, int shortParam, int longParam, int midParam)
        {
            if (lstRet == null || lstRet.Count == 0) return;

            MACDHistory pre;
            if (newIndex == 0)//相当于全部重算
            {
                pre = lstRet[0];
                pre.EMAshort = pre.CP;
                pre.EMAlong = pre.CP;

                newIndex += 1;
            }

            for (int i = newIndex; i < lstRet.Count; i++)
            {
                pre = lstRet[i - 1];

                MACDHistory cur = lstRet[i];
                CalcOneStockValue(cur, pre, shortParam, longParam, midParam);
            }

            CalcOneStockBool(lstRet);
        }

        /// <summary>
        /// 计算一个股票的 EMA（short）、EMA（long）、DIF、DEA、MACD
        /// </summary>
        /// <param name="raw">当天计算结果</param>
        /// <param name="pre">前一天计算结果</param>
        /// <param name="shortParam">入参</param>
        /// <param name="longParam">入参</param>
        /// <param name="midParam">入参</param>
        private static void CalcOneStockValue(MACDHistory cur, MACDHistory pre, int shortParam = 12, int longParam = 26, int midParam = 9)
        {
            cur.EMAshort = pre.EMAshort * (shortParam - 1) / (shortParam + 1) + cur.CP * 2 / (shortParam + 1);
            cur.EMAlong = pre.EMAlong * (longParam - 1) / (longParam + 1) + cur.CP * 2 / (longParam + 1);
            cur.DIF = cur.EMAshort - cur.EMAlong;
            cur.DEA = pre.DEA * (midParam - 1) / (midParam + 1) + cur.DIF * 2 / (midParam + 1);
            cur.MACD = 2 * (cur.DIF - cur.DEA);
        }

        /// <summary>
        /// 计算一个股票的 MACD金叉、MACD死叉、MACD顶背离、MACD底背离
        /// </summary>
        /// <param name="lstRet"></param>
        private static void CalcOneStockBool(List<MACDHistory> lstRet)
        {
            for(int i = 3; i < lstRet.Count; i++)
            {
                MACDHistory cur = lstRet[i];
                MACDHistory pre1 = lstRet[i - 1];
                MACDHistory pre2 = lstRet[i - 2];

                if (pre1.DIF == pre1.DEA)
                {
                    cur.GoldX = (pre2.DIF < pre2.DEA && cur.DIF > cur.DEA);
                    cur.BlackX = (pre2.DIF > pre2.DEA && cur.DIF < cur.DEA);
                }
                else
                {
                    cur.GoldX = (pre1.DIF < pre1.DEA && cur.DIF > cur.DEA);
                    cur.BlackX = (pre1.DIF > pre1.DEA && cur.DIF < cur.DEA);
                }

                if (i == 3) continue;

                MACDHistory pre3 = lstRet[i - 3];
                MACDHistory pre4 = lstRet[i - 4];

                cur.TopDepart = (pre4.CP < pre3.CP && pre3.CP < pre2.CP && pre2.CP < pre1.CP && pre1.CP < cur.CP &&
                                  cur.MACD < pre1.MACD && cur.MACD < pre2.MACD && cur.MACD < pre3.MACD && cur.MACD < pre4.MACD);

                cur.DownDepart = (cur.CP < pre1.CP && pre1.CP < pre2.CP && pre2.CP < pre3.CP && pre3.CP < pre4.CP &&
                                   cur.MACD > pre1.MACD && cur.MACD > pre2.MACD && cur.MACD > pre3.MACD && cur.MACD > pre4.MACD);
            }
        }

        /// <summary>
        /// MACDHistory转MACDResult
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MACDResult> History2Result(List<MACDHistory> lstHistory)
        {
            var lstResult = from m in lstHistory.OrderBy(m => m.TradingDate)
                            group m by m.SecurityID into g
                            select new MACDResult()
                            {
                                SecurityID = g.Last().SecurityID,
                                TradingDate = g.Last().TradingDate,
                                Symbol = g.Last().Symbol,
                                DIF = g.Last().DIF,
                                DEA = g.Last().DEA,
                                MACD = g.Last().MACD,
                                GoldX = g.Last().GoldX,
                                BlackX = g.Last().BlackX,
                                TopDepart = g.Last().TopDepart,
                                DownDepart = g.Last().DownDepart
                            };

            return lstResult;
        }
    }
}
