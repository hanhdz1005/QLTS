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

        public TokenServices(IConfiguration config) 
        {
            _config = config;

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
                new Claim(ClaimTypes.Role, role)
            };

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
