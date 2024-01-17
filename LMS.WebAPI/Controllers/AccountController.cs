using LMS.WebAPI.DTO;
using LMS.WebAPI.Identity;
using LMS.WebAPI.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]  // Allow the unauthorized user to access this controller
    public class AccountController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> _userManager;  //userManager is used to manage the users such as searching, deleting,updating,adding users etc..
        public readonly SignInManager<ApplicationUser> _signInManager; //SignInmanager to signin and signUp the user
        public readonly RoleManager<ApplicationRole> _roleManager; //RoleManger to manipulate the roles such as deleting and creating the roles
        private readonly IJWTService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole> roleManager,IJWTService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        #region UserRegistration
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
            
            //Validation
            if(ModelState.IsValid == false)
            {
                string errorMessages = string.Join("\n",ModelState.Values.SelectMany(item => item.Errors).Select(err => err.ErrorMessage));
                return Problem(errorMessages);
            }

            //Create User
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password == null ? "" : registerDTO.Password);

            if(result.Succeeded)
            {
                //SignIn the User
                await _signInManager.SignInAsync(user, isPersistent: false);

                //Generating the JWT Token
                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);

                return Ok(authenticationResponse);
            }

            //If User is Not registered Successfully
            string errorMessage = string.Join("\n", result.Errors.Select(err => err.Description));

            return Problem(errorMessage);
            
        }
        #endregion

        #region IsEmailAlreadyRegistered
        [HttpGet]
        public async Task<IActionResult> IsEmailAreadyRegistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Ok(true);
            }

            return Ok(false);
        }
        #endregion

        #region Login

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> PostLogin(LoginDTO loginDTO)
        {
            //validation
            if(ModelState.IsValid == false)
            {
                string errorMessages = string.Join("\n",ModelState.Values.SelectMany(item => item.Errors).Select(err => err.ErrorMessage));
                return Problem(errorMessages);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email,loginDTO.Password,isPersistent:false,lockoutOnFailure:false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if(user == null)
                {
                    return NoContent();
                }

                //SignIn the User
                await _signInManager.SignInAsync(user, isPersistent: false);

                //Generating the JWT Token
                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);

                return Ok(authenticationResponse);
            }

            //If It dosent signin

            return Problem("Invalid Email or Password");
        }

        #endregion

        #region Logout

        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpGet("demo")]
        public ActionResult<string> GetDemo()
        {
            return Ok("Just a Demo");
        }

        #endregion

    }
}
