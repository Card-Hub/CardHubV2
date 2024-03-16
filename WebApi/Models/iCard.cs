namespace WebApi.Models;

  public interface iCard {
    public string GetCardType();
    public Dictionary<string, string> GetAttributes();
    public string GetAttribute(string attribute);
    public void SetAttribute(string attribute, string value);
  }
