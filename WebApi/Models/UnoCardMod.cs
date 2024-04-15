using System.Text.RegularExpressions;

namespace WebApi.Models;

public enum UnoColor
{
    Black,
    Blue,
    Green,
    Red,
    Yellow
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

// Class with enum member requires empty constructor/getters & setters for serialization
// https://stackoverflow.com/a/66991853/18790415
public record UnoCardMod
{
    public int Id { get; set; }
    public UnoColor Color { get; set; }
    public UnoValue Value { get; set; }

    // private static List<UnoValue> specialValues =
    //     [UnoValue.DrawTwo, UnoValue.Reverse, UnoValue.Skip, UnoValue.SkipAll, UnoValue.Wild, UnoValue.WildDrawFour];

    public UnoCardMod()
    {
    }
    
    public UnoCardMod(int id, UnoColor color, UnoValue value)
    {
        Id = id;
        Color = color;
        Value = value;
    }

    public override string ToString() => $"Color: {Color}, Value: {Value}";
}
