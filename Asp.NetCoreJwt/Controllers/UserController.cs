using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCoreJwt.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto loginDto)
        {
            return actionResultInstance(await userService.CreateUser(loginDto));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return actionResultInstance(await userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }

    }
}
