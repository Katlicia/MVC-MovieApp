using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly MvcMovieContext _context;

        public FilmsController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: api/films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movie.ToListAsync();
        }

        // GET: api/films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // POST: api/films
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movie.Add(movie);
                await _context.SaveChangesAsync();

                // Return the newly created movie with a status code of 201 Created
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }

            // If the model state is invalid, return a BadRequest response
            return BadRequest(ModelState);
        }

        [HttpPost("{id}/watched")]
        public async Task<IActionResult> MarkFilmAsWatched(int id)
        {
            // Oturumdaki kullanıcı ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Kullanıcı oturum açmamışsa 401 döner
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound(); // Film bulunamazsa 404 döner
            }

            // Kullanıcının bu filmi daha önce izleyip izlemediğini kontrol et
            var existingEntry = await _context.UserMovie
                .FirstOrDefaultAsync(um => um.UserId == int.Parse(userId) && um.MovieId == id);

            if (existingEntry == null)
            {
                // Kullanıcının izlediği film kaydını oluştur
                var userMovie = new UserMovie
                {
                    UserId = int.Parse(userId),
                    MovieId = id
                };

                _context.UserMovie.Add(userMovie);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("User has already marked this film as watched.");
            }

            return Ok("Film has been marked as watched.");
        }

    }
}
