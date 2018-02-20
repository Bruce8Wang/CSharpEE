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
    public class FaultTypesController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //Get:../api/faultTypes
        public IQueryable<FaultTypeDTO> GetFaultTypes()
        {
            var faulttypes = from b in db.FaultTypes
                             select new FaultTypeDTO
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                                 Note = b.Note
                             };
            return faulttypes;
        }

        //Get:../api/faultTypes/1
        [ResponseType(typeof(FaultType))]
        public async Task<IHttpActionResult> GetFaultType(int id)
        {
            FaultType faultType = await db.FaultTypes.FindAsync(id);
            if (faultType == null)
            {
                return NotFound();
            }

            return Ok(faultType);
        }

        //PUT:../api/faultTypes/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFaultType(int id, FaultType faultType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != faultType.Id)
            {
                return BadRequest();
            }

            db.Entry(faultType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaultTypeExists(id))
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

        // POST: ../api/FaultTypes
        [ResponseType(typeof(FaultType))]
        public async Task<IHttpActionResult> PostFaultType(FaultType faultType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FaultTypes.Add(faultType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = faultType.Id }, faultType);
        }

        // DELETE: ../api/FaultTypes/5
        [ResponseType(typeof(FaultType))]
        public async Task<IHttpActionResult> DeleteFaultType(int id)
        {
            FaultType faultType = await db.FaultTypes.FindAsync(id);

            bool Exists = db.RepairApplyBills.Any(u => u.FaultTypeId == id);
            //if (Exists == true)

            if (faultType == null)
            {
                return NotFound();
            }


            db.FaultTypes.Remove(faultType);
            await db.SaveChangesAsync();

            return Ok(faultType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool FaultTypeExists(int id)
        {
            return db.FaultTypes.Count(e => e.Id == id) > 0;
        }
    }
}