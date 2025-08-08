using Microservices.ApacheKafka.ProductAPI.Services;
using Microservices.ApacheKafka.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ApacheKafka.ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(
        IProductService productService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddProduct(
            Product product)
        {
           await productService.AddProduct(product);
            return Created();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
