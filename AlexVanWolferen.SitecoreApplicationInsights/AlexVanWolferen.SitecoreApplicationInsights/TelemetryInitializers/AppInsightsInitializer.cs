using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Sitecore;
using Sitecore.Cloud.ApplicationInsights;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using System;
using System.Configuration;

namespace AlexVanWolferen.SitecoreApplicationInsights.TelemetryInitializers
{
  public class AppInsightsInitializer
  {
    public const string InstrumentationKeyConnectionStringName = "appinsights.instrumentationkey";
    public const string ControlChannelApiKeySettingName = "appinsights.controlchannelApiKey";
    public const string IngestionEndpointSettingName = "appinsights.ingestionEndpoint";
    public const string DefaultIngestionEndpoint = "https://dc.applicationinsights.azure.com";
    public const string LiveEndpointSettingName = "appinsights.liveEndpoint";
    public const string DefaultLiveEndpoint = "https://live.applicationinsights.azure.com";
    public const string DeveloperModeSettingName = "ApplicationInsights.DeveloperMode";
    public const string RoleDefineSettingName = "role:define";

    public virtual void Process(PipelineArgs args)
    {
      var instrumentationKey = ConfigurationManager.ConnectionStrings[InstrumentationKeyConnectionStringName].ConnectionString;
      var controlChannelApiKey = ConfigurationManager.AppSettings[ControlChannelApiKeySettingName] ?? string.Empty;
      var ingestionEndpoint = Settings.GetSetting(IngestionEndpointSettingName, DefaultIngestionEndpoint);
      var liveEndpoint = Settings.GetSetting(LiveEndpointSettingName, DefaultLiveEndpoint);
      var developerMode = bool.Parse(Settings.GetSetting(DeveloperModeSettingName, bool.FalseString));
      var role = ConfigurationManager.AppSettings[RoleDefineSettingName] ?? string.Empty;

      Initialize(instrumentationKey, controlChannelApiKey, ingestionEndpoint, liveEndpoint, developerMode, role);
    }

    public virtual void Initialize(string instrumentationKey, string controlChannelApiKey, string ingestionEndpoint, string liveEndpoint, bool developerMode, string role)
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

      #region ApplicationIdProvider
      var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();
      applicationIdProvider.ProfileQueryEndpoint = $"{StringUtil.EnsurePostfix('/', ingestionEndpoint)}api/profiles/{{0}}/appId";

      TelemetryConfiguration.Active.ApplicationIdProvider = applicationIdProvider;
      #endregion

      #region QuickPulseTelemetryModule

      QuickPulseTelemetryProcessor processor = null;

      TelemetryConfiguration configuration = TelemetryConfiguration.Active;
      if (configuration == null)
      {
        return;
      }

      configuration.TelemetryProcessorChainBuilder
          .Use((next) =>
          {
            processor = new QuickPulseTelemetryProcessor(next);
            return processor;
          })
          .Build();

      var QuickPulse =
          string.IsNullOrWhiteSpace(controlChannelApiKey)
              ? new QuickPulseTelemetryModule()
              : new QuickPulseTelemetryModule()
              {
                AuthenticationApiKey = controlChannelApiKey
              };

      QuickPulse.Initialize(configuration);
      QuickPulse.RegisterTelemetryProcessor(processor);

      if (string.IsNullOrWhiteSpace(controlChannelApiKey))
      {
        Log.Warn("Application Insights is not initialized with a secure control channel API Key, filtering on Live Metrics is not available.", this);
      }

      #endregion


      #region Initialize Cloud Role

      if (!string.IsNullOrEmpty(role))
      {
        TelemetryConfiguration.Active.TelemetryInitializers.Add(new CloudRoleInitializer(role));
      }

      #endregion
    }
  }
}