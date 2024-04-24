namespace WebApi.Common;

public interface ICardPlayer<TCard>
{
    string Name { get; init; }
    void AddCard(TCard card);
    void AddCards(IEnumerable<TCard> cards);
    List<TCard> GetHand();
    bool RemoveCard(TCard card);
    bool RemoveCards(IEnumerable<TCard> cards);
    void ClearHand();
}