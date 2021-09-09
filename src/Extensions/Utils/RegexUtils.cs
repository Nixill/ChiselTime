using System.Text.RegularExpressions;

namespace Nixill.Utils
{
  public static class RegexUtils2
  {
    public static bool TryGroup(this Match match, int group, out string value)
    {
      bool ret = match.Groups[group].Success;

      if (ret) value = match.Groups[group].Value;
      else value = null;

      return ret;
    }
  }
}