using System.Collections.Concurrent;
using WebApi.Common;
using WebApi.Controllers;
using WebApi.GameLogic;
using WebApi.GameLogic.LyssieUno;
using WebApi.Hubs;
using WebApi.Models;
using WebApi.Services;

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
// builder.Services.AddSingleton<IDictionary<string, UserConnection>>(_ => new Dictionary<string, UserConnection>());
builder.Services.AddSingleton<IDictionary<string, GameType>>(_ => new ConcurrentDictionary<string, GameType>());
builder.Services.AddSingleton<IDictionary<string, BaseRoom>>(_ => new ConcurrentDictionary<string, BaseRoom>());
builder.Services.AddSingleton<CardDbContext>();
builder.Services.AddSingleton<UnoDeckBuilder>();
//builder.Services.AddSingleton<UnoGameMod>();
builder.Services.AddSingleton<BlackJackGameStorage>();
//builder.Services.AddSingleton<UnoGameStorage>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddTransient<CahGame>();
builder.Services.AddSingleton<CahFactory>();
builder.Services.AddSingleton<IDictionary<string, CahGame>>(_ => new ConcurrentDictionary<string, CahGame>());
builder.Services.AddSingleton<IDictionary<string, BlackJackGame>>(_ => new ConcurrentDictionary<string, BlackJackGame>());
builder.Services.AddSingleton<BlackJackMessenger>();
builder.Services.AddSingleton<UnoMessenger>();
builder.Services.AddSingleton<IDictionary<string, UnoGameModLyssie>>(_ => new ConcurrentDictionary<string, UnoGameModLyssie>());



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

app.MapHub<CahHub>("/cahhub", options =>
{
    options.AllowStatefulReconnects = true;
});

app.MapHub<UneHub>("/unehub", options =>
{
    options.AllowStatefulReconnects = true;
});

app.MapHub<BlackJackHub>("/blackjackhub", options =>
{
    options.AllowStatefulReconnects = true;
});

app.Run();

public partial class Program { }
