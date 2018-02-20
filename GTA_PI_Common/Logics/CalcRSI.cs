using System;
using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    public class CalcRSI
    {
        /// <summary>
        /// 计算一个股票的RSI
        /// </summary>
        /// <param name="lstRet">计算数据</param>
        /// <param name="param">入参</param>
        public static void CalcOneStock(List<RSIResult> lstRet, int param = 14)
        {
            for (int i = 0; i < lstRet.Count; i++)
            {
                RSIResult cur = lstRet[i];

                if (i < 5)
                {
                    cur.RSI6 = 100;
                }
                else
                {
                    var lst6 = lstRet.Skip(i - 5).Take(6).ToList();
                    cur.RSI6 = CalcRsi(lst6);
                }

                if (i < 11)
                {
                    cur.RSI12 = 100;
                }
                else
                {
                    var lst12 = lstRet.Skip(i - 11).Take(12).ToList();
                    cur.RSI12 = CalcRsi(lst12);
                }

                if (i < param - 1)
                {
                    cur.RSI = 100;
                }
                else
                {
                    var lst = lstRet.Skip(i - param + 1).Take(param).ToList();
                    cur.RSI = CalcRsi(lst);
                }
            }

            CalcOneStockBool(lstRet, param);
        }

        /// <summary>
        /// 技术RSI值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static double CalcRsi(List<RSIResult> list)
        {
            double up = list.Where(m => m.ChangeRatio > 0).Select(m => m.ChangeRatio).Sum() / list.Count;
            double down = Math.Abs(list.Where(m => m.ChangeRatio < 0).Select(m => m.ChangeRatio).Sum() / list.Count);
            double value = up / (up + down) * 100;
            return value;
        }

        /// <summary>
        /// 计算一个股票的 BOLL突破上轨、BOLL突破中轨
        /// </summary>
        /// <param name="lstRet"></param>
        private static void CalcOneStockBool(List<RSIResult> lstRet, int param)
        {
            for (int i = 2; i < lstRet.Count; i++)
            {
                if (i < param - 1) continue;

                RSIResult cur16 = lstRet[i];
                RSIResult pre15 = lstRet[i - 1];
                RSIResult pre14 = lstRet[i - 2];

                //RSI金叉
                if (pre15.RSI == pre15.RSI6)
                {
                    cur16.RSIGoldX = (pre14.RSI6 < pre14.RSI12 && cur16.RSI6 > cur16.RSI12);
                }
                else
                {
                    cur16.RSIGoldX = (pre15.RSI6 < pre15.RSI12 && cur16.RSI6 > cur16.RSI12);
                }

                //RSI死叉
                if (pre15.RSI6 == pre15.RSI12)
                {
                    cur16.RSIBlackX = (pre14.RSI6 > pre14.RSI12 && cur16.RSI6 < cur16.RSI12);
                }
                else
                {
                    cur16.RSIBlackX = (pre15.RSI6 > pre15.RSI12 && cur16.RSI6 < cur16.RSI12);
                }
            }
        }

        /// <summary>
        /// 取每支股票最后的交易日的数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<RSIResult> GetLastResult(List<RSIResult> lstAll)
        {
            var lstResult = from m in lstAll.OrderBy(m => m.TradingDate)
                            group m by m.SecurityID into g
                            select g.Last();

            return lstResult;
        }
    }
}
