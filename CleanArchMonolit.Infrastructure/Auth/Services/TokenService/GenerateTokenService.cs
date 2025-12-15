using CleanArchMonolit.Application.Auth.Interfaces.Token;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Infrastructure.Auth.Services.TokenService
{
    public class GenerateTokenService : IGenerateTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public GenerateTokenService(IOptions<JwtSettings> jwtOptions) 
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Profile.ProfileName),
                new Claim("ProfileId", user.Profile.Id.ToString()),
            };

            foreach (var item in user.UserPermissions)
            {
                new Claim("permissions", item.SystemPermission.PermissionCode);
                claims.Add(new Claim("permissions", item.SystemPermission.PermissionCode));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
