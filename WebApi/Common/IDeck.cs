namespace WebApi.Common;

public interface IDeck<TCard>
{
    void Shuffle();
    TCard Draw();
    TCard Peek();
    List<TCard> DrawUntil(int amount);
    void ReclaimCards();
}