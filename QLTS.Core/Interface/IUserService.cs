using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Interface
{
    public interface IUserService
    {
        IQueryable<AppUser> GetAllUser();
        Task<AppUser?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserRoleAsync(string id, string role);
        Task<bool> DeleteUserByIdAsync(string id);
    }
}
