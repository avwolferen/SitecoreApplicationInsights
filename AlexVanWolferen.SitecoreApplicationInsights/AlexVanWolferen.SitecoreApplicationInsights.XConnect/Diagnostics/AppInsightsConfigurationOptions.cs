using Microsoft.Extensions.Configuration;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.SitecoreApplicationInsights.XConnect.Diagnostics
{
  public class AppInsightsConfigurationOptions : IAppInsightsConfigurationOptions
  {
    public string InstrumentationKeyAppSettingKey { get; }
    public string RoleNameAppSettingKey { get; }

    public string ControlChannelApiKeyAppSettingKey { get; }

    public string LiveEndpointAppSettingKey { get; }

    public string IngestionEndpointAppSettingKey { get; }

    public AppInsightsConfigurationOptions(string instrumentationKeyAppSetting, string roleNameKeyAppSetting, string controlChannelApiKeyAppSetting, string liveEndpointAppSetting, string ingestionEndpointAppSetting)
    {
      Condition.Requires(instrumentationKeyAppSetting, nameof(instrumentationKeyAppSetting)).IsNotNullOrEmpty();
      Condition.Requires(roleNameKeyAppSetting, nameof(roleNameKeyAppSetting)).IsNotNullOrEmpty();
      Condition.Requires(controlChannelApiKeyAppSetting, nameof(controlChannelApiKeyAppSetting));
      Condition.Requires(liveEndpointAppSetting, nameof(liveEndpointAppSetting));
      Condition.Requires(ingestionEndpointAppSetting, nameof(ingestionEndpointAppSetting));

      InstrumentationKeyAppSettingKey = instrumentationKeyAppSetting;
      RoleNameAppSettingKey = roleNameKeyAppSetting;
      ControlChannelApiKeyAppSettingKey = controlChannelApiKeyAppSetting;
      LiveEndpointAppSettingKey = liveEndpointAppSetting; ;
      IngestionEndpointAppSettingKey = ingestionEndpointAppSetting;
    }

    public AppInsightsConfigurationOptions(IConfiguration options)
: this(
      Condition.Requires(options).IsNotNull().Value.GetValue<string>(nameof(InstrumentationKeyAppSettingKey)),
      Condition.Requires(options).IsNotNull().Value.GetValue<string>(nameof(RoleNameAppSettingKey)),
      Condition.Requires(options).IsNotNull().Value.GetValue<string>(nameof(ControlChannelApiKeyAppSettingKey)),
      Condition.Requires(options).IsNotNull().Value.GetValue<string>(nameof(LiveEndpointAppSettingKey)),
      Condition.Requires(options).IsNotNull().Value.GetValue<string>(nameof(IngestionEndpointAppSettingKey)))
    {
    }
  }
}