using GTA.PI.API.Attributes;
using GTA.PI.API.Models;
using GTA.PI.API.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTA.PI.API.Controllers
{
    /// <summary>
    /// 与用户安全相关
    /// </summary>
    public class UserController : ApiController
    {
        private GTAPIContext db = new GTAPIContext();

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Account/Login")]
        [LoggingFilter]
        public async Task<IHttpActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            if (login.username != null && login.username != "") parameters.Add("username", login.username);
            if (login.password != null && login.password != "") parameters.Add("password", login.password);
            HttpContent content = new FormUrlEncodedContent(parameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync("http://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port + "/Token", content);

            return ResponseMessage(response);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="Register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Account/Register")]
        [LoggingFilter]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Register(RegisterViewModel Register)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //确认用户是否已存在
            var user = from u in db.Users
                       where u.UserName == Register.UserName
                       select u;
            if (user.Count() > 0) return BadRequest("用户名已存在");

            //实例化User对象
            string PasswordHash = GeneralHelper.HashPasswordForStoringInConfigFile(Register.Password, "MD5");
            User User = new User();
            User.UserName = Register.UserName;
            User.PasswordHash = PasswordHash;
            User.Email = Register.Email;

            //将User对象添加之数据库中
            db.Users.Add(User);
            await db.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="ChangePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Account/ChangePassword")]
        [LoggingFilter]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Manage(ChangePasswordViewModel ChangePassword)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //确认两次数据的密码是否一致
            if (ChangePassword.NewPassword != ChangePassword.ConfirmPassword) return BadRequest("两次输入的密码不一致");
            //通过账号和密码查询用户
            string PasswordHash = GeneralHelper.HashPasswordForStoringInConfigFile(ChangePassword.Password, "MD5");
            var user = (from u in db.Users
                        where u.UserName == ChangePassword.UserName && u.PasswordHash == PasswordHash
                        select u).FirstOrDefault();
            if (user == null) return BadRequest("用户名或密码不对");
            //更新用户密码
            user.PasswordHash = GeneralHelper.HashPasswordForStoringInConfigFile(ChangePassword.NewPassword, "MD5");
            await db.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}