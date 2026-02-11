using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Entities
{
    public class Categories : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public virtual ICollection<Assets> Assets { get; set; }
        public virtual ICollection<Maintain> Maintain { get; set; }
    }

    

    
}
