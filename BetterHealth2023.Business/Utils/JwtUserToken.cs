using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Utils
{
    public static class JwtUserToken
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public static string CreateInternalUserToken(InternalUser internalUser)
        {
            List<Claim> claims = new List<Claim>
            {
                //employeeID
                new Claim(ClaimTypes.NameIdentifier, internalUser.Id),
                new Claim(ClaimTypes.Role, internalUser.Role.RoleName),
                new Claim(ClaimTypes.Name, internalUser.Fullname),
                new Claim("Image", internalUser.ImageUrl)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtStorage:Token").Value));
            var creds = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
