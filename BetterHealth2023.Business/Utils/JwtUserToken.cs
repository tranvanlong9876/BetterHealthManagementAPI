using BetterHealthManagementAPI.BetterHealth2023.Repository.Commons;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.Site;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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


        public static string CreateInternalUserToken(InternalUser internalUser, SiteViewModel workingSite)
        {
            var roleName = internalUser.Role.RoleName;
            List<Claim> claims = new List<Claim>
            {
                //employeeID
                new Claim(ClaimTypes.NameIdentifier, internalUser.Id),
                new Claim(ClaimTypes.Role, internalUser.Role.RoleName),
                new Claim(ClaimTypes.Name, internalUser.Fullname),
                new Claim("Image", internalUser.ImageUrl)
            };

            if (roleName.Equals(Commons.PHARMACIST_NAME) || roleName.Equals(Commons.MANAGER_NAME))
            {
                claims.Add(new Claim("SiteID", workingSite.Id));
                claims.Add(new Claim("SiteName", workingSite.SiteName));
            }


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
        public static string CreateCustomerToken(Customer customer)
        {
            List<Claim> claims = new List<Claim>
            {
                //employeeID
                new Claim(ClaimTypes.NameIdentifier, customer.Id),
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim(ClaimTypes.Name, customer.Fullname),
                new Claim("Image", customer.ImageUrl)
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

        public static string DecodeFireBaseTokenToPhoneNo(string firebaseToken)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var payLoadString = tokenHandler.ReadJwtToken(firebaseToken).Payload.SerializeToJson();
                dynamic payLoadJson = JObject.Parse(payLoadString);
                string phoneNo = payLoadJson.phone_number;
                return "0" + phoneNo.Substring(3);
            } catch(Exception ex)
            {
                return null;
            }
            
        }

        public static string DecodeAPITokenToRole(string jwtToken)
        {
            if(jwtToken == null)
            {
                return null;
            }
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var claim = tokenHandler.ReadJwtToken(jwtToken).Claims.Where(x => x.Type.Equals(ClaimTypes.Role)).Select(x => x.Value).FirstOrDefault();
                return claim;
            } catch
            {
                return null;
            }
        }
    }
}
