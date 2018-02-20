using System;
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

	public class IssueTracksController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        // GET: api/IssueTracks
        public IQueryable<IssueTrack> GetIssueTracks()
        {
            //try
            //{
            //    //PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", ".");
            //    PerformanceCounter cpuCounter = new PerformanceCounter();
            //    cpuCounter.CategoryName = "Processor";
            //    cpuCounter.CounterName = "% Processor Time";
            //    cpuCounter.InstanceName = "_Total";
            //    cpuCounter.MachineName = ".";

            //    string str = cpuCounter.NextValue() + "%";
            //}
            //catch (Exception ex)
            //{

            //}


            return db.IssueTracks;
        }

        // GET: api/IssueTracks/5
        [ResponseType(typeof(IssueTrack))]
        public async Task<IHttpActionResult> GetIssueTrack(long id)
        {
            IssueTrack issueTrack = await db.IssueTracks.FindAsync(id);
            if (issueTrack == null)
            {
                return NotFound();
            }

            return Ok(issueTrack);
        }

        // PUT: api/IssueTracks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIssueTrack(long id, IssueTrack issueTrack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != issueTrack.Id)
            {
                return BadRequest();
            }

            db.Entry(issueTrack).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueTrackExists(id))
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

        // POST: api/IssueTracks
        [ResponseType(typeof(IssueTrack))]
        public async Task<IHttpActionResult> PostIssueTrack(IssueTrack issueTrack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.IssueTracks.Add(issueTrack);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CreatedAtRoute("DefaultApi", new { id = issueTrack.Id }, issueTrack);
        }

        // DELETE: api/IssueTracks/5
        [ResponseType(typeof(IssueTrack))]
        public async Task<IHttpActionResult> DeleteIssueTrack(long id)
        {
            IssueTrack issueTrack = await db.IssueTracks.FindAsync(id);
            if (issueTrack == null)
            {
                return NotFound();
            }

            db.IssueTracks.Remove(issueTrack);
            await db.SaveChangesAsync();

            return Ok(issueTrack);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IssueTrackExists(long id)
        {
            return db.IssueTracks.Count(e => e.Id == id) > 0;
        }
    }
}