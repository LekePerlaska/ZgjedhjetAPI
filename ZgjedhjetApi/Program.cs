using Microsoft.EntityFrameworkCore;
using Nest;
using StackExchange.Redis;
using ZgjedhjetApi.Data;
using ZgjedhjetApi.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LifeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LifeDatabase"))
);

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
);
builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var uri = builder.Configuration["Elasticsearch:Uri"];
    var defaultIndex = builder.Configuration["Elasticsearch:DefaultIndex"];

    var settings = new ConnectionSettings(new Uri(uri))
        .DefaultIndex(defaultIndex)
        .DefaultMappingFor<Zgjedhjet>(m => m.IdProperty(p => p.Id))
        .DisableDirectStreaming();

    return new ElasticClient(settings);
});

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
