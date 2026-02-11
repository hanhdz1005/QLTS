using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Entities
{
    public class Assets : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Categories Category { get; set; }
        public AssetStatus Status { get; set; }
        public ICollection<Maintain> Maintain { get; set; }
    }

    public enum AssetStatus
    {
        Available = 1,
        InUse = 2,
        Broken = 3,
        Maintenance = 4,
        Disposed = 5
    }

    
}
