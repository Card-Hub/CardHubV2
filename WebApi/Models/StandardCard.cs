namespace WebApi.Models;

public class StandardCard
{
    public StandardCard() {
      this.Id = 0;
      this.Suit = "";
      this.Value = "";
    }
    public StandardCard(int id, string suit, string value) : this() {
      this.Id = id;
      this.Suit = suit;
      this.Value = value;
    }
    public int Id { get; set; }
    public string Suit { get; set; }
    public string Value { get; set; }

    // manually doing json is a red flag but oh well lmao
    public string GetValues()
    {
      string innerJson = "";
      innerJson += $"\"id\":  \"{this.Id}\",";
      innerJson += $"\"suit\": \"{this.Suit}\"" + ",";
      innerJson += $"\"value\": \"{this.Value}\"";
      return $"{{{innerJson}}}";
    }
}