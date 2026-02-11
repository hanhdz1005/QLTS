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
    public class CategoryController : ControllerBase
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public CategoryController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("Get_all_categories")]
        public async Task<ActionResult> GetALL(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _uow.CategoryRepository
                .Query()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(search)
                );
            }

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return NotFound(new
                {
                    Success = false,
                    Message = "No categories found.",
                });

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new PagedResult<CategoryDto>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = _mapper.Map<IReadOnlyList<CategoryDto>>(items)
            };

            return Ok(new
            {
                Success = true,
                Message = "Get all categories success!",
                Data = data
            });
        }




        [HttpGet("Get_by_Id:{id:guid}")]
        public async Task<ActionResult<Categories>> GetCategoryById(Guid id)
        {
            var category = await _uow.CategoryRepository.GetAsync(id);
            if (category == null) return NotFound(new
            {
                Success = false,
                Message = $"Are u sure that this id is correct?"
            });

            return Ok(new
            {
                Success = true,
                Message = $"This id is real!! ^-^",
                data = _mapper.Map<CategoryDto>(category)
            });

        }

        [HttpPost("Add_new_category")]
        public async Task<ActionResult> AddCategoryById(AddCategoryDto category)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                Success = false,
                Message = ModelState
            });

            var exist = await _uow.CategoryRepository.CheckAsync(c => c.Name.Trim().ToLower() == category.Name.ToLower());
            if (exist) return BadRequest(new
            {
                Success = false,
                Message = "Category already exists"
            });

            var res = _mapper.Map<Categories>(category);
            await _uow.CategoryRepository.AddAsync(res);

            return Ok(new
            {
                Success = true,
                Message = "Add category successfully",
                Data = new
                {
                    id = res.Id,
                    name = res.Name
                }
            });
        }

        [HttpPut("Update_category_by_id:{id}")]
        public async Task<ActionResult> UpdateCategoryById(Guid id, AddCategoryDto category)
        {
            var existCategory = await _uow.CategoryRepository.GetAsync(id);
            if (existCategory == null) return NotFound(new
            {
                Success = false,
                Message = "Not found category"
            });
            var res = _mapper.Map<Categories>(category);
            await _uow.CategoryRepository.UpdateAsync(id, res);
            var data = (new
            {
                id = res.Id,
                name = res.Name
            });
            return Ok(new
            {
                Success = true,
                Message = "Update category successfully",
                Data = data
            });

        }

        [HttpDelete("Delete_category_by_id:{id}")]
        public async Task<ActionResult> DeleteCategoryById(Guid id)
        {
            var existCategory = await _uow.CategoryRepository.GetAsync(id);
            if (existCategory == null) return NotFound(new
            {
                Success = false,
                Message = "Not found catgory"
            });
            await _uow.CategoryRepository.DeleteAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Delete category successfully"
            });
        }

    }
}
