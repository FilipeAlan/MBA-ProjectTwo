using IdentityModel;
using Microsoft.IdentityModel.JsonWebTokens;
using PCF.Core.Interface;
using System.Security.Claims;

namespace PCF.API.Services
{
    public class AppIdentityUser : IAppIdentityUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppIdentityUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public int GetUserId()
        {
            if (!IsAuthenticated()) return default;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claim))
                claim = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return claim is null ? default : int.Parse(claim);
        }

        public string GetUsername()
        {
            var username = _accessor.HttpContext?.User.FindFirst("username")?.Value;
            if (!string.IsNullOrEmpty(username)) return username;

            username = _accessor.HttpContext?.User.Identity?.Name;
            if (!string.IsNullOrEmpty(username)) return username;

            username = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(username)) return username;

            username = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.GivenName)?.Value;
            if (!string.IsNullOrEmpty(username)) return username;

            var sub = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.Subject);
            if (sub != null) return sub.Value;

            return string.Empty;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext != null && _accessor.HttpContext.User.IsInRole(role);
        }

        public bool IsAdmin()
        {
            return IsInRole("Admin");
        }

        public string? GetLocalIpAddress()
        {
            return _accessor.HttpContext?.Connection.LocalIpAddress?.ToString();
        }

        public string? GetRemoteIpAddress()
        {
            return _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
    }
}