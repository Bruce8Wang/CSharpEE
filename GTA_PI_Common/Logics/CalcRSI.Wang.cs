//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace GTA.PI.Logics
//{
//    public class CalcRSI
//    {
//        /// <summary>
//        /// 计算一个股票的RSI指标
//        /// </summary>
//        /// <param name="lstRet"></param>
//        /// <param name="para">参数范围为13~200，默认值为14</param>
//        public static void CalcOneStock(List<RSIHistory> lstRet, int para = 14)
//        {
//            ///设置默认值
//            for (int z = 0; z <= lstRet.Count - 1; z++)
//            {
//                lstRet[z].RSI6 = 100;
//                lstRet[z].RSI12 = 100;
//                lstRet[z].RSI = 100;
//                lstRet[z].RSIGoldX = false;
//                lstRet[z].RSIBlackX = false;
//            }

//            int i, j = 0;
//            double x = 0, y = 0;
//            for (i = 0; i <= lstRet.Count - 1; i++)
//            {
//                if (i >= 5 && i < 11)
//                {
//                    //开始计算RSI6
//                    x = 0; y = 0;
//                    for (j = i - 5; j <= i; j++)
//                    {
//                        if (lstRet[j].ChangeRatio > 0)
//                        { x = x + lstRet[j].ChangeRatio; }
//                        else
//                        { y = y + lstRet[j].ChangeRatio; }
//                    }
//                    lstRet[i].RSI6 = (x / 6) / (x / 6 + Math.Abs(y / 6)) * 100;
//                }
//                else if (i >= 11 && i < para - 1)
//                {
//                    //开始计算RSI6
//                    x = 0; y = 0;
//                    for (j = i - 5; j <= i; j++)
//                    {
//                        if (lstRet[j].ChangeRatio > 0)
//                        { x = x + lstRet[j].ChangeRatio; }
//                        else
//                        { y = y + lstRet[j].ChangeRatio; }
//                    }
//                    lstRet[i].RSI6 = (x / 6) / (x / 6 + Math.Abs(y / 6)) * 100;
//                    //开始计算RSI12
//                    x = 0; y = 0;
//                    for (j = i - 11; j <= i; j++)
//                    {
//                        if (lstRet[j].ChangeRatio > 0)
//                        { x = x + lstRet[j].ChangeRatio; }
//                        else
//                        { y = y + lstRet[j].ChangeRatio; }
//                    }
//                    lstRet[i].RSI12 = (x / 12) / (x / 12 + Math.Abs(y / 12)) * 100;
//                }
//                else if (i >= para - 1)
//                {
//                    //开始计算RSI6
//                    x = 0; y = 0;
//                    for (j = i - 5; j <= i; j++)
//                    {
//                        if (lstRet[j].ChangeRatio > 0)
//                        { x = x + lstRet[j].ChangeRatio; }
//                        else
//                        { y = y + lstRet[j].ChangeRatio; }
//                    }
//                    lstRet[i].RSI6 = (x / 6) / (x / 6 + Math.Abs(y / 6)) * 100;

//                    //开始计算RSI12
//                    x = 0; y = 0;
//                    for (j = i - 11; j <= i; j++)
//                    {
//                        if (lstRet[j].ChangeRatio > 0)
//                        { x = x + lstRet[j].ChangeRatio; }
//                        else
//                        { y = y + lstRet[j].ChangeRatio; }
//                    }
//                    lstRet[i].RSI12 = (x / 12) / (x / 12 + Math.Abs(y / 12)) * 100;

//                    //开始计算RSI
//                    x = 0; y = 0;
//                    for (j = i - 13; j <= i; j++)
//                    {
//                        if (lstRet[j].ChangeRatio > 0)
//                        { x = x + lstRet[j].ChangeRatio; }
//                        else
//                        { y = y + lstRet[j].ChangeRatio; }
//                    }
//                    lstRet[i].RSI = (x / para) / (x / para + Math.Abs(y / para)) * 100;
//                }
//            }

//            for (i = 0; i <= lstRet.Count - 1; i++)
//            {
//                if (i >= para - 1)
//                {
//                    lstRet[i].RSIGoldX = lstRet[i - 1].RSI6 == lstRet[i - 1].RSI12 ?
//                        lstRet[i - 2].RSI6 < lstRet[i - 2].RSI12 && lstRet[i].RSI6 > lstRet[i].RSI12 ? true : false :
//                        lstRet[i - 1].RSI6 < lstRet[i - 1].RSI12 && lstRet[i].RSI6 > lstRet[i].RSI12 ? true : false;

//                    lstRet[i].RSIBlackX = lstRet[i - 1].RSI6 == lstRet[i - 1].RSI12 ?
//                        lstRet[i - 2].RSI6 > lstRet[i - 2].RSI12 && lstRet[i].RSI6 < lstRet[i].RSI12 ? true : false :
//                        lstRet[i - 1].RSI6 > lstRet[i - 1].RSI12 && lstRet[i].RSI6 < lstRet[i].RSI12 ? true : false;
//                }
//            }
//        }

//        public static IEnumerable<RSIResult> History2Result(List<RSIHistory> lstHistory)
//        {
//            var lstResult = from m in lstHistory.OrderBy(m => m.TradingDate)
//                            group m by m.SecurityID into g
//                            select new RSIResult()
//                            {
//                                SecurityID = g.Last().SecurityID,
//                                TradingDate = g.Last().TradingDate,
//                                Symbol = g.Last().Symbol,
//                                RSI = g.Last().RSI,
//                                RSIGoldX = g.Last().RSIGoldX,
//                                RSIBlackX = g.Last().RSIBlackX
//                            };

//            return lstResult;
//        }
//    }
//}
