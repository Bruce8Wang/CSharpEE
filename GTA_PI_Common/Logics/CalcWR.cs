using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    public class CalcWR
    {
        /// <summary>
        /// 计算一个股票的WR
        /// </summary>
        /// <param name="lstRet">计算数据</param>
        /// <param name="nParam">入参</param>
        public static List<WRResult> CalcOneStock(List<WRResult> lstRet, int nParam = 14)
        {
            List<WRResult> lstResult = new List<WRResult>();

            for (int i = 0; i < lstRet.Count; i++)
            {
                int curIndex = nParam + i - 1;
                if (curIndex >= lstRet.Count) break;

                WRResult cur = lstRet[curIndex];
                lstResult.Add(cur);

                var lstTemp = lstRet.Skip(i).Take(nParam);
                cur.Hn = lstTemp.Max(m => m.HIP);
                cur.Ln = lstTemp.Min(m => m.LOP);
                cur.WR = (cur.Hn - cur.CP) / (cur.Hn - cur.Ln) * 100;
            }

            return lstResult;
        }

        /// <summary>
        /// 取每支股票最后的交易日的数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WRResult> GetLastResult(List<WRResult> lstAll)
        {
            var lstResult = from m in lstAll.OrderBy(m => m.TradingDate)
                            where double.NaN.Equals(m.WR) == false
                            group m by m.SecurityID into g
                            select g.Last();

            return lstResult;
        }
    }
}
