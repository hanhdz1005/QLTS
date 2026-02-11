using QLTS.Core.Entities;
using QLTS.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Repository
{
    public class MaintainRepository : GenericRepository<Maintain>, IMaintainRepository
    {
        public MaintainRepository(AppDbContext context) : base(context)
        {
        }
    }
}
