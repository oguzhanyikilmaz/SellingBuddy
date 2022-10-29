using IdentityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Api.Application.Services
{
    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> Login(LoginRequestModel requestModel)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,requestModel.UserName),
                new Claim(ClaimTypes.Name,"Oğuzhan Yıkılmaz"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OguzhanYklmzSecretKeyShouldBeLong"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseModel response = new LoginResponseModel()
            {
                UserName = requestModel.UserName,
                UserToken = encodedJwt
            };

            return Task.FromResult(response);
        }
    }
}
