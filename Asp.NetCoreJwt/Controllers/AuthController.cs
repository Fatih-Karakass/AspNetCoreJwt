using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCoreJwt.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result=await _authenticationService.CreateTokenAsync(loginDto);
            return actionResultInstance(result);
        }
        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto loginDto)
        {
            var result =  _authenticationService.CreateTokenByClient(loginDto);
            return actionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto loginDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(loginDto.Token);
            return actionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto loginDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(loginDto.Token);
            return actionResultInstance(result);
        }
    }
}
