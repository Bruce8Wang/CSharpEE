using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncDataJob.Utility
{
    class LogHelper
    {
        private static ConcurrentQueue<string[]> lstLog;

        static LogHelper()
        {
            lstLog = new ConcurrentQueue<string[]>();
            Task.Factory.StartNew(WriteLog, TaskCreationOptions.LongRunning);
        }

        private static void WriteLog()
        {
            while(true)
            {
                string[] logs;
                while (lstLog.TryDequeue(out logs))
                {
                    File.AppendAllText(logs[0], logs[1]);
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 取得日志路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string GetFileName(string type)
        {
            string path = Path.Combine("Logs", type);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.Combine(path, DateTime.Today.ToString("yyyyMMdd") + ".log");
            return fileName;
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public static void Info(string type, string msg)
        {
            string line = string.Format("INFO：{0}  {1}", DateTime.Now, msg);
            string fileName = GetFileName(type);
            lstLog.Enqueue(new string[] { fileName, line + Environment.NewLine });
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public static void Error(string type, string msg)
        {
            string line = string.Format("ERROR：{0}  {1}", DateTime.Now, msg);
            string fileName = GetFileName(type);
            lstLog.Enqueue(new string[] { fileName, line + Environment.NewLine });
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ex"></param>
        public static void Error(string type, string msg, Exception ex)
        {
            string line = string.Format("ERROR：{0}", DateTime.Now);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line);
            if (string.IsNullOrEmpty(msg) == false)
            {
                sb.AppendLine("Error Record：" + msg);
            }
            sb.AppendLine("Message：" + ex.Message);
            sb.AppendLine("StackTrace：" + ex.StackTrace);
            sb.AppendLine();

            string fileName = GetFileName(type);
            lstLog.Enqueue(new string[] { fileName, sb.ToString() });
        }

        /// <summary>
        /// 轨迹日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public static void Track(string type, string msg)
        {
            string line = string.Format("TRACK：{0}  {1}", DateTime.Now, msg);
            string fileName = GetFileName(type);
            lstLog.Enqueue(new string[] { fileName, line + Environment.NewLine });
        }
    }
}
