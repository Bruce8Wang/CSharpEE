using GTA.PI.API.Attributes;
using System.Web.Http;

namespace GTA.PI.API.Controllers
{
    /// <summary>
    /// 仅做测试之用
    /// </summary>
    public class DefaultController : ApiController
    {
        /// <summary>
        /// 获取API版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Default/Version")]
        [LoggingFilter]
        public string GetVersion()
        {
            return "GTA.PI.API V1.0";
        }

        /// <summary>
        /// 获取当前用户账号
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("Default/UserName")]
        [LoggingFilter]
        public string GetUserName()
        {
            return User.Identity.Name.Trim();
        }
    }
}
