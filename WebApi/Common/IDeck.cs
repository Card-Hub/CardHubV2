namespace WebApi.Common;

public interface IDeck<TCard>
{
    void Add(TCard card);
    void Shuffle();
    TCard Draw();
    TCard Peek();
    List<TCard> Draw(int amount);
    void ReclaimCards();
}