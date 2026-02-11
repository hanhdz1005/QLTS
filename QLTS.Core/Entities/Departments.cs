using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Entities
{
    public class Departments : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();
    }

    
}
