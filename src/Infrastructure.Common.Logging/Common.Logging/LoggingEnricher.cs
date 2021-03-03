using System;
using Serilog.Core;
using Serilog.Events;

namespace ProData.Infrastructure.Common.Logging
{
    public class LoggingEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LevelValue", (int)logEvent.Level, false));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("MachineName", Environment.MachineName));
        }
    }
}
