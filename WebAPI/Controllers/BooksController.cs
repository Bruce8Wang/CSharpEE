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
    public class BooksController : ApiController
    {
        private BookDbContext _db;

        public BooksController(BookDbContext db)
        {
            _db = db;
        }

        [Route("api/Books")]
        [HttpGet]
        [Authorize]
        [LoggingFilter]
        public IQueryable<BookDTO> GetBooks()
        {
            var books = from b in _db.Books
                        select new BookDTO()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            AuthorName = b.Author.Name
                        };
            return books;
        }

        [Route("api/Books/{id}")]
        [HttpGet]
        [ResponseType(typeof(BookDetailDTO))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            var book = await _db.Books.Include(b => b.Author).Select(b =>
                new BookDetailDTO()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Price = b.Price,
                    AuthorName = b.Author.Name,
                    Genre = b.Genre
                }).SingleOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return this.Ok(book);
        }

        [Route("api/Books/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            _db.Entry(book).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/Books")]
        [HttpPost]
        [ResponseType(typeof(Book))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            // Load author name
            _db.Entry(book).Reference(x => x.Author).Load();

            var dto = new BookDTO()
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author.Name
            };

            return this.CreatedAtRoute("DefaultApi", new { id = book.Id }, dto);
        }

        [Route("api/Books/{id}")]
        [HttpDelete]
        [ResponseType(typeof(Book))]
        [Authorize]
        [LoggingFilter]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            Book book = await _db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            return this.Ok(book);
        }

        private bool BookExists(int id)
        {
            return _db.Books.Count(e => e.Id == id) > 0;
        }
    }
}
