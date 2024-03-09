namespace WebApi.Models;

public class UnoCard
{
    public int id { get; set; }
    public string value { get; set; }
    public string color { get; set; }
    public override string ToString() => $"{{ id: {id}, value: {value}, color: {color} }}";
}