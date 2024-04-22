using System.Text.RegularExpressions;
namespace WebApi.Common;

public static class MiscFunctions {
  public static string AddWhitespaceToCamelcase (string str)
    {
        return Regex.Replace(str, "(?<!^)([A-Z])", " $1");
    }
}