using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Employee Manager")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("Get_all_users")]
        public async Task<ActionResult> GetAllUser(
         [FromQuery] int pageIndex = 1,
         [FromQuery] int pageSize = 10,
         [FromQuery] string? search = null)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _userService
                .GetAllUser()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(u =>
                    u.DisplayName.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search)
                );
            }

            var totalUsers = await query.CountAsync();

            if (totalUsers == 0)
            {
                return NotFound("No users found.");
            }

            var users = await query
                .OrderBy(u => u.DisplayName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalUsers = totalUsers,
                TotalPage = (int)Math.Ceiling(totalUsers / (double)pageSize),
                Users = _mapper.Map<IReadOnlyList<GetUserDto>>(users)
            };

            return Ok(new
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = data
            });
        }

        [HttpGet("Get-user-by-id/{id}")]
        public async Task<ActionResult<GetUserDto>> GetUserById (string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var userDto = _mapper.Map<GetUserDto>(user);
            return Ok(new
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = userDto
            });
        }

        [HttpPut("Update-user-role/{id},{role}")]
        public async Task<IActionResult> UpdateUserRoleById (string id,string role )
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");

            if (role == null)
                return Ok(new
                {
                    Success = true,
                    Message = $"User role is still {id}"
                });

            var result = await _userService.UpdateUserRoleAsync(id, role);
            if (!result)
            {
                return BadRequest("Failed to update user role.");
            }
            var updatedUser = await _userService.GetUserByIdAsync(id);
            var data = _mapper.Map<GetUserDto>(updatedUser);
            return Ok(new
            {
                Success = true,
                Message = "User role updated successfully.",
                Data = data

            });
        }

        [HttpDelete("Delete-user-by-id/{id}")]
        public async Task<IActionResult> DeleteUserById (string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            var result = await _userService.DeleteUserByIdAsync(id);
            if (!result) return BadRequest("Failed to delete user.");
            return Ok(new
            {
                Success = true,
                Message = $"{user.DisplayName} has been deleted."
            });

        }
    }
}
