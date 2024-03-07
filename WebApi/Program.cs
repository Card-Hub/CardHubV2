//using WebApi.Common;
//using WebApi.Controllers;
//using WebApi.Hubs;

//var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddSignalR(hubOptions =>
//{
//    if (builder.Environment.IsDevelopment())
//    {
//        hubOptions.EnableDetailedErrors = true;
//    }
//});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("DevelopmentPolicy", policyBuilder =>
//    {
//        policyBuilder.AllowAnyMethod()
//            .AllowAnyHeader()
//            .WithOrigins("http://localhost:3000")
//            .AllowCredentials();
//    });
//    options.AddPolicy("ProductionPolicy", policyBuilder =>
//    {
//        policyBuilder.AllowAnyMethod()
//            .AllowAnyHeader()
//            .WithOrigins("https://playcardhub.vercel.app", "https://playcardhub.com")
//            .AllowCredentials();
//    });
//});
//builder.Services.AddSingleton<IDictionary<string, UserConnection>>(options => new Dictionary<string, UserConnection>());
//builder.Services.AddSingleton<HashSet<string>>(options => new HashSet<string>());
//builder.Services.AddSingleton<CardDbContext>();

//var app = builder.Build();

//app.UseHttpsRedirection();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.UseCors("DevelopmentPolicy");
//}
//else
//{
//    app.UseCors("ProductionPolicy");
//}

//app.UseAuthorization();
//app.MapControllers();
//app.MapHub<BaseHub>("/basehub", options =>
//{
//    options.AllowStatefulReconnects = true;
//});

//app.Run();

using WebApi.GameLogic;
using WebApi.Models;

UnoGame game = new UnoGame();
game.AddPlayer("Alex");
game.AddPlayer("Liam");
game.AddPlayer("Lyssie");
game.AddPlayer("Rubi");
Console.WriteLine(game.GetPlayerList());
foreach (string player in game.GetPlayerList()) {
  Console.WriteLine(player);
}
game.shufflePlayers();
foreach (string player in game.GetPlayerList()) {
  Console.WriteLine("Random order: " + player);
}
game.InitDeck();
foreach (UnoCard card in game.GetDeck()) {
  Console.WriteLine(game.GetUnoCardString(card));
}
game.StartGame();
Console.WriteLine("!");