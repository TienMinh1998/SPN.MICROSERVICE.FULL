using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SPNApplication.Authentication
{
    public class JWTHandler
    {
        public string CreateToken(string user_name, int user_id, string[] permission = null)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user_name),
                new Claim("UserId", user_id.ToString())
            };

            if (permission != null && permission.Length > 0)
            {
                foreach (var item in permission)
                {
                    claims.Add(new Claim("role", item));
                }
            }
            string my_key = JWTConstant.JWT_KEY;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(my_key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
