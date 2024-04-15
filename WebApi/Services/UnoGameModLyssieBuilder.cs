using WebApi.GameLogic.LyssieUno;
using WebApi.GameLogic;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Common;

public class UnoGameModLyssieBuilder {
  private IHubContext<BaseHub> HubContext;
  public UnoGameModLyssieBuilder(IHubContext<BaseHub> hubContext) {
    this.HubContext = hubContext;
  }
  public UnoGameModLyssie BuildGame(UnoDeckBuilderLyssie deckBuilder) {
    var game = new UnoGameModLyssie(new UnoMessenger(HubContext), deckBuilder);
    return game;
  }
}