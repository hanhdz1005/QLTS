using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using QLTS.Infrastructure;
using QLTS.Infrastructure.Data.Dtos;
using System.Security.Claims;

namespace QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly IMapper _mapper;
        public readonly AppDbContext _context;
        public readonly ITokenServices _tokenService;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IMapper mapper, AppDbContext context, ITokenServices tokenServices)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _context = context;
            _tokenService = tokenServices;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto Dto)
        {
            var user = await _userManager.FindByEmailAsync(Dto.Email);
            if (user == null) return Unauthorized("Invalid email.");
            var result = await _signInManager.CheckPasswordSignInAsync(user, Dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Wrong password.");

            return Ok(new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Role = user.Role,
                Code = user.Code
            });
        }

        [HttpGet("checkEmail")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) return true;
            return false;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto Dto)
        {
            if (CheckEmail(Dto.Email).Result.Value)
            {
                return BadRequest("Email already in use.");

            }
            var user = new AppUser
            {
                DisplayName = Dto.DisplayName,
                Email = Dto.Email,
                UserName = Dto.Email,
                Role = "User",
            };
            user.Code = user.DisplayName.ToUpper();
            var result = await _userManager.CreateAsync(user, Dto.Password);
            if (!result.Succeeded) return BadRequest("Register failed.");
            var userId = user.Id;
            return Ok(new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = user.Role,
                Token = _tokenService.CreateToken(user),
                Code = user.Code
            });
        }

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized();

            return Ok(new GetUserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = user.Role,
                Code = user.Code,

            });
        }


    }
}
