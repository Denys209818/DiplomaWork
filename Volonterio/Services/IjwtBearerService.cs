using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volonterio.Data.Entities;

namespace Volonterio.Services
{
    public interface IJwtBearerService
    {
        public string GenerateToken(AppUser user);
    }

    public class JwtBearerService : IJwtBearerService
    {
        private UserManager<AppUser> _userManager;
        private IConfiguration _configuration;
        public JwtBearerService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public string GenerateToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("email", user.UserName));
            claims.Add(new Claim("firstName", user.FirstName));
            claims.Add(new Claim("secondName", user.SecondName));
            claims.Add(new Claim("image", user.Image));
            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim("phone", user.PhoneNumber.ToString()));

            var roles = _userManager.GetRolesAsync(user).Result; 
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetValue<String>("private_key")
                ));

            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(claims: claims, signingCredentials: credentials, expires: DateTime.Now.AddDays(10));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}
