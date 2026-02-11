using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Entities
{
    public class Employees : BaseEntity<Guid>
    {
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Guid DeptId { get; set; }
        public string DeptName { get; set; }
        public Departments Departments { get; set; }
        public ICollection<Maintain> Maintain { get; set; }
    }

    
}
