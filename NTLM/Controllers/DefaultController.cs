using System.Web.Http;

namespace com.example.demo.Controllers
{
    public  class DefaultController:ApiController
    {
        [Route("api/Default")]
        [HttpGet]
        [Authorize]
        public string Home()
        {
            return User.Identity.Name;
        }
    }
}
