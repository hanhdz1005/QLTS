using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTS.Core.Entities;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRole(
         [FromQuery] int pageIndex = 1,
         [FromQuery] int pageSize = 10,
         [FromQuery] string? search = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 10) pageSize = 5;

            var query = _roleManager.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(search));
            }

            var totalRoles = await query.CountAsync();

            if (totalRoles == 0)
            {
                return Ok(new
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRoles = totalRoles,
                    TotalPage = (int)Math.Ceiling(totalRoles / (double)pageSize),
                });
            }

            var users = await query
                .OrderBy(u => u.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRoles = totalRoles,
                TotalPage = (int)Math.Ceiling(totalRoles / (double)pageSize),
                Users = _mapper.Map<IReadOnlyList<RolesDto>>(users)
            };

            return Ok(new
            {
                Success = true,
                Message = "Get roles success",
                Data = data

            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound("Role not exist.");
            var data = _mapper.Map<RolesDto>(role);
            return Ok(new
            {
                Success = true,
                Message = "Get role success.",
                Data = data
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(AddRolesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = ModelState
                });

            var exist_Role = await _roleManager.RoleExistsAsync(dto.Name);
            if (exist_Role) return BadRequest(new
            {
                Success = false,
                Message = "Asset name already exists"
            });

            var role = new IdentityRole(dto.Name);
            await _roleManager.CreateAsync(role);

            return Ok(new
            {
                Success = true,
                Message = "Add role success.",
                Data = new
                {
                    id = Guid.Parse(role.Id),
                    name = role.Name,
                    code = role.Name.ToUpper()
                }
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(Guid id, AddRolesDto dto)
        {
            var existRole = await _roleManager.FindByIdAsync(id.ToString());
            if (existRole == null) return NotFound("Not found role.");

            existRole.Name = dto.Name;
            existRole.NormalizedName = dto.Name.ToUpper();
            await _roleManager.UpdateAsync(existRole);
            return Ok(new
            {
                Success = true,
                Message = "Update role success.",
                Data = new
                {
                    id = existRole.Id,
                    name = existRole.Name,
                    code = existRole.NormalizedName
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletRoleById(Guid id)
        {
            var existRole = await _roleManager.FindByIdAsync(id.ToString());
            if (existRole == null) return NotFound("Not found role.");

            await _roleManager.DeleteAsync(existRole);
            return Ok(new
            {
                Success = true,
                Message = "Delete role success."
            });
        }

    }
}
