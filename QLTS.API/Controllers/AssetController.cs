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
    public class AssetController : ControllerBase
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public AssetController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("filter")]
        public async Task<ActionResult> GetALL(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _uow.AssetRepository
                .Query()
                .Include(a => a.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(search)
                );
            }

            query = query.OrderBy(a => a.Name);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return Ok(new
                {
                    Success = true,
                    Message = "We don't have any asset yet.",
                    Data = new
                    {
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                    }
                });

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = new PagedResult<AssetDto>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = _mapper.Map<IReadOnlyList<AssetDto>>(items)
            };

            return Ok(new
            {
                Success = true,
                Message = "Get all assets success!",
                Data = data
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Assets>> GetAssetById(Guid id)
        {
            var asset = await _uow.AssetRepository.Query(c => c.Category).FirstOrDefaultAsync(a => a.Id == id);
            if (asset == null) return NotFound(new
            {
                Success = false,
                Message = $"Are u sure that this id is correct?"
            });

            return Ok(new
            {
                Success = true,
                Message = $"This id is real!! ^-^",
                data = _mapper.Map<AssetDto>(asset)
            });

        }

        [HttpPost()]
        public async Task<ActionResult> AddAssetById(AddAssetDto asset)
        {
            if (!ModelState.IsValid) return BadRequest(new
            {
                Success = false,
                Message = ModelState
            });

            var exist_Asset = await _uow.AssetRepository.CheckAsync(c => c.Name == asset.Name);
            if (exist_Asset) return BadRequest(new
            {
                Success = false,
                Message = "Asset name already exists"
            });

            var exist_Category = await _uow.CategoryRepository.GetByIdAsync(asset.CategoryId);
            if (exist_Category == null) return NotFound(new
            {
                Success = false,
                Message = "CategoryId not found"
            });

            var res = _mapper.Map<Assets>(asset);
            res.CategoryName = exist_Category.Name;
            await _uow.AssetRepository.AddAsync(res);

            var data = (new
            {
                id = res.Id,
                name = res.Name,
                imgUrl = res.ImgUrl,
                categoryId = res.CategoryId,
                categoryName = res.CategoryName,
                status = res.Status
            });
            return Ok(new
            {
                Success = true,
                Message = "Add asset successfully",
                Data = data
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAssetById(Guid id, AddAssetDto asset)
        {
            var existAsset = await _uow.AssetRepository.GetAsync(id);
            if (existAsset == null) return NotFound(new
            {
                Success = false,
                Message = "Not found asset"
            });
            var res = _mapper.Map<Assets>(asset);
            res.CategoryName = existAsset.CategoryName;

            if (res.CategoryId != existAsset.CategoryId)
            {
                var exist_Category = await _uow.CategoryRepository.GetByIdAsync(asset.CategoryId);
                if (exist_Category == null) return NotFound(new
                {
                    Success = false,
                    Message = "CategoryId not found"
                });
                res.CategoryName = exist_Category.Name;
            }

            await _uow.AssetRepository.UpdateAsync(id, res);
            var data = (new
            {
                id = res.Id,
                name = res.Name,
                imgUrl = res.ImgUrl,
                categoryId = res.CategoryId,
                categoryName = res.CategoryName,
                status = res.Status
            });
            return Ok(new
            {
                Success = true,
                Message = "Update asset successfully",
                Data = data
            });

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAssetById(Guid id)
        {
            var existAsset = await _uow.AssetRepository.GetAsync(id);
            if (existAsset == null) return NotFound(new
            {
                Success = false,
                Message = "Not found asset"
            });
            await _uow.AssetRepository.DeleteAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Delete asset successfully"
            });
        }

    }
}
