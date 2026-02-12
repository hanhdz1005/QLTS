using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Dtos
{
    public class RolesDto
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class AddRolesDto
    {
        public string Name { get; set; }
    }

    
}
