using Microsoft.EntityFrameworkCore;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Repository
{
    public class EmployeeRepository : GenericRepository<Employees>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Employees> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
