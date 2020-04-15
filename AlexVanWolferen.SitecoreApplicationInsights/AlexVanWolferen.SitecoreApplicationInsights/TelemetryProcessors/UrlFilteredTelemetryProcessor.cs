using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.SitecoreApplicationInsights.TelemetryProcessors
{
    /// <summary>
    /// Do not send the telemetry when the operation is excluded.
    /// Filtering is done for every implementation of ITelemtry (requests, dependencies, traces, etc.)
    /// </summary>
    public class UrlFilteredTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        public IList<string> ExcludeOperations { get; } = new List<string>();

        // next will point to the next TelemetryProcessor in the chain.
        public UrlFilteredTelemetryProcessor(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            // To filter out an item, return without calling the next processor.
            if (!OKtoSend(item)) { 
                return;
            }

            this.Next.Process(item);
        }

        private bool OKtoSend(ITelemetry item)
        {
            var operationContext = item?.Context?.Operation;
            if (operationContext == null)
            {
                return true; 
            }

            if (!string.IsNullOrWhiteSpace(operationContext.Name) && this.ExcludeOperations.Contains(operationContext.Name))
            {
                return false;
            }

            return true;
        }
    }
}