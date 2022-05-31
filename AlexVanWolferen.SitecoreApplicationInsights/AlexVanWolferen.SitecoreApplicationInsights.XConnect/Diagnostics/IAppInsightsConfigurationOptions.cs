namespace AlexVanWolferen.SitecoreApplicationInsights.XConnect.Diagnostics
{
  internal interface IAppInsightsConfigurationOptions
  {
    string InstrumentationKeyAppSettingKey { get; }

    string RoleNameAppSettingKey { get; }

    string ControlChannelApiKeyAppSettingKey { get; }

    string LiveEndpointAppSettingKey { get; }

    string IngestionEndpointAppSettingKey { get; }
  }
}