using Microsoft.AspNetCore.Identity;
using PCF.Core.Identity;

namespace PCF.Core.Interface
{
    public interface ITokenGenerator
    {
        string GerarToken(ApplicationUser user);
    }
}