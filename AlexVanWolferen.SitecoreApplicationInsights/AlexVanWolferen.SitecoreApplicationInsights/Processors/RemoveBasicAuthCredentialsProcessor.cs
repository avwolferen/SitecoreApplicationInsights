using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AlexVanWolferen.SitecoreApplicationInsights.Processors
{
  public class RemoveBasicAuthCredentialsProcessor
  {
    private string BasicAuthRegex = "^(?<protocol>.+?//)(?<username>.+?):(?<password>.+?)@(?<address>.+)$";

    private ITelemetryProcessor Next { get; set; }

    public RemoveBasicAuthCredentialsProcessor(ITelemetryProcessor next)
    {
      Next = next;
    }

    public void Process(ITelemetry item)
    {
      if (item is DependencyTelemetry dependency)
      {
        if (string.Equals(dependency.Type, "http", StringComparison.OrdinalIgnoreCase))
        {
          Regex regex = new Regex(BasicAuthRegex) ;
          Match matches = regex.Match(dependency.Data);
          if (matches.Success)
          {
            dependency.Data = dependency.Data.Replace(matches.Groups["username"].Value, "****");
            dependency.Data = dependency.Data.Replace(matches.Groups["password"].Value, "****");
          }
        }
      }

      Next.Process(item);
    }
  }
}