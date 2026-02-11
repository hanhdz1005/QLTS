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
    [Authorize(Roles = "Employee Manager, Admin")]
    public class EmployeeController : ControllerBase
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("Get_all_assets")]
        public async Task<ActionResult> GetALL(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _uow.EmployeeRepository
                .Query()
                .Include(e => e.Departments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(e =>
                    e.FullName.ToLower().Contains(search)
                );
            }

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return NotFound(new
                {
                    Success = false,
                    Message = "No employees found.",
                });

            var items = await query
                .OrderBy(e => e.FullName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new PagedResult<EmployeeDto>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = _mapper.Map<IReadOnlyList<EmployeeDto>>(items)
            };

            return Ok(new
            {
                Success = true,
                Message = "Get all Employees success!",
                Data = data
            });
        }



        [HttpGet("Get_by_Id:{id:guid}")]
        public async Task<ActionResult<Employees>> GetEmployeeById(Guid id)
        {
            var employee = await _uow.EmployeeRepository.Query(c => c.Departments).FirstOrDefaultAsync(a => a.Id == id);
            if (employee == null) return NotFound(new
            {
                Success = false,
                Message = $"Are u sure that this id is correct?"
            });

            return Ok(new
            {
                Success = true,
                Message = $"This id is real!! ^-^",
                data = _mapper.Map<EmployeeDto>(employee)
            });

        }

        [HttpPost("Add_new_employee")]
        public async Task<ActionResult> AddEmployeeById(AddEmployeeDto employee)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                Success = false,
                Message = ModelState
            });

            var exist_Employee = await _uow.EmployeeRepository.CheckAsync(e => e.FullName == employee.FullName);
            if (exist_Employee) return BadRequest(new
            {
                Success = false,
                Message = "Employee name already exists"
            });

            var exist_Email = await _uow.EmployeeRepository.CheckAsync(e => e.Email == employee.Email.Trim());
            if (exist_Email) return BadRequest(new
            {
                Success = false,
                Message = "Email already exists"
            });

            var exist_Department = await _uow.DepartmentRepository.GetByIdAsync(employee.DeptId);
            if (exist_Department == null) return NotFound(new
            {
                Success = false,
                Message = "DeptId not found"
            });

            var res = _mapper.Map<Employees>(employee);
            res.DeptName = exist_Department.Name;
            await _uow.EmployeeRepository.AddAsync(res);

            var result = _mapper.Map<EmployeeDto>(res);
            var data = (new
            {
                id = result.Id,
                fullName = result.FullName,
                email = result.Email,
                deptId = result.DeptId,
                deptName = res.DeptName
            });
            return Ok(new
            {
                Success = true,
                Message = "Add employee successfully",
                Data = data
            });
        }

        [HttpPut("Update_employee_by_id:{id}")]
        public async Task<ActionResult> UpdateEmployeeById(Guid id, AddEmployeeDto employee)
        {
            var existEmployee = await _uow.EmployeeRepository.Query(c => c.Departments).FirstOrDefaultAsync(a => a.Id == id);
            if (existEmployee == null) return NotFound(new
            {
                Success = false,
                Message = "Not found employee"
            });
            var res = _mapper.Map<Employees>(employee);
            res.DeptName = existEmployee.Departments.Name;

            if(res.DeptId != existEmployee.DeptId)
            {
                var exist_Department = await _uow.DepartmentRepository.GetByIdAsync(employee.DeptId);
                if (exist_Department == null) return NotFound(new
                {
                    Success = false,
                    Message = "DeptId not found"
                });
                res.DeptName = exist_Department.Name;
            }

            await _uow.EmployeeRepository.UpdateAsync(id, res);
            var data = (new
            {
                id = res.Id,
                fullName = res.FullName,
                email = res.Email,
                deptId = res.DeptId,
                deptName = res.DeptName
            });
            return Ok(new
            {
                Success = true,
                Message = "Update employee successfully",
                Data = data
            });

        }


        [HttpDelete("Delete_employee_by_id:{id}")]
        public async Task<ActionResult> DeleteEmployeeById(Guid id)
        {
            var existEmployee = await _uow.EmployeeRepository.GetAsync(id);
            if (existEmployee == null) return NotFound(new
            {
                Success = false,
                Message = "Not found employee"
            });
            await _uow.EmployeeRepository.DeleteAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Delete employee successfully"
            });
        }
    }
}
