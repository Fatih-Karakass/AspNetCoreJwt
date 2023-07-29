using Asp.NetCoreJwt.Core.Repositories;
using Asp.NetCoreJwt.Core.Services;
using Asp.NetCoreJwt.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCoreJwt.Services.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<TEntity> genericRepository;

        public ServiceGeneric(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            this.unitOfWork = unitOfWork;
            this.genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await genericRepository.AddAsync(newEntity);
            await unitOfWork.CommitAsync();
            var newDto=ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Succsess(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
           var products=ObjectMapper.Mapper.Map<List<TDto>>(await genericRepository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Succsess(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
           var product=await genericRepository.GetByIdAsync(id);
            if (product == null)
            {
                return Response<TDto>.Fail("Id Not Found", 404, true);
            }
            return Response<TDto>.Succsess(ObjectMapper.Mapper.Map<TDto>(product),200);
        }

        public async Task<Response<NoDataDto>> RemoveAsync(int id)
        {
            var product = await genericRepository.GetByIdAsync(id);
            if (product == null)
            {
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);
            }
             genericRepository.Remove(product);
            await unitOfWork.CommitAsync();
            return Response<NoDataDto>.Succsess(200);
        }

        public async Task<Response<NoDataDto>> UpdateAsync(TDto entity, int id)
        {
            var isExistEntity= await genericRepository.GetByIdAsync(id);
            if(isExistEntity==null)
            {
                return Response<NoDataDto>.Fail("Id Not Found", 404,true);
            }
            var updateEntity=ObjectMapper.Mapper.Map<TEntity>(entity);
            genericRepository.Update(updateEntity);
            await unitOfWork.CommitAsync();
            return Response<NoDataDto>.Succsess(200);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list=genericRepository.Where(predicate);
             return Response<IEnumerable<TDto>>.Succsess(ObjectMapper.Mapper.Map<IEnumerable<TDto>>( await list.ToListAsync()),200);
        }
    }
}
