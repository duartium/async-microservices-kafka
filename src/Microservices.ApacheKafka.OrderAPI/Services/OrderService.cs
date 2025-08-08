using Confluent.Kafka;
using Microservices.ApacheKafka.Shared.Models;
using System.Text.Json;

namespace Microservices.ApacheKafka.OrderAPI.Services;

public interface IOrderService
{
    Task StartConsumingServices();
    void AddOrder(Order order);
    List<Product> GetProducts();
    List<OrderSummary> GetOrdersSummary();
}
public class OrderService(
    IConsumer<Null, string> consumer) : IOrderService
{
    private const string AddProductTopic = "add-product-topic";
    private const string DeleteProductTopic = "delete-product-topic";
    public List<Product> Products { get; set; } = [];
    public List<Order> Orders { get; set; } = [];

    public void AddOrder(Order order)
    => Orders.Add(order);

    public List<OrderSummary> GetOrdersSummary()
    {
        var orderSummary = new List<OrderSummary>();
        foreach(var order in Orders)
            orderSummary.Add(new OrderSummary
            {
                OrderId = order.Id,
                ProductId = order.ProductId,
                ProductName = Products.FirstOrDefault(p => p.Id == order.ProductId)!.Name,
                ProductPrice = Products.FirstOrDefault(p => p.Id == order.ProductId)!.Price,
                OrderedQuantity = order.Quantity,
            });
        return orderSummary;
    }

    public List<Product> GetProducts() => Products;

    public async Task StartConsumingServices()
    {
        await Task.Delay(10);
        consumer.Subscribe(
        [
            AddProductTopic,
            DeleteProductTopic
        ]);
        while (true)
        {
            var response = consumer.Consume();
            if (!string.IsNullOrEmpty(response.Message.Value))
            {
                if(response.Topic == AddProductTopic)
                {
                    var product = JsonSerializer.Deserialize<Product>(
                        response.Message.Value);
                    Products.Add(product!);
                }else if (response.Topic == DeleteProductTopic)
                {
                    Products.Remove(Products.First(
                        p => p.Id == int.Parse(response.Message.Value)));

                    ConsoleProduct();
                }
            }
        }
    }

    private void ConsoleProduct()
    {
        Console.Clear();
        Console.WriteLine("Products in Order Service");
        foreach (var product in Products)
            Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
    }
}
