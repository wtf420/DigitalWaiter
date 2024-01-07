using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.InMemory;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<FoodContext>(opt =>
    opt.UseInMemoryDatabase("FoodList"));
builder.Services.AddDbContext<OrderContext>(opt =>
    opt.UseInMemoryDatabase("OrderList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage());
builder.Services.AddHangfireServer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseAuthorization();

app.MapControllers();

string hostName = Dns.GetHostName();
IPHostEntry? hostEntry = Dns.GetHostEntry(hostName);
IPAddress? ipv4Address = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

app.Run();
