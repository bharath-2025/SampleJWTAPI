using LMS.WebAPI.DTO;
using LMS.WebAPI.Identity;
using LMS.WebAPI.ServiceContracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.WebAPI.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        //Constructor Injection
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            //1: Get the ExpirationTime from ConfigurationFile
            //2: Prepare the claims that are needed to be added in the payload.
            //Note: A claim represents a particular values much like details / fields of a particuar user.
            //3: generate a SecretKey based on which, the signature can be generated. The Hashing fun req a secretKey


            //Step1: Generating the Expiration_Time based on EXPIRATION_MINUTES
            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

            //Step2: Creating the Claims for the Payload
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat , DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier , user.Email),
                new Claim(ClaimTypes.Name , user.PersonName)
            };

            //Step3: Creating the SecurityKey

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //Step4: Ready the Hasing Algorithm
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Step5: Creating the tokengenerator Object and make it ready to generate the token
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires:expiration,
                signingCredentials:signingCredentials
            );

            //Step6: Generate the Token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                PersonName = user.PersonName,
                ExpirationTime = expiration
            };

        }
    }
}
