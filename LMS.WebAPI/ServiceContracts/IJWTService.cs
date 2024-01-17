using LMS.WebAPI.DTO;
using LMS.WebAPI.Identity;

namespace LMS.WebAPI.ServiceContracts
{
    public interface IJWTService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}
