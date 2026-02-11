using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using QLTS.Infrastructure.Data.Dtos;

namespace QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Department Manager, Admin")]
    public class DepartmentController : ControllerBase
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("Get_all_departments")]
        public async Task<ActionResult> GetALL(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _uow.DepartmentRepository
                .Query()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(d =>
                    d.Name.ToLower().Contains(search)
                );
            }

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return NotFound(new
                {
                    Success = false,
                    Message = "No departments found.",
                });

            var items = await query
                .OrderBy(d => d.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new PagedResult<DepartmentDto>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = _mapper.Map<IReadOnlyList<DepartmentDto>>(items)
            };

            return Ok(new
            {
                Success = true,
                Message = "Get all department success!",
                Data = data
            });
        }


        [HttpGet("Get_by_Id:{id:guid}")]
        public async Task<ActionResult<Departments>> GetDepartmentById(Guid id)
        {
            var department = await _uow.DepartmentRepository.GetAsync(id);
            if (department == null) return NotFound(new
            {
                Success = false,
                Message = $"Are u sure that this id is correct?"
            });

            return Ok(new
            {
                Success = true,
                Message = $"This id is real!! ^-^",
                data = _mapper.Map<DepartmentDto>(department)
            });

        }

        [HttpPost("Add_new_department")]
        public async Task<ActionResult> AddDepartmentById(AddDepartmentDto department)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                Success = false,
                Message = ModelState
            });

            var exist = await _uow.DepartmentRepository.CheckAsync(c => c.Name.Trim().ToLower() == department.Name.ToLower());
            if (exist) return BadRequest(new
            {
                Success = false,
                Message = "Department already exists"
            });

            var res = _mapper.Map<Departments>(department);
            await _uow.DepartmentRepository.AddAsync(res);
            var data = (new
            {
                id = res.Id,
                name = res.Name,
                description = res.Description
            });

            return Ok(new
            {
                Success = true,
                Message = "Add department successfully",
                Data = data
            });
        }

        [HttpPut("Update_department_by_id:{id}")]
        public async Task<ActionResult> UpdateDepartmentById(Guid id, AddDepartmentDto department)
        {
            var existDepartment = await _uow.DepartmentRepository.GetAsync(id);
            if (existDepartment == null) return NotFound(new
            {
                Success = false,
                Message = "Not found department"
            });
            var res = _mapper.Map<Departments>(department);
            await _uow.DepartmentRepository.UpdateAsync(id, res);
            var data = (new
            {
                id = res.Id,
                name = res.Name,
                description = res.Description
            });
            return Ok(new
            {
                Success = true,
                Message = "Update department successfully",
                Data = data
            });

        }

        [HttpDelete("Delete_department_by_id:{id}")]
        public async Task<ActionResult> DeleteDepartmentById(Guid id)
        {
            var existDepartment = await _uow.DepartmentRepository.GetAsync(id);
            if (existDepartment == null) return NotFound(new
            {
                Success = false,
                Message = "Not found department"
            });
            await _uow.DepartmentRepository.DeleteAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Delete department successfully"
            });
        }
    }
}
