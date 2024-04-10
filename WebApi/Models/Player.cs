using StackExchange.Redis;
using Newtonsoft.Json;

namespace WebApi.Models;
public class Player {
  [JsonProperty(Order = -2)]
  public string Name {get; set;}
  [JsonProperty(Order = -2)]
  public string Avatar {get; set;}
  public Player(string name) {
    this.Name = name;
    this.Avatar = "";
  }
}