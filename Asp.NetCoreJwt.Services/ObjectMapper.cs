using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Services
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> mapper=new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper=> mapper.Value;
    }
}
