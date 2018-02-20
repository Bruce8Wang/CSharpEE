using System.Collections.Generic;
using System.Web.Http;

namespace com.example.demo.Controllers
{
    public class DefaultController : ApiController
    {
        [Route("api/Default")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("api/Default/{id}")]
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/Default")]
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("api/Default/{id}")]
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route("api/Default/{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
