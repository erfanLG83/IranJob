using IranJob.Domain.Auth;
using IranJob.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IranJob.Services.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IAppUserManager _userManager;
        public JwtService(IAppUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GenerateTokenAsync(AppUser user)
        {
            var securityKey = Encoding.UTF8.GetBytes("1234567890asdfgh");
            var encryptionKey = Encoding.UTF8.GetBytes("qwsadfrewtyh4532");
            var encryptionCredentials = new EncryptingCredentials(
                    new SymmetricSecurityKey(encryptionKey),
                    SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256
                );
            var signinCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(securityKey),
                    SecurityAlgorithms.HmacSha256Signature
                );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "IranJob",
                Audience = "IranJob",
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(0),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signinCredentials,
                Subject = new ClaimsIdentity(await GetClaimsAsync(user)),
                EncryptingCredentials = encryptionCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            if (user.PhoneNumber != null)
                claims.Add(
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            if (user.SecurityStamp != null)
                claims.Add(
                    new Claim(new ClaimsIdentityOptions().SecurityStampClaimType, user.SecurityStamp));
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            return claims;
        }
    }
}
