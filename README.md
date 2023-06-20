# Upgrading Application Insights SDK in Sitecore

This repository provides instructions for upgrading the Application Insights SDK in Sitecore, including xConnect, from version 2.5.1 to 2.21.0. Additionally, it highlights how the solution enables control channel for live stream filtering.

## Overview

The Application Insights SDK provides powerful monitoring and analytics capabilities for Sitecore applications. This upgrade guide focuses on upgrading the SDK version in Sitecore, ensuring compatibility with xConnect, and leveraging the control channel for live stream filtering.

## Prerequisites

Before proceeding with the upgrade, make sure you have the following prerequisites:

- Sitecore solution with an existing Application Insights integration
- Access to your codebase
- NuGet package manager

## Upgrade Steps

Follow the steps below to upgrade the Application Insights SDK in your Sitecore solution:

1. **Backup your solution**: Before making any changes, create a backup of your Sitecore solution to avoid data loss or unintended consequences.

2. **Identify dependencies**: Review your solution's codebase and note any customizations or dependencies on the current Application Insights SDK version (2.5.1). Ensure that the new SDK version (2.21.0) is compatible with these customizations.

3. **Update/Add NuGet package references**: Open your Sitecore solution in Visual Studio and navigate to the solution explorer. Locate the projects that reference the Application Insights SDK (e.g., Web project, xConnect projects) and update the NuGet package references to version 2.21.0. The required packages can be found in the [packages.config of Sitecore roles](AlexVanWolferen.SitecoreApplicationInsights/AlexVanWolferen.SitecoreApplicationInsights/packages.config) and the [packages.config for xConnect](AlexVanWolferen.SitecoreApplicationInsights/AlexVanWolferen.SitecoreApplicationInsights.XConnect/packages.config)

4. **Update configuration files**: Update the configuration files to reflect the changes in the upgraded SDK version. This may include modifying the regional endpoints for Application Insights
```xml
      <!-- Regional ingestion endpoints. Find your endpoints in the Azure Portal at your Application Insights -> Configure -> Properties
           Regions that require custom endpoint can be found at https://docs.microsoft.com/en-us/azure/azure-monitor/app/custom-endpoints?tabs=net#regions-that-require-endpoint-modification
           Using a regional endpoint also ensures that data doesn't leave a geographic region.
           Defaults when these settings are not set are the global endpoints.
            ingestionEndpoint : https://dc.applicationinsights.azure.com
            liveEndpoint      : https://live.applicationinsights.azure.com
      -->
      <setting name="appinsights.ingestionEndpoint" value="https://westeurope-1.in.applicationinsights.azure.com" />
      <setting name="appinsights.liveEndpoint" value="https://westeurope.livediagnostics.monitor.azure.com" />
```

5. **Test and verify**: Rebuild your solution and execute a series of tests to ensure that the upgraded Application Insights SDK is functioning as expected. Verify that the control channel for live stream filtering is working correctly with your solution. Configuring the SDK Control Channel Key can be done in the appSettings section of your web.config (or on your appService/keyvault or wherever you put your settings).
```xml
    <add key="appinsights.controlchannelApiKey" value="YOUR_CONTROLCHANNEL_APIKEY_HERE" />
``` 

6. **Deploy the upgraded solution**: Once you have verified the upgrade in your local environment, deploy the upgraded solution to your staging or production environment. Follow your organization's deployment procedures and best practices.

## Control Channel for Live Stream Filtering

With this upgraded version of the Application Insights SDK, you can leverage the control channel feature for live stream filtering. This feature allows you to dynamically filter telemetry data, improving the efficiency of data collection and reducing noise in your monitoring. Implement the necessary logic in your solution to control the telemetry data sent to Application Insights.

## Contributing

Contributions to this repository are welcome!

## Acknowledgments

Special thanks to the Sitecore community and the contributors of the Application Insights SDK for their ongoing support and continuous improvement of the integration.
