using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCF.API.Controllers.Base;
using PCF.API.Dto;
using PCF.Core.Interface;
using PCF.Core.Util;

namespace PCF.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ApiControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Realiza o login de um usuário.
        /// </summary>
        /// <param name="loginResponseDto">DTO contendo as credenciais do usuário.</param>
        /// <returns>Token JWT se o login for bem-sucedido, ou mensagem de erro.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginResponseDto loginResponseDto)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(loginResponseDto.Login);

                if (user != null && await _userRepository.CheckPasswordAsync(user, loginResponseDto.Password))
                {
                    var token = TokenGenerator.GerarToken(user);
                    return Ok(token);
                }

                return Unauthorized("Credenciais inválidas.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao realizar login: {ex.Message}");
            }
        }

        /// <summary>
        /// Realiza o registro de um novo usuário.
        /// </summary>
        /// <param name="loginResponseDto">DTO contendo os dados do usuário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginResponseDto loginResponseDto)
        {
            try
            {
                // Verifica se o usuário já existe
                var user = await _userRepository.FindByEmailAsync(loginResponseDto.Login);

                if (user == null)
                {
                    // Cria um novo IdentityUser
                    var newUser = new IdentityUser
                    {
                        UserName = loginResponseDto.Name,
                        Email = loginResponseDto.Login,
                    };

                    // Hash da senha
                    var passwordHasher = new PasswordHasher<IdentityUser>();
                    newUser.PasswordHash = passwordHasher.HashPassword(newUser, loginResponseDto.Password);

                    // Salva o usuário no banco
                    await _userRepository.CreateAsync(newUser);

                    return Ok("Usuário cadastrado com sucesso.");
                }

                return Conflict("Usuário já cadastrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao realizar registro: {ex.Message}");
            }
        }
    }
}
