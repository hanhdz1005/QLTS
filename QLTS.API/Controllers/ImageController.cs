using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage( IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new
                {
                    Success = false,
                    Message = "No file uploaded."
                });

            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "uploads"
            );

            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new
            {
                Success = true,
                Message = "Image uploaded successfully!",
                ImageUrl = fileUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "uploads"
            );
            if (!Directory.Exists(uploadsFolder))
            {
                return Ok(new
                {
                    Success = true,
                    Message = "No images found.",
                    Images = new List<string>()
                });
            }
            var imageFiles = Directory.GetFiles(uploadsFolder);
            var imageUrls = imageFiles
            .Select((file, index) => new
            {
                Image = index + 1,
                Url = $"{Request.Scheme}://{Request.Host}/uploads/{Path.GetFileName(file)}"
            })
            .ToList();
            return Ok(new
            {
                Success = true,
                Message = "Images retrieved successfully!",
                Images = imageUrls
            });
        }

    }
}
