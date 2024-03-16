using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Models;
namespace WebApi.Models.CardConverter;

public static class CardConverter {
  public static UnoCard ConvertUnoCard(iCard card) {
    Dictionary<string, string> cardVals = card.GetAttributes();
    return new UnoCard(int.Parse(cardVals["Id"]), cardVals["Value"], cardVals["Color"]);
  }
}