using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ITSM.Models;

namespace ITSM.Controllers
{
    public class SuperUsersController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //[EnableQuery]
        //Get:../api/superUsers
        public IQueryable<SuperUser> GetSuperUsers()
        {
            return db.SuperUsers;           
        }

        //Get:../api/superUsers/1
        [ResponseType(typeof(SuperUser))]
        public async Task<IHttpActionResult> GetSuperUser(int id)
        {
            SuperUser superUser = await db.SuperUsers.FindAsync(id);
            if (superUser == null)
            {
                return NotFound();
            }

            return Ok(superUser);
        }

        // POST: ../api/superUsers
        [ResponseType(typeof(SuperUser))]
        public async Task<IHttpActionResult> PostSuperUser(SuperUser superUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SuperUsers.Add(superUser);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = superUser.Id }, superUser);
        }

        // DELETE: ../api/superUsers/5
        [ResponseType(typeof(SuperUser))]
        public async Task<IHttpActionResult> DeleteSuperUser(int id)
        {
            SuperUser superUser = await db.SuperUsers.FindAsync(id);
            if (superUser == null)
            {
                return NotFound();
            }

            db.SuperUsers.Remove(superUser);
            await db.SaveChangesAsync();

            return Ok(superUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool SuperUserExists(int id)
        {
            return db.SuperUsers.Count(e => e.Id == id) > 0;
        }
    }
}