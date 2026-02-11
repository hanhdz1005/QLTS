using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Dtos
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid DeptId { get; set; }
        public string DeptName { get; set; }
    }
    public class AddEmployeeDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid DeptId { get; set; }
    }

    
}
