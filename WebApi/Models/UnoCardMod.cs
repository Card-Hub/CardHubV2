using System.Text.RegularExpressions;

namespace WebApi.Models;

public enum UnoColor
{
    Blue,
    Green,
    Red,
    Yellow,
    Black
}

public enum UnoValue
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    DrawTwo,
    Reverse,
    Skip,
    SkipAll,
    Wild,
    WildDrawFour
}

public partial class UnoCardMod
{
    public int Id { get; }
    public UnoColor Color { get; init; }
    public UnoValue Value { get; }

    private static List<UnoValue> specialValues =
        [UnoValue.DrawTwo, UnoValue.Reverse, UnoValue.Skip, UnoValue.SkipAll, UnoValue.Wild, UnoValue.WildDrawFour];

    public UnoCardMod(int id, UnoColor color, UnoValue value)
    {
        Id = id;
        Color = color;
        Value = value;
    }

    public object ExtractValue()
    {
        if (specialValues.Contains(Value))
        {
            return AddWhitespaceToCamelcase(Value.ToString());
        }

        return new
        {
            Id,
            Color = Color.ToString(),
            Value = (int)Value
        };
    }

    public string AddWhitespaceToCamelcase(string str)
    {
        return Regex.Replace(str, "(?<!^)([A-Z])", " $1");
    }

    public override bool Equals(object? obj)
    {
        if (obj is UnoCardMod card)
        {
            return Color == card.Color &&
                   Value == card.Value;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Color, Value);
    }
}