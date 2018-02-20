using GTA.Dsp.Client.Client;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SyncDataJob.Utility
{
    public class DspRequest : IDisposable
    {
        public delegate object _DealData(object data, object extra);
        public _DealData DealData;

        private ConcurrentDictionary<ushort, object> dicReqExtra = new ConcurrentDictionary<ushort, object>();
        private int _MaxReqTime = 10;

        public DspRequest()
        {
            GatewayAdapter.Instance.OnRequestResponse += Instance_ReceiveData;
        }

        public void Dispose()
        {
            GatewayAdapter.Instance.OnRequestResponse -= Instance_ReceiveData;
        }

        /// <summary>
        /// 等待任务结束
        /// </summary>
        /// <returns></returns>
        public void WaitFinished()
        {
            while (dicReqExtra.Count > 0)
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        private void DealDataMethod(object data, object extra)
        {
            if (DealData != null)
            {
                DealData(data, extra);
            }
        }

        /// <summary>
        /// 从DSP接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Instance_ReceiveData(object sender, RequestEventArgs e)
        {
            if (dicReqExtra.ContainsKey(e.SeqNumId))
            {
                object extra;
                while (dicReqExtra.TryRemove(e.SeqNumId, out extra) == false)
                {
                    Thread.Sleep(100);
                }

                var securityID = extra;
                //Task.Factory.StartNew(() => { DealDataMethod(e.Data, securityID); });
                DealDataMethod(e.Data, securityID);
            }
        }

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="data">要发送的对象</param>
        /// <returns>请求编码</returns>
        public int AsyncSendData(object data, int timeout, object extra = null)
        {
            while (dicReqExtra.Count >= _MaxReqTime)
            {
                Thread.Sleep(300);
            }

            ushort reqNum = GatewayAdapter.Instance.Request(data, timeout);
            dicReqExtra[reqNum] = extra;

            //Thread.Sleep(100);//预防前_MaxReqTime个请求发送太频繁，DSP host不住
            return reqNum;
        }
    }
}
