using System.Configuration;

namespace GTA.PI.API.Utility
{
    /// <summary>
    /// 配置帮助器
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 是否使用json静态数据
        /// </summary>
        public static bool IsUseStaticJson
        {
            get
            {
                var value = ConfigurationManager.AppSettings["IsUseStaticJson"];
                if (value == "1") return true;
                return false;
            }
        }
    }
}