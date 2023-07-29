using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Models;
using Asp.NetCoreJwt.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUser(CreateUserDto createUserDto)
        {
           var user=new UserApp { Email= createUserDto.Email ,UserName=createUserDto.UserName,City=createUserDto.City};
            var result=await _userManager.CreateAsync(user,createUserDto.Password);
            if(!result.Succeeded)
            {
                var errors=result.Errors.Select(x=>x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }
            return Response<UserAppDto>.Succsess(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Response<UserAppDto>.Fail("user not found", 400, true);
            }
            return Response<UserAppDto>.Succsess(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
