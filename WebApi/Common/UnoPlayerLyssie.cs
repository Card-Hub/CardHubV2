using System.Text.Json.Serialization;
using WebApi.Models;

namespace WebApi.Common;

public class UnoPlayerLyssie : CardPlayerLyssie<UnoCardModLyssie>
{
  public bool PickingWildColor { get; set; }
  public bool PressedUne { get; set; }
  public bool CanPressUne { get; set; }
    public UnoPlayerLyssie(string name, string connectionString) : base(name, connectionString)
    {
      PickingWildColor = false;
      PressedUne = false;
      CanPressUne = false;
    }
    
  // override object.Equals
  public override bool Equals(object obj)
  {
    //
    // See the full list of guidelines at
    //   http://go.microsoft.com/fwlink/?LinkID=85237
    // and also the guidance for operator== at
    //   http://go.microsoft.com/fwlink/?LinkId=85238
    //
    
    if (obj == null || GetType() != obj.GetType())
    {
      return false;
    }
    var item = obj as UnoPlayerLyssie;
    
    // TODO: write your implementation of Equals() here
    return ConnectionString == item.ConnectionString;
  }
}