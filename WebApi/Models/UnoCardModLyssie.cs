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

public class UnoCardModLyssie : IEquatable<UnoCardModLyssie>
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
        List<UnoValueLyssie> integerEnums = new() 
        {
          UnoValueLyssie.Zero,
          UnoValueLyssie.One,
          UnoValueLyssie.Two,
          UnoValueLyssie.Three,
          UnoValueLyssie.Four,
          UnoValueLyssie.Five,
          UnoValueLyssie.Six,
          UnoValueLyssie.Seven,
          UnoValueLyssie.Eight,
          UnoValueLyssie.Nine
        };
        if (integerEnums.Contains(ValueEnum)) {
          return ((int) ValueEnum).ToString();
        }
        else {
          return MiscFunctions.AddWhitespaceToCamelcase(ValueEnum.ToString());
        }
      }
    }

    private static List<UnoValueLyssie> specialValues =
        [UnoValueLyssie.DrawTwo, UnoValueLyssie.Reverse, UnoValueLyssie.Skip, UnoValueLyssie.SkipAll, UnoValueLyssie.Wild, UnoValueLyssie.WildDrawFour];

    public UnoCardModLyssie (int id, UnoColorLyssie color, UnoValueLyssie value)
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
    public bool Equals(UnoCardModLyssie other)
    {
        // Would still want to check for null etc. first.
        return this.Id == other.Id;
    }
}