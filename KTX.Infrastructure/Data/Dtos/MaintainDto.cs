using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Dtos
{
    public class MaintainDto
    {
        public Guid Id { get; set; }
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
    }

    public class GetMaintainDto
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
    }

    public class CreateMaintainDto
    {
        public Guid AssetId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public string Description { get; set; }
        //public decimal TotalCost { get; set; }
        public MaintainStatus Status { get; set; }
        public MaintainType Type { get; set; }
    }

    public class UpdateMaintainDto
    {
        public Guid AssetId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public MaintainStatus Status { get; set; }
        public MaintainType Type { get; set; }
    }
}
