namespace WebApi.Models;
public class Player {
  public string Name {get; set;}
  public string Icon {get; set;}
  public Player(string name) {
    this.Name = name;
    this.Icon = "";
  }
}