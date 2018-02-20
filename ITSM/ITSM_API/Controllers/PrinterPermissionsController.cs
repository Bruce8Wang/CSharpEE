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
	public class PrinterPermissionsController : ApiController
    {
        private ITSMModel db = new ITSMModel();

        // GET: api/PrinterPermissions
        public IQueryable<PrinterPermissionDTO> GetPrinterPermissions()
        {
            var printerPermission = from b in db.PrinterPermissions
                                    select new PrinterPermissionDTO
                                    {
                                        Id = b.Id,
                                        No = b.No,
                                        Name = b.Name,
                                        Center = b.Center,
                                        Color = b.Color == true ? "有" : "无",
                                        FiveFloor = b.FiveFloor == true ? "有" : "无",
                                        SixFloor = b.SixFloor == true ? "有" : "无",
                                        SevenFloor = b.SevenFloor == true ? "有" : "无",
                                        EighthFloor = b.EighthFloor == true ? "有" : "无",
                                        ForthBamboo = b.ForthBamboo == true ? "有" : "无",
                                        SecondHill = b.SecondHill == true ? "有" : "无",
                                        ThirtyfifthSZStock = b.ThirtyfifthSZStock == true ? "有" : "无",
                                        Note = b.Note
                                    };
            return printerPermission;
        }

        // GET: api/PrinterPermissions/5
        [ResponseType(typeof(PrinterPermission))]
        public async Task<IHttpActionResult> GetPrinterPermission(long id)
        {
            PrinterPermission printerPermission = await db.PrinterPermissions.FindAsync(id);
            if (printerPermission == null)
            {
                return NotFound();
            }

            return Ok(printerPermission);
        }

        // PUT: api/PrinterPermissions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPrinterPermission(long id, PrinterPermission printerPermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != printerPermission.Id)
            {
                return BadRequest();
            }

            db.Entry(printerPermission).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrinterPermissionExists(id))
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

        // POST: api/PrinterPermissions
        [ResponseType(typeof(PrinterPermission))]
        public async Task<IHttpActionResult> PostPrinterPermission(PrinterPermission printerPermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //long m = 0;
            //long? max = (from t in db.PrinterPermissions
            //             select (long?)t.Id).Max();
            //if (!max.HasValue)
            //{
            //    m = 1;
            //}
            //else
            //    m = (long)max + 1;
            //printerPermission.Id = m;

            db.PrinterPermissions.Add(printerPermission);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }


            return CreatedAtRoute("DefaultApi", new { id = printerPermission.Id }, printerPermission);
        }

        // DELETE: api/PrinterPermissions/5
        [ResponseType(typeof(PrinterPermission))]
        public async Task<IHttpActionResult> DeletePrinterPermission(long id)
        {
            try
            {
                PrinterPermission printerPermission = await db.PrinterPermissions.FindAsync(id);
                if (printerPermission == null)
                {
                    return NotFound();
                }

                db.PrinterPermissions.Remove(printerPermission);
                await db.SaveChangesAsync();

                return Ok(printerPermission);
            }
            catch (Exception ex)
            {
                PrinterPermission printerPermission = new PrinterPermission();
                return Ok(printerPermission);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrinterPermissionExists(long id)
        {
            return db.PrinterPermissions.Count(e => e.Id == id) > 0;
        }
    }
}