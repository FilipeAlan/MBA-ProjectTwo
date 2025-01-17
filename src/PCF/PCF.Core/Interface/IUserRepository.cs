using Microsoft.AspNetCore.Identity;
using PCF.Core.Identity;

namespace PCF.Core.Interface
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName);
    }
}
