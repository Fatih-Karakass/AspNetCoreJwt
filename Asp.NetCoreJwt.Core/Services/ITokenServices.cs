using Asp.NetCoreJwt.Core.Configuration;
using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Core.Services
{
    public interface ITokenServices
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
