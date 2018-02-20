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
	public class FeedBacksController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        // GET: api/FeedBacks
        public IQueryable<FeedBack> GetFeedBacks()
        {
            return db.FeedBacks;
        }

        // GET: api/FeedBacks/5
        [ResponseType(typeof(FeedBack))]
        public async Task<IHttpActionResult> GetFeedBack(long id)
        {
            FeedBack feedBack = await db.FeedBacks.FindAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return Ok(feedBack);
        }

        // PUT: api/FeedBacks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeedBack(long id, FeedBack feedBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedBack.Id)
            {
                return BadRequest();
            }

            db.Entry(feedBack).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedBackExists(id))
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

        // POST: api/FeedBacks
        [ResponseType(typeof(FeedBack))]
        public async Task<IHttpActionResult> PostFeedBack(FeedBack feedBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeedBacks.Add(feedBack);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = feedBack.Id }, feedBack);
        }

        // DELETE: api/FeedBacks/5
        [ResponseType(typeof(FeedBack))]
        public async Task<IHttpActionResult> DeleteFeedBack(long id)
        {
            FeedBack feedBack = await db.FeedBacks.FindAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            db.FeedBacks.Remove(feedBack);
            await db.SaveChangesAsync();

            return Ok(feedBack);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedBackExists(long id)
        {
            return db.FeedBacks.Count(e => e.Id == id) > 0;
        }
    }
}