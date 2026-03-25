using Microsoft.AspNetCore.Mvc;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels.ProductModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("search")]
        public async Task<IActionResult> GetProduct(int pageNumber, int pageSize,[FromForm] QueryProduct? queryProduct)
        {
            var result = await _productService.Search(pageNumber, pageSize, queryProduct);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductRequest productRequest)
        {
            var result = await _productService.AddNew(productRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, ProductRequest productRequest)
        {
            var result = await _productService.Update(id, productRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
