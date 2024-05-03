using System.Text.Json.Serialization;

namespace WebApi.Models;

public enum CahCardType
{
    White,
    Black
}

public class CahCard
{
    [JsonIgnore] private string _text;
    [JsonIgnore] private CahCardType? _type;

    [JsonPropertyName("text")]
    public string Text
    {
        get => _text;
        set
        {
            if (value == null) return;
            if (value.Contains('_'))
            {
                if (!IsValidBlackText(value))
                {
                    throw new ArgumentException("Black cards must have 0, 1, 2, or 3 underscores");
                }
                
                if (Type == CahCardType.White)
                {
                    throw new ArgumentException("White cards cannot contain underscores");
                }
                
            }

            _text = value;
        }
    }

    [JsonIgnore]
    public CahCardType? Type
    {
        get => _type;
        set
        {
            if (string.IsNullOrEmpty(Text)) return;
            if (Text.Contains('_') && value == CahCardType.White)
                throw new ArgumentException("White cards cannot contain underscores");

            _type = value;
        }
    }

    public CahCard() { }

    public CahCard(string text, CahCardType type)
    {
        Text = text;
        Type = type;
    }

    public int PickAmount
    {
        get
        {
            // if (Type != CahCardType.Black) throw new InvalidOperationException("Pick amount is only for black cards");
            if (Type != CahCardType.Black) return -1;
            
            // If the card has no underscores, it's a regular black card with a pick amount of 1
            var underscores = Text.Count(c => c == '_');
            return underscores == 0 ? 1 : underscores;
        }
    }

    private static bool IsValidBlackText(string text)
    {
        return text.Length - text.Replace("_", "").Length is 0 or 1 or 2 or 3;
    }
}