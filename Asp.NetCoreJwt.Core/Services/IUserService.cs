using Asp.NetCoreJwt.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUser(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    }
}
