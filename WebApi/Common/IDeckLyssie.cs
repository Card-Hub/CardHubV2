namespace WebApi.Common;

public interface IDeckLyssie<TCard>
{
    void Add(TCard card);
    void Shuffle();
    TCard Draw();
    TCard Peek();
    List<TCard> Draw(int amount);
    void ReclaimCards();
    List<TCard> GetCards();
}