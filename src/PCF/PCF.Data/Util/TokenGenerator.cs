
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace PCF.Data.Util
{
    public static class TokenGenerator
    {
        private static readonly string chavePrivada = "ASDF2134RF2#@#$%g5342qfm23XYZ987@abcd"; // Secrets futuramente.

        public static string GerarToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),                
                new Claim(JwtRegisteredClaimNames.Email, user.Email),                
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),                                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chavePrivada));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracao = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiracao,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
