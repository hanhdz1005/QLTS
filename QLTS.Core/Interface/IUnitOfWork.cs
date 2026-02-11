using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Interface
{
    public interface IUnitOfWork
    {
         public ICategoryRepository CategoryRepository { get; }
        public IAssetRepository AssetRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IMaintainRepository MaintainRepository { get; }
    }
}
