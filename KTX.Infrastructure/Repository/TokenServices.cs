using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QLTS.Core.Entities;
using QLTS.Core.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Repository
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        //==========================================================//
        //private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //==========================================================//
        public TokenServices(IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _config = config;
            //=============================== 
            //_roleManager = roleManager;
            //_userManager = userManager;
            //===============================
            var key = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is missing");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
        public string CreateToken(AppUser appUser)
        {
            var role = appUser.Role;

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.DisplayName),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id)
            };
            //=====================================================//
            //var userClaims = await _userManager.GetClaimsAsync(appUser);
            //Claims.AddRange(userClaims);

            //var roles = await _userManager.GetRolesAsync(appUser);
            //foreach (var roleName in roles)
            //{
            //    Claims.Add(new Claim(ClaimTypes.Role, roleName)); //{author(Roles =...)

            //    var role = await _roleManager.FindByIdAsync(roleName); //rold claims
            //    var roleClaims = await _roleManager.GetClaimsAsync(role);
            //    Claims.AddRange(roleClaims);
            //}
            //=====================================================//
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(10),
                Issuer = _config["Jwt:Issuer"],
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }


        
    }
}
