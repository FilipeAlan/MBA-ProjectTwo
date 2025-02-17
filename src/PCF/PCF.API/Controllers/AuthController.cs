using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos;
using PCF.Core.Identity;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IUserRepository userRepository,ITokenGenerator tokenGenerator) : ApiControllerBase
    {
        [HttpPost("login")]
        public async Task<Results<Ok<string>,UnauthorizedHttpResult,StatusCodeHttpResult>> Login([FromBody] LoginResponse loginResponse) 
        {
            try
            {
                var user = await userRepository.FindByEmailAsync(loginResponse.Login);

                if (user != null && await userRepository.CheckPasswordAsync(user, loginResponse.Password))
                {
                    var token = tokenGenerator.GerarToken(user);
                    return TypedResults.Ok(token);
                }

                return TypedResults.Unauthorized();
            }
            catch
            {
                return TypedResults.StatusCode(500);
            }
        }
        [HttpPost("register")]
        public async Task<Results<Ok,Conflict,StatusCodeHttpResult>> Register([FromBody] LoginResponse loginResponse)
        {
            try
            {
                // Verifica se o usuário já existe
                var user = await userRepository.FindByEmailAsync(loginResponse.Login);

                if (user == null)
                {
                    // Cria um novo IdentityUser
                    var newUser = new ApplicationUser
                    {
                        UserName = loginResponse.Name,
                        Email = loginResponse.Login,
                    };

                    // Hash da senha
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    newUser.PasswordHash = passwordHasher.HashPassword(newUser, loginResponse.Password);

                    // Salva o usuário no banco
                    await userRepository.CreateAsync(newUser, loginResponse.Name!);

                    return TypedResults.Ok();
                }

                return TypedResults.Conflict();
            }
            catch
            {
                return TypedResults.StatusCode(500);
            }
        }
    }
}
