//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GTA.PI.Logics
//{
//    class CalcMACDNew
//    {
//        public static List<MACDResult> CalcMACDMain(List<MACDPara> list, int shortPara = 12, int longPara = 26, int midPara = 9)
//        {
//            var n = list.Count - 1;
//            double? ema12 = 0;
//            for (var i = 0; i < n - 1; i++)
//            {
//                ema12 += list[n - i].CP * 2 * Math.Pow((shortPara - 1), i) / Math.Pow((shortPara + 1), i + 1);
//            }
//            ema12 += list[0].CP * (Math.Pow((shortPara - 1) / (shortPara + 1), n - 1));
            
//            var results = new List<MACDResult>();
//            results.Add(new MACDResult() { EMA12 = ema12 });
//            return results;
//        }
//    }
//}
