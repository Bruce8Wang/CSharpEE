using com.example.demo.Attributes;
using com.example.demo.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace com.example.demo.Controllers
{
    public class AuthorsController : ApiController
    {
        private BookDbContext _db;

        public AuthorsController(BookDbContext db)
        {
            _db = db;
        }

        [Route("api/Authors")]
        [HttpGet]
        [Authorize]
        [LoggingFilter]
        public IQueryable<Author> GetAuthors()
        {
            return _db.Authors;
        }

        [Route("api/Authors/{id}")]
        [HttpGet]
        [ResponseType(typeof(Author))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> GetAuthor(int id)
        {
            Author author = await _db.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [Route("api/Authors/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> PutAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.Id)
            {
                return BadRequest();
            }

            _db.Entry(author).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/Authors")]
        [HttpPost]
        [ResponseType(typeof(Author))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> PostAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Authors.Add(author);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        [Route("api/Authors/{id}")]
        [HttpDelete]
        [ResponseType(typeof(Author))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> DeleteAuthor(int id)
        {
            Author author = await _db.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();

            return Ok(author);
        }
        private bool AuthorExists(int id)
        {
            return _db.Authors.Count(e => e.Id == id) > 0;
        }
    }
}