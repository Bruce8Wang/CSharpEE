using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ITSM.Models;

namespace ITSM.Controllers
{
    public class StatusesController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //Get:../api/statuss
        public IQueryable<Status> GetStatuses()
        {
            return db.Statuses;
        }

        //Get:../api/statuss/1
        [ResponseType(typeof(Status))]
        public async Task<IHttpActionResult> GetStatus(int id)
        {
            Status status = await db.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        //PUT:../api/statuss/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStatus(int id, Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != status.Id)
            {
                return BadRequest();
            }

            db.Entry(status).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: ../api/Statuss
        [ResponseType(typeof(Status))]
        public async Task<IHttpActionResult> PostStatus(Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Statuses.Add(status);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = status.Id }, status);
        }

        // DELETE: ../api/Statuss/5
        [ResponseType(typeof(Status))]
        public async Task<IHttpActionResult> DeleteStatus(int id)
        {
            Status status = await db.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            db.Statuses.Remove(status);
            await db.SaveChangesAsync();

            return Ok(status);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool StatusExists(int id)
        {
            return db.Statuses.Count(e => e.Id == id) > 0;
        }
    }
}
