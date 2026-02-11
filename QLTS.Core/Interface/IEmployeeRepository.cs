using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Interface
{
    public interface IEmployeeRepository : IGenericRepository<Employees>
    {
        Task<Employees> GetByEmailAsync(string email);
    }
}
