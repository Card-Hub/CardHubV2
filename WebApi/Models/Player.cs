namespace WebApi.Models;

// not used for anything rn

public class Player {
  string Name {get; set;}
  string Icon {get; set;}
  Player(string name) {
    this.Name = name;
    this.Icon = "";
  }
}