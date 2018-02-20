using System.Collections.Generic;
using System.Linq;

namespace GTA.PI.Logics
{
    /// <summary>
    /// 它是将某一段时间的收盘价之和除以该周期。 比如日线MA5指5天内的收盘价除以5。
    /// 入参为MA（N）
    /// 若天数不足，MA（N）为空，如只上线4天，计算MA（5），值为空
    /// </summary>
    public class CalcMA
    {
        /// <summary>
        /// 一只股票移动平均线(MA)
        /// </summary>
        /// <param name="lstRaw">原始数据</param>
        /// <param name="maParam">周期</param>
        public static List<MAResult> CalcOneStock(List<MARawData> lstRaw, int maParam)
        {
            List<MAResult> lstResult = new List<MAResult>();

            int times = lstRaw.Count() - maParam;
            for (int i = 0; i <= times; i++)
            {
                var lstTemp = lstRaw.Skip(i).Take(maParam).ToList();
                double value = lstTemp.Average(m => m.CP);

                MARawData raw = lstTemp[lstTemp.Count - 1];
                MAResult result = new MAResult();
                lstResult.Add(result);

                result.SecurityID = raw.SecurityID;
                result.TradingDate = raw.TradingDate;
                result.Symbol = raw.Symbol;
                result.MA = value;
            }

            return lstResult;
        }
    }
}
