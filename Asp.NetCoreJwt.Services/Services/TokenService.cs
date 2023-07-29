using Asp.NetCoreJwt.Core.Configuration;
using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Models;
using Asp.NetCoreJwt.Core.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Services.Services
{
    public class TokenService : ITokenServices
    {
        private readonly UserApp _userApp;
        private readonly CustomTokenOptions _customTokenOptions;
        public TokenService(UserApp userApp, IOptions< CustomTokenOptions> customTokenOptions)
        {
            _userApp = userApp;
            _customTokenOptions = customTokenOptions.Value;
        }
        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var rnd=RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
        private IEnumerable<Claim> GetClaim(UserApp user,List<string> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userList;
        }
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString());  
            new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString());
            return claims;
        }
        public TokenDto CreateToken(UserApp userApp)
        {
            var accsessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccsessTokenExpiration);
            var refreshTokenExpiration=DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                issuer: _customTokenOptions.Issuer,
                expires: accsessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaim(userApp, _customTokenOptions.Audience),
                signingCredentials:signingCredentials
            
                );
            var handler = new JwtSecurityTokenHandler();
            var token=handler.WriteToken(jwtSecurityToken);
            var tokenDto = new TokenDto
            {
                AccsessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccsessTokenExpiration = accsessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;

        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accsessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccsessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                issuer: _customTokenOptions.Issuer,
                expires: accsessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials

                );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new ClientTokenDto
            {
                AccsessToken = token,
                AccsessTokenExpiration = accsessTokenExpiration,
              
            };
            return tokenDto;

        }
    }
}
