using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(15, MinimumLength = 3)]
        [Required]
        public string? Username { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<UserMovie>? UserMovies { get; set; }
    }
}
