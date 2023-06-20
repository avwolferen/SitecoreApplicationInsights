# Application Insights
Add the instrumentation key to your App_Data\ConnectionStrings.config
    <add name="appinsights.instrumentationkey" connectionString="insert-instrumentation-key-here"/>

Add the secure control channel API key to your web.config, obtain this in the Azure Portal by creating an api key for your Application Insights resource with ONLY the 'Authenticate SDK control channel' permissions checked. 
    <add key="appinsights.controlchannelApiKey" value="{INSERT API KEY HERE}" />



## Azure config files
Copy the following files in this project to your siteroot.
\App_Config\Layers.config
\App_Config\Include\Telemetry\ApplicationInsights.config
\App_Config\Sitecore\Azure\Sitecore.Cloud.ApplicationInsights.config
\App_Config\Sitecore\Azure\Sitecore.EmailExperience.Logging.Azure.config
Their "Build Action" is set to "None" so it won't get accidentally published.

## Copy one single Sitecore dll from a Sitecore 10.2.0 Cloud package
From one of the Cloud packages for Sitecore 10.2.0 copy over the 'Sitecore.Cloud.ApplicationInsights.dll' dll to the \libs\Sitecore\ in this repo.

Publish this project to your local environment and you are good to go!