using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Sitecore.Configuration;
using Sitecore.Pipelines;
using System.Configuration;

namespace AlexVanWolferen.SitecoreApplicationInsights.TelemetryModules
{
  public class QuickPulseTelemetryModule
  {

    public virtual void Process(PipelineArgs args)
    {
      string controlChannelApiKey = ConfigurationManager.AppSettings["appinsights.controlchannelApiKey"] ?? string.Empty;
      Initialize(controlChannelApiKey);
    }

    public virtual void Initialize(string apikey)
    {
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
          string.IsNullOrWhiteSpace(apikey)
              ? new Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule()
              : new Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule()
              {
                AuthenticationApiKey = apikey
              };

      QuickPulse.Initialize(configuration);
      QuickPulse.RegisterTelemetryProcessor(processor);
    }
  }
}