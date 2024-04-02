namespace WebApi.Models;
public class Player {
  string Name {get; set;}
  string Icon {get; set;}
  public Player(string name) {
    this.Name = name;
    this.Icon = "";
  }
}