using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using QLTS.Infrastructure;
using QLTS.Infrastructure.Data.Dtos;
using System.Data;
using System.Security.Claims;

namespace QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (user == null) return Unauthorized(new
            {
                Success = false,
                Message = "Invalid email."
            });
            var result = await _signInManager.CheckPasswordSignInAsync(user, Dto.Password, false);
            if (!result.Succeeded) return Unauthorized(new
            {
                Success = false,
                Message = "Wrong password"
            });

            return Ok(new 
            {
                Succcess = true,
                Message = "Login successful.",
                Data = new {
                    Token = _tokenService.CreateToken(user)
                }
            });
        }

        private async Task<bool> CheckEmail([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) return true;
            return false;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto Dto)
        {
            var isCheckEmail = await CheckEmail(Dto.Email);
            if (isCheckEmail)
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
            await _userManager.AddToRoleAsync(user, user.Role);
            if (!result.Succeeded) return BadRequest("Register failed.");
            var userId = user.Id;
            return Ok(new 
            {
                Success = true,
                Message = "Register success.",
                Data = new {
                    id = Guid.Parse(user.Id),
                    displayName = user.DisplayName,
                    //email = user.Email,
                    //role = user.Role,
                    token = _tokenService.CreateToken(user),
                    //code = user.Code
                }
                
            });
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized();

            return Ok(new 
            {
                Success = true,
                Message = "Get current user success.",
                Data = new {
                    id = Guid.Parse(user.Id),
                    displayName = user.DisplayName,
                    //email = user.Email,
                    //role = user.Role,
                    //code = user.Code,
                }
                
            });
        }


    }
}
