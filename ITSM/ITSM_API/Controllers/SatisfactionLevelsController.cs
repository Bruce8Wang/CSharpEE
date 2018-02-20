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
    public class SatisfactionLevelsController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //Get:../api/satisfactionLevels
        public IQueryable<SatisfactionLevel> GetSatisfactionLevels()
        {
            return db.SatisfactionLevels;
        }

        //Get:../api/satisfactionLevels/1
        [ResponseType(typeof(SatisfactionLevel))]
        public async Task<IHttpActionResult> GetSatisfactionLevel(int id)
        {
            SatisfactionLevel satisfactionLevel = await db.SatisfactionLevels.FindAsync(id);
            if (satisfactionLevel == null)
            {
                return NotFound();
            }

            return Ok(satisfactionLevel);
        }

        //PUT:../api/satisfactionLevels/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSatisfactionLevel(int id, SatisfactionLevel satisfactionLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != satisfactionLevel.Id)
            {
                return BadRequest();
            }

            db.Entry(satisfactionLevel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SatisfactionLevelExists(id))
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

        // POST: ../api/SatisfactionLevels
        [ResponseType(typeof(SatisfactionLevel))]
        public async Task<IHttpActionResult> PostSatisfactionLevel(SatisfactionLevel satisfactionLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SatisfactionLevels.Add(satisfactionLevel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = satisfactionLevel.Id }, satisfactionLevel);
        }

        // DELETE: ../api/SatisfactionLevels/5
        [ResponseType(typeof(SatisfactionLevel))]
        public async Task<IHttpActionResult> DeleteSatisfactionLevel(int id)
        {
            SatisfactionLevel satisfactionLevel = await db.SatisfactionLevels.FindAsync(id);
            if (satisfactionLevel == null)
            {
                return NotFound();
            }

            db.SatisfactionLevels.Remove(satisfactionLevel);
            await db.SaveChangesAsync();

            return Ok(satisfactionLevel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool SatisfactionLevelExists(int id)
        {
            return db.SatisfactionLevels.Count(e => e.Id == id) > 0;
        }
    }
}