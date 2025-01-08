
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCF.Data.Context;
using PCF.Data.Interface;

namespace PCF.Data.Repository
{
    public class UserRepository : Repository<IdentityUser>, IUserRepository
    {
        public UserRepository(PCFDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return Task.FromResult(result == PasswordVerificationResult.Success);
        }
    }
}
