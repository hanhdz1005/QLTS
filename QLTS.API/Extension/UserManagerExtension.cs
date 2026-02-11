using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLTS.Core.Entities;
using System.Security.Claims;

namespace QLTS.API.Extension
{
    public static class UserManagerExtension
    {
        //public static async Task<AppUser> FindUserByClaimPrincipalWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        //{
        //    var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        //    return await userManager.Users.Include(x => x.address).SingleOrDefaultAsync();
        //}

        public static async Task<AppUser> FindEmailByClaimPrincipal(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.SingleOrDefaultAsync();
        }
    }
}
