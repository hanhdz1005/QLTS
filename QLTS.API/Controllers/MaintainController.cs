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
    [Authorize(Roles = "Admin, Asset Manager")]
    public class MaintainController : ControllerBase
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public MaintainController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null
        )
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _uow.MaintainRepository
                .Query(m => m.Asset, m => m.Category, m => m.Employee);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(m =>
                    m.AssetName.ToLower().Contains(search) ||
                    m.CategoryName.ToLower().Contains(search) ||
                    m.MaintainHost.ToLower().Contains(search)
                );
            }

            query = query.OrderByDescending(m => m.RequestDate);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return Ok(new
                {
                    Success = true,
                    Message = "We don't have any maintain yet.",
                    Data = new
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    }
                });

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new PagedResult<MaintainDto>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = _mapper.Map<IReadOnlyList<MaintainDto>>(items)
            };

            return Ok(new
            {
                Success = true,
                Message = "Get all maintains success!",
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var maintain = await _uow.MaintainRepository
                .Query(m => m.Asset, m => m.Category, m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintain == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Maintain not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Get maintain success",
                Data = _mapper.Map<MaintainDto>(maintain)
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateMaintainDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = ModelState
                });

            var asset = await _uow.AssetRepository
                .Query(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == dto.AssetId);

            if (asset == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Asset not found"
                });

            var employee = await _uow.EmployeeRepository.GetAsync(dto.EmployeeId);
            if (employee == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Employee not found"
                });

            var maintain = _mapper.Map<Maintain>(dto);
            maintain.AssetName = asset.Name;
            maintain.CategoryId = asset.CategoryId;
            maintain.CategoryName = asset.Category.Name;
            maintain.MaintainHost = employee.FullName;
            maintain.Status = MaintainStatus.InProgress;

            await _uow.MaintainRepository.AddAsync(maintain);

            return Ok(new
            {
                Success = true,
                Message = "Create maintain successfully",
                Data = maintain
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateMaintainDto dto)
        {
            var existMaintain = await _uow.MaintainRepository.GetAsync(id);
            if (existMaintain == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Maintain not found"
                });

            if (dto.EndDate < dto.StartDate)
                return BadRequest(new
                {
                    Success = false,
                    Message = "EndDate must be after StartDate"
                });

            var res = _mapper.Map<Maintain>(dto);

            res.AssetName = existMaintain.AssetName;
            res.CategoryId = existMaintain.CategoryId;
            res.CategoryName = existMaintain.CategoryName;
            res.MaintainHost = existMaintain.MaintainHost;
            res.RequestDate = existMaintain.RequestDate;

            await _uow.MaintainRepository.UpdateAsync(id, res);

            return Ok(new
            {
                Success = true,
                Message = "Update maintain successfully",
                Data = new
                {
                    id = res.Id,
                    assetId = res.AssetId,
                    assetname = res.AssetName,
                    categoryId = res.CategoryId,
                    categoryName = res.CategoryName,
                    employeeId = res.EmployeeId,
                    maintainHost = res.MaintainHost,
                    resquestDate = res.RequestDate,
                    startDate = res.StartDate,
                    endDate = res.EndDate,
                    description =res.Description,
                    totalCost = res.TotalCost,
                    status = res.Status,
                    type = res.Type,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existMaintain = await _uow.MaintainRepository.GetAsync(id);
            if (existMaintain == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Maintain not found"
                });

            await _uow.MaintainRepository.DeleteAsync(id);

            return Ok(new
            {
                Success = true,
                Message = "Delete maintain successfully"
            });
        }
    }
}
