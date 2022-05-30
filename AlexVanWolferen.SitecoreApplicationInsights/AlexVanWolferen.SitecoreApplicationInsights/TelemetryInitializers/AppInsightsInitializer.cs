using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
using Sitecore;
using Sitecore.Cloud.ApplicationInsights;
using Sitecore.Configuration;
using Sitecore.Pipelines;
using System;
using System.Configuration;

namespace AlexVanWolferen.SitecoreApplicationInsights.TelemetryInitializers
{
  public class AppInsightsInitializer
  {
    public const string DefaultConnectionStringName = "appinsights.instrumentationkey";
    public const string IngestionEndpointSettingName = "appinsights.ingestionEndpoint";
    public const string LiveEndpointSettingName = "appinsights.liveEndpoint";

    public virtual void Process(PipelineArgs args)
    {
      string instrumentationKey = ConfigurationManager.ConnectionStrings[DefaultConnectionStringName].ConnectionString;
      string ingestionEndpoint = Settings.GetSetting(IngestionEndpointSettingName, "https://dc.applicationinsights.azure.com");
      string liveEndpoint = Settings.GetSetting(LiveEndpointSettingName, "https://live.applicationinsights.azure.com");
      bool developerMode = bool.Parse(Settings.GetSetting("ApplicationInsights.DeveloperMode", bool.FalseString));

      Initialize(instrumentationKey, ingestionEndpoint, liveEndpoint, developerMode);
    }

    public virtual void Initialize(string instrumentationKey, string ingestionEndpoint, string liveEndpoint, bool developerMode)
    {
      if (string.IsNullOrWhiteSpace(instrumentationKey))
      {
        throw new AppInsightsException("Application Insights instrumentationKey connection string is not set");
      }

      if (string.IsNullOrWhiteSpace(ingestionEndpoint))
      {
        throw new AppInsightsException($"Application Insights {IngestionEndpointSettingName} setting is not set");
      }

      if (string.IsNullOrWhiteSpace(liveEndpoint))
      {
        throw new AppInsightsException($"Application Insights {LiveEndpointSettingName} setting is not set");
      }

      if (!Uri.IsWellFormedUriString(ingestionEndpoint, UriKind.Absolute))
      {
        throw new AppInsightsException($"Application Insights {IngestionEndpointSettingName} is not a valid absolute Uri");
      }

      if (!Uri.IsWellFormedUriString(liveEndpoint, UriKind.Absolute))
      {
        throw new AppInsightsException($"Application Insights {LiveEndpointSettingName} is not a valid absolute Uri");
      }

      string connectionString = $"InstrumentationKey={instrumentationKey};IngestionEndpoint={ingestionEndpoint};LiveEndpoint={liveEndpoint}";

      TelemetryConfiguration.Active.ConnectionString = connectionString;
      TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = developerMode;

      var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();
      applicationIdProvider.ProfileQueryEndpoint = $"{StringUtil.EnsurePostfix('/', ingestionEndpoint)}api/profiles/{0}/appId";

      TelemetryConfiguration.Active.ApplicationIdProvider = applicationIdProvider;
    }
  }
}