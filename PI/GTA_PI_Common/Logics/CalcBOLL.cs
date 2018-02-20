using System;
using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    public class CalcBOLL
    {
        /// <summary>
        /// 计算一个股票的BOLL
        /// </summary>
        /// <param name="lstRet">计算数据</param>
        /// <param name="maParam">入参</param>
        public static void CalcOneStock(List<BOLLResult> lstRet, int maParam = 20)
        {
            for (int i = 0; i < lstRet.Count; i++)
            {
                int curIndex = maParam + i - 1;
                if (curIndex >= lstRet.Count) break;

                BOLLResult cur = lstRet[curIndex];

                var lstTemp = lstRet.Skip(i).Take(maParam);
                cur.MA = lstTemp.Average(m => m.CP);
                cur.MD = Math.Sqrt(lstTemp.Average(m => Math.Pow(m.CP - cur.MA, 2)));
                cur.MB = lstTemp.Skip(1).Average(m => m.CP);
                cur.UP = cur.MB + 2 * cur.MD;
                cur.DN = cur.MB - 2 * cur.MD;
            }

            CalcOneStockBool(lstRet);
        }

        /// <summary>
        /// 计算一个股票的 BOLL突破上轨、BOLL突破中轨
        /// </summary>
        /// <param name="lstRet"></param>
        private static void CalcOneStockBool(List<BOLLResult> lstRet)
        {
            for (int i = 2; i < lstRet.Count; i++)
            {
                BOLLResult cur = lstRet[i];
                BOLLResult pre1 = lstRet[i - 1];
                BOLLResult pre2 = lstRet[i - 2];

                //突破上轨
                if (pre1.CP == pre1.UP)
                {
                    cur.Upper = (pre2.CP < pre2.UP && cur.UP > cur.UP);
                }
                else
                {
                    cur.Upper = (pre1.CP < pre1.UP && cur.CP > cur.UP);
                }

                //突破中轨
                if(pre1.CP == pre1.MB)
                {
                    cur.Mid = (pre2.UP < pre2.MB && cur.CP > cur.MB);
                }
                else
                {
                    cur.Mid = (pre1.CP < pre1.MB && cur.CP > cur.MB);
                }
            }
        }

        /// <summary>
        /// 取每支股票最后的交易日的数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BOLLResult> GetLastResult(List<BOLLResult> lstAll)
        {
            var lstResult = from m in lstAll.OrderBy(m => m.TradingDate)
                            group m by m.SecurityID into g
                            select g.Last();

            return lstResult;
        }
    }
}
