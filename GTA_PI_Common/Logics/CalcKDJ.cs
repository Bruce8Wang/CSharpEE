using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    public class CalcKDJ
    {
        /// <summary>
        /// 计算一个股票的MACD
        /// </summary>
        /// <param name="lstRet">计算数据</param>
        /// <param name="newIndex">需要计算记录的索引号</param>
        /// <param name="N">入参</param>
        /// <param name="M1">入参</param>
        /// <param name="M2">入参</param>
        public static void CalcOneStock(List<KDJHistory> lstRet, int newIndex, int N, int M1, int M2)
        {
            if (lstRet == null || lstRet.Count == 0) return;

            KDJHistory pre;
            if (newIndex == 0)//相当于全部重算
            {
                pre = lstRet[0];
                pre.LLV = pre.LOP;
                pre.HHV = pre.HIP;
                pre.RSV = (pre.CP - pre.LLV) / (pre.HHV - pre.LLV) * 100;
                pre.K = 100;
                pre.D = 100;
                pre.J = 100;

                newIndex += 1;
            }

            for (int i = newIndex; i < lstRet.Count; i++)
            {
                pre = lstRet[i - 1];

                int skip = i - N + 1;
                if (skip < 0) skip = 0;
                int take = N > i ? i + 1 : N;
                var lstTemp = lstRet.Skip(skip).Take(take);

                KDJHistory cur = lstRet[i];
                cur.LLV = lstTemp.Min(m => m.LOP);
                cur.HHV = lstTemp.Max(m => m.HIP);
                cur.RSV = (cur.CP - cur.LLV) / (cur.HHV - cur.LLV) * 100;

                if (double.NaN.Equals(cur.RSV))
                {
                    cur.RSV = pre.RSV;
                    cur.K = pre.K;
                    cur.D = pre.D;
                    cur.J = pre.J;
                }
                else
                {
                    cur.K = pre.K * (M1 - 1) / M1 + cur.RSV / M1;
                    cur.D = pre.D * (M2 - 1) / M2 + cur.K / M2;
                    cur.J = cur.K * 3 - cur.D * 2;
                }
                
            }

            CalcOneStockBool(lstRet);
        }

        /// <summary>
        /// 计算一个股票的 MACD金叉、MACD死叉、MACD顶背离、MACD底背离
        /// </summary>
        /// <param name="lstRet"></param>
        private static void CalcOneStockBool(List<KDJHistory> lstRet)
        {
            for (int i = 1; i < lstRet.Count; i++)
            {
                KDJHistory cur = lstRet[i];

                //超卖
                cur.Oversold = cur.D < 20;
                //超买
                cur.Overbought = cur.D > 80;

                if (i == 1) continue;
                KDJHistory pre1 = lstRet[i - 1];
                KDJHistory pre2 = lstRet[i - 2];

                if (pre1.K == pre1.D)
                {
                    //金叉
                    cur.GoldX = (pre2.K < pre2.D && cur.K > cur.D);
                    //死叉
                    cur.BlackX = (pre2.K > pre2.D && cur.K < cur.D);
                    //低位金叉
                    cur.LowGoldX = (pre1.K < 20 && pre1.D < 20 && cur.K > pre1.K && cur.D > pre1.D && pre2.K < pre2.D && cur.K > cur.D);
                }
                else
                {
                    //金叉
                    cur.GoldX = (pre1.K < pre1.D && cur.K > cur.D);
                    //死叉
                    cur.BlackX = (pre1.K > pre1.D && cur.K < cur.D);
                    //低位金叉
                    cur.LowGoldX = (pre1.K < 20 && pre1.D < 20 && cur.K > pre1.K && cur.D > pre1.D && pre1.K < pre1.D && cur.K > cur.D);
                }

                if (i <= 3) continue;
                KDJHistory pre3 = lstRet[i - 3];
                KDJHistory pre4 = lstRet[i - 4];

                //顶背离
                cur.TopDepart = (cur.CP > pre1.CP && pre1.CP > pre2.CP && pre2.CP > pre3.CP && pre3.CP > pre4.CP && cur.D < pre1.D && cur.D < pre2.D && cur.D < pre3.D && cur.D < pre4.D);
                //底背离
                cur.DownDepart = (cur.CP < pre1.CP && pre1.CP < pre2.CP && pre2.CP < pre3.CP && pre3.CP < pre4.CP && cur.D > pre1.D && cur.D > pre2.D && cur.D > pre3.D && cur.D > pre4.D);
            }
        }

        /// <summary>
        /// KDJHistory转KDJResult
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KDJResult> History2Result(List<KDJHistory> lstHistory)
        {
            var lstResult = from m in lstHistory.OrderBy(m => m.TradingDate)
                            group m by m.SecurityID into g
                            select new KDJResult()
                            {
                                SecurityID = g.Last().SecurityID,
                                TradingDate = g.Last().TradingDate,
                                Symbol = g.Last().Symbol,
                                K = g.Last().K,
                                D = g.Last().D,
                                J = g.Last().J,
                                GoldX = g.Last().GoldX,
                                BlackX = g.Last().BlackX,
                                LowGoldX = g.Last().LowGoldX,
                                TopDepart = g.Last().TopDepart,
                                DownDepart = g.Last().DownDepart,
                                Oversold = g.Last().Oversold,
                                Overbought = g.Last().Overbought
                            };

            return lstResult;
        }
    }
}
