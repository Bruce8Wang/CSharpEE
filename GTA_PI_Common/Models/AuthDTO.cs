using System;

namespace GTA.PI.Models
{
    public class AuthenticationToken
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationMark { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string ClientIp { get; set; }
        public string ClientMacAddress { get; set; }
        public UserStatus Status { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastActiveTime { get; set; }
        public DateTime ExpireTime { get; set; }
    }

    public class AuthToken
    {
        public AuthenticationToken result { get; set; }
        public UserStatus status { get; set; }

    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus : int
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 2,
        /// <summary>
        /// 未激活
        /// </summary>
        NotActived = 3,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 4
    }
}