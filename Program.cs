using Microsoft.EntityFrameworkCore;
using speedupApi.Data;
using speedupApi.Repositories;
using speedupApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IPriceService, PriceService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
  options.InstanceName = builder.Configuration.GetValue<string>("Redis:Name");
  options.Configuration = builder.Configuration.GetValue<string>("Redis:Host");
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
