using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Sitecore.Framework.Conditions;

namespace AlexVanWolferen.SitecoreApplicationInsights.TelemetryInitializers
{
  internal class CloudRoleInitializer : ITelemetryInitializer
  {
    private readonly string _roleName;
    public CloudRoleInitializer(string roleName)
    {
      Condition.Requires(roleName, nameof(roleName)).IsNotNull();

      _roleName = roleName;
    }
    public void Initialize(ITelemetry telemetry)
    {
      if (telemetry?.Context?.Cloud == null || string.IsNullOrEmpty(_roleName))
      {
        return;
      }

      telemetry.Context.Cloud.RoleName = _roleName;
    }
  }
}