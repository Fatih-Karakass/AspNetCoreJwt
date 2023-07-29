using Asp.NetCoreJwt.Core.Dtos;
using Asp.NetCoreJwt.Core.Models;
using Asp.NetCoreJwt.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCoreJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : CustomBaseController
    {
        private readonly IServiceGeneric<Product,ProductDto> _productService;

        public ProductController(IServiceGeneric<Product, ProductDto> productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return actionResultInstance(await _productService.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return actionResultInstance(await _productService.AddAsync(productDto));

        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto  productDto)
        {
            return actionResultInstance( await _productService.UpdateAsync(productDto,productDto.Id));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return actionResultInstance(await _productService.RemoveAsync(id));  
        }


    }
}
