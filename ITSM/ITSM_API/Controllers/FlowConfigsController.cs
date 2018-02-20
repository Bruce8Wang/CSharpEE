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
    public class FlowConfigsController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        //[EnableQuery]
        //Get:../api/flowConfigs
        public IQueryable<FlowConfig> GetFlowConfigs()
        {
            return db.FlowConfigs;
        }

        //Get:../api/flowConfigs/1
        [ResponseType(typeof(FlowConfig))]
        public async Task<IHttpActionResult> GetFlowConfig(int id)
        {
            FlowConfig flowConfig = await db.FlowConfigs.FindAsync(id);
            if (flowConfig == null)
            {
                return NotFound();
            }

            return Ok(flowConfig);
        }

        //PUT:../api/flowConfigs/1
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFlowConfig(int id, FlowConfig flowConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flowConfig.Id)
            {
                return BadRequest();
            }

            db.Entry(flowConfig).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowConfigExists(id))
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

        // POST: ../api/FlowConfigs
        [ResponseType(typeof(FlowConfig))]
        public async Task<IHttpActionResult> PostFlowConfig(FlowConfig flowConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FlowConfigs.Add(flowConfig);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = flowConfig.Id }, flowConfig);
        }

        // DELETE: ../api/FlowConfigs/5
        [ResponseType(typeof(FlowConfig))]
        public async Task<IHttpActionResult> DeleteFlowConfig(int id)
        {
            FlowConfig flowConfig = await db.FlowConfigs.FindAsync(id);
            if (flowConfig == null)
            {
                return NotFound();
            }

            db.FlowConfigs.Remove(flowConfig);
            await db.SaveChangesAsync();

            return Ok(flowConfig);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool FlowConfigExists(int id)
        {
            return db.FlowConfigs.Count(e => e.Id == id) > 0;
        }


    }
}