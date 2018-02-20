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
    public class DealMethodsController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //Get:../api/dealmethods
        public IQueryable<DealMethod> GetDealMethods()
        {
            return db.DealMethods;           
        }

        //Get:../api/dealmethods/1
        [ResponseType(typeof(DealMethod))]
        public async Task<IHttpActionResult> GetDealMethod(int id)
        {
            DealMethod dealMethod = await db.DealMethods.FindAsync(id);
            if (dealMethod == null)
            {
                return NotFound();
            }

            return Ok(dealMethod);
        }

        //PUT:../api/dealmethods/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDealMethod(int id, DealMethod dealMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dealMethod.Id)
            {
                return BadRequest();
            }

            db.Entry(dealMethod).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealMethodExists(id))
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

        // POST: ../api/DealMethods
        [ResponseType(typeof(DealMethod))]
        public async Task<IHttpActionResult> PostDealMethod(DealMethod dealMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DealMethods.Add(dealMethod);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dealMethod.Id }, dealMethod);
        }

        // DELETE: ../api/DealMethods/5
        [ResponseType(typeof(DealMethod))]
        public async Task<IHttpActionResult> DeleteDealMethod(int id)
        {
            DealMethod dealMethod = await db.DealMethods.FindAsync(id);
            if (dealMethod == null)
            {
                return NotFound();
            }

            db.DealMethods.Remove(dealMethod);
            await db.SaveChangesAsync();

            return Ok(dealMethod);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool DealMethodExists(int id)
        {
            return db.DealMethods.Count(e => e.Id == id) > 0;
        }
    }
}