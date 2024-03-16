using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Npgsql.Replication;
namespace WebApi.Models;

public class UnoCard : iCard
{
  public string GetCardType() {return "Uno";}
    private Dictionary<string, string> Attributes {get;set;}

    // Question: why are the getters and setters not just get; set; ?
    // Answer: the setter isn't automatic (set;) because it needs to modify the dictionary
    // and when we don't use an auto setter, we can't use an auto getter (get;)
    // and when you don't use an auto getter, we need a private var to return
    // also: in setter shorthand, value is whatever the var is set to
    // ex. if you use card.Id = 5, then value = 5
    private int _id; // necessary 
    public int Id {
      get {return this._id;}
      set {this._id = value; this.Attributes["Id"] = value.ToString(); }
    }
    private string _value; // necessary 
    public string Value {
      get {return this._value;}
      set {this._value = value; this.Attributes["Value"] = value;}
    }
    private string _color; // necessary
    public string Color {
      get {return this._color;}
      set {this._color = value; this.Attributes["Color"] = value;}
    }
    // Constructors
    public UnoCard() {
      this.Attributes = new Dictionary<string, string>();
      this.Id = 0;
      this._value = ""; // throws warning without this
      this.Value = "";
      this._color = ""; // throws warning without this
      this.Color = "";
    }
    public UnoCard(int id, string value, string color) : this() // this() runs the default constructor
    {
      this.Id = id;
      this.Value = value;
      this.Color = color;
    }
    public Dictionary<string, string> GetAttributes()
    {
      return this.Attributes;
    }

  public void SetAttribute(string attribute, string value)
  {
    switch (attribute) {
      case "Id":
        this.Attributes["Id"] = value;
        this.Id = int.Parse(value);
        break;
      case "Color":
        this.Attributes["Color"] = value;
        this.Color = value;
        break;
      case "Value":
        this.Attributes["Value"] = value;
        this.Value = value;
        break;
      default:
        throw new ArgumentException($"{attribute} is not a valid UnoCard attribute");
      }

    }
  public string GetAttribute(string attribute)
  {
    return this.Attributes[attribute];
  }

    public override string ToString() => $"{{ id: {_id}, value: {_value}, color: {_color} }}";
}