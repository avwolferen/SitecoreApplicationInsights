using AlexVanWolferen.SitecoreApplicationInsights.XConnect.Diagnostics.Initializers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.SitecoreApplicationInsights.XConnect.Diagnostics
{
  internal class AppInsightsInitializer : Sitecore.XConnect.DependencyInjection.Abstractions.IApplicationInitializer
  {
    private readonly IAppInsightsConfigurationOptions _options;

    private readonly ILogger _logger;

    public int Order => 1;

    public AppInsightsInitializer(IAppInsightsConfigurationOptions options, ILogger logger)
    {
      Condition.Requires(options, nameof(options)).IsNotNull();
      Condition.Requires(logger, nameof(logger)).IsNotNull();

      _options = options;
      _logger = logger;
    }

    public void Initialize()
    {
      var instrumentationKey = ConfigurationManager.AppSettings[_options.InstrumentationKeyAppSettingKey];
      var controlchannelKey = ConfigurationManager.AppSettings[_options.ControlChannelApiKeyAppSettingKey];
      var liveEndpoint = ConfigurationManager.AppSettings[_options.LiveEndpointAppSettingKey];
      var ingestionEndpoint = ConfigurationManager.AppSettings[_options.IngestionEndpointAppSettingKey];

      if (string.IsNullOrEmpty(instrumentationKey))
      {
        _logger.LogInformation($"Application Insights {nameof(instrumentationKey)} is not set");
        return;
      }

      if (string.IsNullOrEmpty(controlchannelKey))
      {
        _logger.LogInformation($"Application Insights {nameof(controlchannelKey)} is not set. QuickPulse will not be authenticated.");
      }

      if (string.IsNullOrEmpty(liveEndpoint))
      {
        liveEndpoint = "https://live.applicationinsights.azure.com";
        _logger.LogInformation($"Application Insights {nameof(liveEndpoint)} is not set. Default '{liveEndpoint}' will be used.");
      }

      if (string.IsNullOrEmpty(ingestionEndpoint))
      {
        ingestionEndpoint = "https://dc.applicationinsights.azure.com";
        _logger.LogInformation($"Application Insights {nameof(ingestionEndpoint)} is not set. Default '{ingestionEndpoint}' will be used.");
      }

      if (!Uri.IsWellFormedUriString(liveEndpoint, UriKind.Absolute))
      {
        _logger.LogInformation($"Application Insights {nameof(liveEndpoint)} is not a absolute uri");
        return;
      }

      if (!Uri.IsWellFormedUriString(ingestionEndpoint, UriKind.Absolute))
      {
        _logger.LogInformation($"Application Insights {nameof(ingestionEndpoint)} is not a absolute uri");
        return;
      }

      string connectionstring = $"InstrumentationKey={instrumentationKey};IngestionEndpoint={ingestionEndpoint};LiveEndpoint={liveEndpoint}";
      TelemetryConfiguration.Active.ConnectionString = connectionstring;

      var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();
      applicationIdProvider.ProfileQueryEndpoint = $"{StringUtil.EnsurePostfix('/', ingestionEndpoint)}api/profiles/{{0}}/appId";
      TelemetryConfiguration.Active.ApplicationIdProvider = applicationIdProvider;

      TelemetryConfiguration.Active.TelemetryInitializers.Add(new XConnectRoleInstanceInitializer(_options, _logger));

      // Initialize the QuickPulse Telemetry for Secure LiveStream
      QuickPulseTelemetryProcessor processor = null;

      TelemetryConfiguration.Active.TelemetryProcessorChainBuilder
      .Use((next) =>
      {
        processor = new QuickPulseTelemetryProcessor(next);
        return processor;
      })
      .Build();

      var quickPulse = string.IsNullOrWhiteSpace(controlchannelKey)
      ? new QuickPulseTelemetryModule()
      : new QuickPulseTelemetryModule() { AuthenticationApiKey = controlchannelKey };

      quickPulse.Initialize(TelemetryConfiguration.Active);
      quickPulse.RegisterTelemetryProcessor(processor);
    }
  }
}