using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Entities
{
    public class Maintain : BaseEntity<Guid>
    {
        public Guid AssetId { get; set; }
        public string AssetName { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid EmployeeId { get; set; }
        
        public string MaintainHost { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Description { get; set; }
        public decimal TotalCost { get; set; }

        public MaintainStatus Status { get; set; }
        public MaintainType Type { get; set; }
        public Assets Asset { get; set; }
        public Categories Category { get; set; }
        public Employees Employee { get; set; }

    }

    public enum MaintainStatus
    {
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }

    public enum MaintainType
    {
        Periodic = 1,
        Repair = 2,
        Emergency = 3
    }
}
