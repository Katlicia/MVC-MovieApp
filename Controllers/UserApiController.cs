using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly MvcMovieContext _context;

        public UsersApiController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: api/usersapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/usersapi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(int id)
        {
            var user = await _context.User
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Username,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/usersapi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/usersapi
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Check for existing user with the same username or email
            if (_context.User.Any(u => u.Username == user.Username || u.Email == user.Email))
            {
                return Conflict("A user with the same username or email already exists.");
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // POST: api/usersapi/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginModel loginModel)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Username == loginModel.Username && u.Password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return user;
        }

        // GET: api/usersapi/{id}/watched
        [HttpGet("{id}/watched")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetWatchedMovies(int id)
        {
            var watchedMovies = await _context.UserMovie
                .Where(um => um.UserId == id)
                .Include(um => um.Movie)
                .Select(um => um.Movie)
                .ToListAsync();

            return watchedMovies;
        }

        // POST: api/usersapi/{id}/watched/{filmId}
        [HttpPost("{id}/watched/{filmId}")]
        public async Task<IActionResult> MarkAsWatched(int id, int filmId)
        {
            var user = await _context.User.FindAsync(id);
            var movie = await _context.Movie.FindAsync(filmId);

            if (user == null || movie == null)
            {
                return NotFound();
            }

            // Kullanıcının bu filmi daha önce izleyip izlemediğini kontrol et
            var existingEntry = await _context.UserMovie
                .FirstOrDefaultAsync(um => um.UserId == id && um.MovieId == filmId);

            if (existingEntry == null)
            {
                var userMovie = new UserMovie
                {
                    UserId = id,
                    MovieId = filmId
                };

                _context.UserMovie.Add(userMovie);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }


        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
