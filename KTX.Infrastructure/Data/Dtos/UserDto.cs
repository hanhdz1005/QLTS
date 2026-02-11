using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public string Role { get; set; }
    }

    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public string Role { get; set; }
    }

    

    public class AddressDto 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AppUserId { get; set; }

        public virtual AppUser appUser { get; set; }
    }
}
