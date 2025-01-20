using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Entities;
using PCF.Core.Identity;
using PCF.Core.Interface;

namespace PCF.Core.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(PCFDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return Task.FromResult(result == PasswordVerificationResult.Success);
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName)
        {
            var result = await base.CreateAsync(entity);

            var appUser = new Usuario
            {
                Nome = userName,
                Id = entity.Id
            };

            await _dbContext.Usuarios.AddAsync(appUser);
            await _dbContext.CommitAsync();
            return result;
        }
    }
}