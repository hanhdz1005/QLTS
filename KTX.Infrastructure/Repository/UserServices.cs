using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Repository
{
    public class UserServices : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserServices(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        
        public IQueryable<AppUser> GetAllUser()
        {
            return _userManager.Users.AsNoTracking();
        }

        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateUserRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.Role = role;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
