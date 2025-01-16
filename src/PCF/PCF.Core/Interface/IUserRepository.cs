using Microsoft.AspNetCore.Identity;

namespace PCF.Core.Interface
{
    public interface IUserRepository : IRepository<IdentityUser>
    {
        Task<IdentityUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
    }
}
