using Confluent.Kafka;
using Microservices.ApacheKafka.ProductAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    Acks = Acks.All,
    EnableIdempotence = true,
    MessageTimeoutMs = 10000,
};
builder.Services.AddSingleton(
    new ProducerBuilder<Null, string>(config).Build());

builder.Services.AddScoped<IProductService, ProductService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
