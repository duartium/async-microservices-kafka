using Confluent.Kafka;
using Microservices.ApacheKafka.Shared.Models;
using System.Text.Json;

namespace Microservices.ApacheKafka.ProductAPI.Services;

public interface IProductService
{
    Task AddProduct(Product product);
    Task DeleteProduct(int id);
}
public class ProductService(
    IProducer<Null, string> producer) : IProductService
{
    private List<Product> Products = [];
    public async Task AddProduct(Product product)
    {
        Products.Add(product);
        var result = await producer.ProduceAsync("add-product-topic",
            new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(product)
            });

        if(result.Status != PersistenceStatus.Persisted)
        {
            //get last product
            var lastProduct = Products.Last();

            //remove last product
            Products.Remove(lastProduct);
        }
    }

    public async Task DeleteProduct(int id)
    {
        var productToRemove = Products.FirstOrDefault(p => p.Id == id);
        Products.Remove(productToRemove!);
        await producer.ProduceAsync(
            "delete-product-topoic",
            new Message<Null, string>
            { 
                Value = JsonSerializer.Serialize(new { Id = id })
            });

    }
}
