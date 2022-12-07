using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.SitecoreApplicationInsights.XConnect
{
  public class StringUtil
  {
    public static string EnsurePostfix(char postfix, string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return postfix.ToString();
      }
      if (value[value.Length - 1] == postfix)
      {
        return value;
      }
      return value + postfix.ToString();
    }
  }
}