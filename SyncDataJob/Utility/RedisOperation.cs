using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SyncDataJob.Utility
{
    /// <summary>
    /// Redis操作类
    /// </summary>
    public class RedisOperation
    {
        #region 单例
        /// <summary>
        /// 单例锁对象
        /// </summary>
        private static object lockObj = new object();

        /// <summary>
        /// 单例对象
        /// </summary>
        private static RedisOperation _instance = null;

        /// <summary>
        /// 单例对象
        /// </summary>
        public static RedisOperation Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisOperation();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// redis操作对象
        /// </summary>
        ConnectionMultiplexer mRedisSession;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RedisOperation()
        {
            string ip = ConfigurationManager.AppSettings["dsp_redis_ip"].Trim();
            string port = ConfigurationManager.AppSettings["dsp_redis_port"].Trim();
            string password = ConfigurationManager.AppSettings["dsp_redis_pw"].Trim();
            SetRedisConnect(ip, int.Parse(port), password);
        }

        /// <summary>
        /// 设置Redis连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        private void SetRedisConnect(string ip, int port, string password)
        {
            ConfigurationOptions fRedisConfig = new ConfigurationOptions()
            {
                EndPoints = { { ip, port } },
                Password = password,
                AllowAdmin = true
            };
            mRedisSession = ConnectionMultiplexer.Connect(fRedisConfig);
        }

        /// <summary>
        /// 单个取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否存在该值</returns>
        public byte[] GetRedisValue(string key)
        {
            IDatabase client = mRedisSession.GetDatabase();
            byte[] fSessionValue = (byte[])client.StringGet(key);
            return fSessionValue;
        }

        /// <summary>
        /// 批量取值
        /// </summary>
        /// <param name="keys"></param>
        public RedisValue[] GetRedisValue(List<string> keys)
        {
            //IDatabase client = mRedisSession.GetDatabase();
            //RedisKey[] allKey = new RedisKey[keys.Count];
            //for (int i = 0; i < keys.Count; i++)
            //{
            //    allKey[i] = keys[i];
            //}
            //int count = (keys.Count / 300) + 1;
            //for (int i = 1; i <= count; i++)
            //{
            //    RedisKey[] rKey = null;
            //    int value = (keys.Count - (i * 300));
            //    int forCount = 0;
            //    if (value >= 300)
            //    {
            //        rKey = new RedisKey[300];
            //        forCount = 300;
            //    }
            //    else
            //    {
            //        rKey = new RedisKey[value];
            //        forCount = value;
            //    }
            //    int index = ((i - 1) * 300);
            //    Array.Copy(allKey, index, rKey, 0, forCount);
            //    var fSessionValue = client.StringGet(rKey);
            //}
            try
            {
                IDatabase client = mRedisSession.GetDatabase();
                RedisKey[] rKey = new RedisKey[keys.Count];
                for (int i = 0; i < keys.Count; i++)
                {
                    rKey[i] = keys[i];
                }
                var fSessionValue = client.StringGet(rKey);
                return fSessionValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
