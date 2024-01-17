using LMS.WebAPI.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LMS.WebAPI.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "PersonName can't be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage="Email is invalid")]
        [Remote(action: "IsEmailAreadyRegistered", controller:"Account",ErrorMessage ="Email is already registered")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password can't be blank")]
        [RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%&~])(?=.*?[A-Z]).{8,15}$",
        ErrorMessage = "The Password should contains atleast 1 number,Uppercase,Lowercase,Specialchar and length should be 8")]
        public string? Password { get; set; }
       
        [Required(ErrorMessage = "ConfirmPassword can't be blank")]
        [Compare("Password", ErrorMessage = "Password and confirm password are not matched")]
        public string? ConfirmPassword { get; set; }
    }
}
