using WebApi.Controllers;
using WebApi.GameLogic;
using WebApi.Hubs;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(hubOptions =>
{
    if (builder.Environment.IsDevelopment())
    {
        hubOptions.EnableDetailedErrors = true;
    }
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });
    options.AddPolicy("ProductionPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("https://playcardhub.vercel.app", "https://playcardhub.com")
            .AllowCredentials();
    });
});
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(options => new Dictionary<string, UserConnection>());
builder.Services.AddSingleton<HashSet<string>>(options => new HashSet<string>());
builder.Services.AddSingleton<CardDbContext>();
builder.Services.AddSingleton<IBaseGame<UnoCard>>(options => new UnoGame());

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseAuthorization();
app.MapControllers();
app.MapHub<BaseHub>("/basehub", options =>
{
    options.AllowStatefulReconnects = true;
});

app.Run();

public partial class Program { }

//// For simulations
//using WebApi.GameLogic.Simulations;
//UnoGameSim.Simulate();
