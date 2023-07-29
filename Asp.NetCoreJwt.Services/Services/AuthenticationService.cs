using Asp.NetCoreJwt.Core.Configuration;
using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Models;
using Asp.NetCoreJwt.Core.Repositories;
using Asp.NetCoreJwt.Core.Services;
using Asp.NetCoreJwt.Core.UnitOfWork;
using Asp.NetCoreJwt.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenServices _tokenServices;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _repository;

        public AuthenticationService(IGenericRepository<UserRefreshToken> repository, IUnitOfWork unitOfWork, UserManager<UserApp> userManager, ITokenServices tokenServices, IOptions<List<Client>> clients)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _tokenServices = tokenServices;
            _clients = clients.Value;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
           if(loginDto == null) throw new ArgumentNullException(nameof(loginDto));
           var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Response<TokenDto>.Fail("Email or password is wrong", 400, true);
            if(!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Response<TokenDto>.Fail("Email or password is wrong", 400, true);
            }
            var token = _tokenServices.CreateToken(user);
            var userRefreshToken = await _repository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
              await  _repository.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Succsess(token, 200);

        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or secret not found",400,true);
            }
            var token=_tokenServices.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Succsess(token,200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string RefreshToken)
        {
            var existRefreshToken = await _repository.Where(x => x.Code == RefreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("refresh token is not found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if(user== null) 
            {
                return Response<TokenDto>.Fail("userId not found",404,true);
            }
            var tokenDto = _tokenServices.CreateToken(user);
            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Succsess(tokenDto,200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string RefreshToken)
        {
            var existRefreshToken=await _repository.Where(x=>x.Code==RefreshToken).SingleOrDefaultAsync();
            if(existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("refresh token not found", 400, true);
            }
            _repository.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Succsess(200);

        }
    }
}
