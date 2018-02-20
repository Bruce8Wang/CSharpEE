using PI.API.Attributes;
using System.Web.Http;

namespace PI.API.Controllers
{
    public class DefaultController : ApiController
    {
        /// <summary>
        /// Vesion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Version")]
        [LoggingFilter]
        public string GetVersion()
        {
            return "PI.API V1.0";
        }

        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/UserName")]
        [LoggingFilter]
        public string GetUserName()
        {
            return User.Identity.Name.Trim();
        }
    }
}
