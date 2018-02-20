using GTA.Dsp.Client.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncDataJob.Utility
{
    public class SyncRequestEx : IDisposable
    {
        #region private

        private uint m_WaitTime = 900;//保证足够长，因为SDK会设置超时等待时间，必定有消息返回

        private static SyncRequestEx _instance;
        private static object Lock = new object();
        public static SyncRequestEx Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SyncRequestEx();
                        }
                    }
                }
                return _instance;
            }
        }

        public SyncRequestEx()
        {
            Connect();
        }

        private ConcurrentDictionary<ushort, object> dicReceivedData = new ConcurrentDictionary<ushort, object>();
        private List<ushort> lstReqNumber = new List<ushort>();

        private void Connect()
        {
            GatewayAdapter.Instance.OnRequestResponse += Instance_ReceiveData;
        }

        private void Instance_ReceiveData(object sender, RequestEventArgs e)
        {
            if (lstReqNumber.Contains(e.SeqNumId))
            {
                dicReceivedData[e.SeqNumId] = e.Data;
            }
        }

        private object GetValue(ushort reqNum)
        {
            DateTime dateTime = DateTime.Now;

            while ((DateTime.Now - dateTime).TotalMilliseconds < m_WaitTime * 1000)
            {
                if (dicReceivedData.ContainsKey(reqNum))
                {
                    object value;
                    while (dicReceivedData.TryRemove(reqNum, out value) == false)
                    {
                        Thread.Sleep(100);
                    }

                    return value;
                }

                Thread.Sleep(100);
            }
            return null;
        }

        void IDisposable.Dispose()
        {
            GatewayAdapter.Instance.OnRequestResponse -= Instance_ReceiveData;
        }

        #endregion

        /// <summary>
        /// 同步请求
        /// </summary>
        /// <param name="data">要发送的对象</param>
        /// <returns>返回的结果对象</returns>
        public object SyncSendData(object data, int timeout)
        {
            ushort reqNum = ushort.MaxValue;
            try
            {
                int ThreadID = Thread.CurrentThread.ManagedThreadId;
                var task = Task.Factory.StartNew(() =>
                {
                    reqNum = GatewayAdapter.Instance.Request(data, timeout);

                    lock (Lock)
                    {
                        lstReqNumber.Add(reqNum);
                    }

                    return GetValue(reqNum);
                });
                bool completed = false;
                completed = Task.WaitAll(new Task[] { task }, (int)m_WaitTime * 1000 * 2);//估算一个异常发送完成所需时间和GetValue()等待时间的和
                if (completed)
                {
                    ////添加了GTA.Dsp.Client.Client路径（为DSP V1.1 版本）
                    //if (task.Result is StationException)
                    //{
                    //    StationException ex = task.Result as StationException;
                    //    throw ex;
                    //}
                    //else
                    //{
                    //    return task.Result;
                    //}
                    return task.Result;
                }
                else
                {
                    DSPErrorCodeNew.SetException(DSPErrorCodeNew.DSP_RESULT_RECV_DATA_TIMEOUT);//特列
                    return null;
                    //throw new Exception("发送超时！");
                }
            }
            finally
            {
                lstReqNumber.Remove(reqNum);
            }
        }
    }
}
