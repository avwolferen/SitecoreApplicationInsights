* Information for local development targeting Application Insights

prerequisites

* Application Insights
Add the instrumentation key to your App_Data\ConnectionStrings.config
    <add name="appinsights.instrumentationkey" connectionString="insert-instrumentation-key-here"/>

* Azure config files
Copy the following files in this project to your siteroot.
\App_Config\Layers.config
\App_Config\Sitecore\Azure\Sitecore.Cloud.ApplicationInsights.config
\App_Config\Sitecore\Azure\Sitecore.EmailExperience.Logging.Azure.config
Their "Build Action" is set to "None" so it won't get accidentally published.

* Choose the Solution Configuration
For development purposes the Debug solution configuration is used for local development. This configuration performs the neccesary XDT Transforms on the web.config to add all neccesary Application Insights configurations.

* Copy one single Sitecore dll from a Sitecore 9.3 Cloud package
From one of the Cloud packages for Sitecore 9.3 copy over the 'Sitecore.Cloud.ApplicationInsights.dll' dll to your bin folder.

Publish this project to your local environment and you are good to go!