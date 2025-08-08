using Microservices.ApacheKafka.OrderAPI.Services;
using Microservices.ApacheKafka.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Microservices.ApacheKafka.OrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(
        IOrderService orderService) : ControllerBase
    {
        [HttpGet(Name = "start-consuming-service")]
        public async Task<IActionResult> StartConsumingService()
        {
            await orderService.StartConsumingServices();
            return NoContent();
        }

        [HttpGet("get-products")]
        public IActionResult GetProducts()
        {
            var products = orderService.GetProducts();
            return Ok(products);
        }

        [HttpPost("add-order")]
        public IActionResult AddOrder(
            Order order)
        {
            orderService.AddOrder(order);
            return Ok("Order placed");
        }
        
        [HttpGet("get-orders-summary")]
        public IActionResult GetOrdersSummary()
        {
            var ordersSummary = orderService.GetOrdersSummary();
            return Ok(ordersSummary);
        }


    }
}
