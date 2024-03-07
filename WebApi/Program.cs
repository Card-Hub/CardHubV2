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
//Console.WriteLine(game.GetPlayerList());
//foreach (string player in game.GetPlayerList()) {
//  Console.WriteLine(player);
//}
//Console.WriteLine("~~!");
// foreach (UnoCard card in game.GetDeck()) {
//   Console.WriteLine(game.GetUnoCardString(card));
// }
//game.ShuffleDeck();
// Console.WriteLine("LIST OF CARDS IN THE DECK:");
// foreach (UnoCard card in game.GetDeck()) {
//   Console.WriteLine("\t" + game.GetUnoCardString(card));
// }
game.ShuffleDeck();
game.StartGame();
Console.WriteLine("RANDOM PLAYER ORDER:");
foreach (string player in game.GetPlayerList()) {
  Console.WriteLine("\t" + player);
}
Console.WriteLine("PLAYER CARDS:");
foreach (string name in game.GetPlayerList())
{
  Console.WriteLine("\t" + name + "'s cards:");
  foreach (UnoCard card in game.GetPlayerHand(name))
  {
    Console.WriteLine("\t\t" + game.GetUnoCardString(card));
  }
}
int playerTurnNum = 0;
Console.WriteLine("GAME START!");
List<UnoCard> playerHand = new List<UnoCard>();
string playerName = "";
while (game.IsOngoing()) {
  playerName = game.GetCurrentPlayer();
  // person is going to try and play the game.
  Console.WriteLine(playerTurnNum.ToString() + ". " + playerName + "'s turn.");
  // try and play a card
  playerHand = game.GetPlayerHand(playerName);
  if (!game.PlayerHasPlayableCard(playerName)) {Console.WriteLine("No playable cards!"); game.DrawCard(playerName); game.NextTurn();}
  bool playedACard = false;
  for (int i = 0; i < playerHand.Count; i++) {
    if (!playedACard && game.CardCanBePlayed(playerHand.ElementAt(i))) {
      playedACard = true;
      game.PlayCard(playerName, playerHand.ElementAt(i));
      if (game.PlayerNeedsToPickWildColor(playerName)) {
        // get the first non-black color in the hand
        string newWildColor = "red";
        foreach (UnoCard card in game.GetPlayerHand(playerName)) {
          if (card.color != "black") {
            newWildColor = card.color;
          }
          // set that color as the new wild color
          game.SetWildColor(playerName, newWildColor);
          break;
        }
        // game.NextTurn();
      }
    }
  }
  string endTurnStr = "Card totals: ";
  foreach (string name in game.GetPlayerList()) {
    endTurnStr += name + ": " + game.GetPlayerHand(name).Count();
  }
  Console.WriteLine(endTurnStr);

  playerTurnNum ++;
}
Console.WriteLine(playerName + " won!");