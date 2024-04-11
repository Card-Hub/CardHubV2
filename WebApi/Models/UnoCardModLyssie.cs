using WebApi.Common;
using Newtonsoft.Json;
namespace WebApi.Models;

public enum UnoColorLyssie
{
    Blue,
    Green,
    Red,
    Yellow,
    Black
}

public enum UnoValueLyssie
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

public class UnoCardModLyssie
{
    public int Id { get; }
    [JsonIgnore]
    public UnoColorLyssie ColorEnum { get; }
    [JsonIgnore]
    public UnoValueLyssie ValueEnum { get; }
    // for json and extract value purposes
    public string Color {
      get { return ColorEnum.ToString(); }
    }
    public string Value {
      get { 
        return MiscFunctions.AddWhitespaceToCamelcase(ValueEnum.ToString());
      }
    }

    private static List<UnoValueLyssie> specialValues =
        [UnoValueLyssie.DrawTwo, UnoValueLyssie.Reverse, UnoValueLyssie.Skip, UnoValueLyssie.SkipAll, UnoValueLyssie.Wild, UnoValueLyssie.WildDrawFour];

    public UnoCardModLyssie(int id, UnoColorLyssie color, UnoValueLyssie value)
    {
        Id = id;
        ColorEnum = color;
        ValueEnum = value;
    }

    

    // not used for anything rn but this feels like it has purpose
    public override int GetHashCode()
    {
        return HashCode.Combine(Color, Value);
    }
}