using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.SitecoreApplicationInsights.XConnect.Diagnostics.Initializers
{
  internal class XConnectRoleInstanceInitializer : ITelemetryInitializer
  {
    private readonly IAppInsightsConfigurationOptions _options;
    private readonly ILogger _logger;

    public XConnectRoleInstanceInitializer(IAppInsightsConfigurationOptions options, ILogger logger)
    {
      Condition.Requires(options, nameof(options)).IsNotNull();
      Condition.Requires(logger, nameof(logger)).IsNotNull();

      _options = options;
      _logger = logger;
    }

    public void Initialize(ITelemetry telemetry)
    {
      var role = ConfigurationManager.AppSettings[_options.RoleNameAppSettingKey];
      if (string.IsNullOrWhiteSpace(role))
      {
        _logger.LogDebug("The role name for the telemetry is not specified. Application Insights SDK will add the default cloud role name automatically.");
      }
      else
      {
        telemetry.Context.Cloud.RoleName = role;
      }
    }
  }
}