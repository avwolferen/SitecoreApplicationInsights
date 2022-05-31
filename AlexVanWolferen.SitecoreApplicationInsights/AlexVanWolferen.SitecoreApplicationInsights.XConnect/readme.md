## XConnect configuration

Add the following appsettings to your appsettings.config for all XConnect roles

  <add key="AppInsightsKey" value="" />
  <add key="AppInsightsRoleName" value="" />
  <add key="AppInsightsControlChannelApiKey" value="" />
  <add key="AppInsightsLiveEndpoint" value="" />
  <add key="AppInsightsIngestionEndpoint" value="" />


# Live and ingestion endpoint

Regional ingestion endpoints. Find your endpoints in the Azure Portal at your Application Insights -> Configure -> Properties
Regions that require custom endpoint can be found at https://docs.microsoft.com/en-us/azure/azure-monitor/app/custom-endpoints?tabs=net#regions-that-require-endpoint-modification
Using a regional endpoint also ensures that data doesn't leave a geographic region.
Defaults when these settings are not set are the global endpoints.
- AppInsightsIngestionEndpoint : https://dc.applicationinsights.azure.com
- AppInsightsLiveEndpoint      : https://live.applicationinsights.azure.com

The secure control channel API key can be obtained in the Azure Portal by creating an api key for your Application Insights resource with ONLY the 'Authenticate SDK control channel' permissions checked.