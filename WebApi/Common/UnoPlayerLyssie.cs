using System.Text.Json.Serialization;
using WebApi.Models;

namespace WebApi.Common;

public class UnoPlayerLyssie : CardPlayerLyssie<UnoCardModLyssie>
{
  public bool PickingWildColor { get; set; }
    public UnoPlayerLyssie(string name, string connectionString) : base(name, connectionString)
    {
      PickingWildColor = false;
    }
}