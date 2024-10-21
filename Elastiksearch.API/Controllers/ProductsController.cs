using Elastiksearch.API.DTOs;
using Elastiksearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elastiksearch.API.Controllers
{
    //Base controller oluşturduğumuz için burada actionlara ihtiyacumınz yok 
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService; //newlenemez

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionresult(await _productService.SaveAsync(request));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            var value = await _productService.UpdateAsync(productUpdateDto);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var value = await _productService.GetAllAsync();
            return Ok(value);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            var value = await _productService.GetByIdAsync(id);
            return Ok(value);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var value = await _productService.DeleteAsync(id);
            return Ok();
        }

    }
}
