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
    public class PrioritysController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //Get:../api/prioritys
        public IQueryable<Priority> GetSatisfactionLevels()
        {
            return db.Prioritys;
        }

        //Get:../api/prioritys/1
        [ResponseType(typeof(Priority))]
        public async Task<IHttpActionResult> GetPriority(int id)
        {
            Priority priority = await db.Prioritys.FindAsync(id);
            if (priority == null)
            {
                return NotFound();
            }

            return Ok(priority);
        }

        //PUT:../api/Prioritys/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPriority(int id, Priority priority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priority.Id)
            {
                return BadRequest();
            }

            db.Entry(priority).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriorityExists(id))
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

        // POST: ../api/Prioritys
        [ResponseType(typeof(Priority))]
        public async Task<IHttpActionResult> PostPriority(Priority priority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prioritys.Add(priority);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = priority.Id }, priority);
        }

        // DELETE: ../api/Prioritys/5
        [ResponseType(typeof(Priority))]
        public async Task<IHttpActionResult> DeletePriority(int id)
        {
            Priority priority = await db.Prioritys.FindAsync(id);
            if (priority == null)
            {
                return NotFound();
            }

            db.Prioritys.Remove(priority);
            await db.SaveChangesAsync();

            return Ok(priority);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool PriorityExists(int id)
        {
            return db.Prioritys.Count(e => e.Id == id) > 0;
        }
    }
}