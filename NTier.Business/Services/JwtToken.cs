using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NTier.Business.Interfaces;
using NTier.DataObject.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTier.Business.Services
{
    public class JwtToken : IJwtToken
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public JwtToken(IConfiguration config)
        {
            _config = config;
        }
        public string ExtractUserId()
        {
            string Id = "";
            if (_httpContextAccessor.HttpContext == null)
            {
                //Log.Warning("HttpContext is null in ExtractUserId");
                return string.Empty;
            }

            var tokenIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = tokenIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }
            return Id;
        }

        public string GenerateToken(User user)
        {
            //var userrole = (user.Id == SuperAdminData.SuperAdmin_Key || user.ParentId != null) ? SystemUserRoles.Admin : SystemUserRoles.User;
            var userrole = "User";
            var securityToken = _config["JwtToken:Token"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityToken));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, userrole)

        };
            var token = new JwtSecurityToken(_config["JwtToken:Token"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public bool VerifyJwtToken(string token)
        {
            var securityToken = _config["JwtToken:Token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityToken);

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // You may set this to true to validate the token's issuer
                ValidateAudience = false, // You may set this to true to validate the audience
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return validatedToken.ValidTo > DateTime.UtcNow; // Token is valid
            }
            catch (Exception)
            {
                return false; // Token is invalid
            }
        }
        public DateTimeOffset? GetTokenExpiry(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;
            return jwtToken?.ValidTo;
        }
    }
}
