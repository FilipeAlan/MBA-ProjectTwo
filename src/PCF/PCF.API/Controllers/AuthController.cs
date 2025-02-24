using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.Core.Dtos.Login;
using PCF.Core.Identity;
using PCF.Core.Interface;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IUserRepository userRepository, ITokenGenerator tokenGenerator) : ApiControllerBase
    {
        [HttpPost("login")]
        public async Task<Results<Ok<string>, UnauthorizedHttpResult, StatusCodeHttpResult>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var user = await userRepository.FindByEmailAsync(loginRequest.Login);

                if (user != null && await userRepository.CheckPasswordAsync(user, loginRequest.Password))
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
        public async Task<Results<Ok, Conflict, StatusCodeHttpResult>> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                // Verifica se o usuário já existe
                var user = await userRepository.FindByEmailAsync(registerRequest.Login);

                if (user == null)
                {
                    // Cria um novo IdentityUser
                    var newUser = new ApplicationUser
                    {
                        UserName = registerRequest.Name,
                        Email = registerRequest.Login,
                    };

                    // Hash da senha
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    newUser.PasswordHash = passwordHasher.HashPassword(newUser, registerRequest.Password);

                    // Salva o usuário no banco
                    await userRepository.CreateAsync(newUser, registerRequest.Name);

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