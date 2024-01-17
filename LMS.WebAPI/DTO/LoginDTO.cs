using System.ComponentModel.DataAnnotations;

namespace LMS.WebAPI.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Email can't be blank")]
        [EmailAddress(ErrorMessage ="Enter a valid Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password can't be blank")]
        public string? Password { get; set; }
    }
}
