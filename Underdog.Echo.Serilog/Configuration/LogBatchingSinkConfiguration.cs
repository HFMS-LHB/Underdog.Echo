﻿using Underdog.Echo.Common.Helper;
using Underdog.Echo.Serilog.Sink;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Sinks.PeriodicBatching;

namespace Underdog.Echo.Serilog.Configuration
{
    public static class LogBatchingSinkConfiguration
    {
        public static LoggerConfiguration WriteToLogBatching(this LoggerConfiguration loggerConfiguration)
        {
            if (!AppSettings.app("AppSettings", "LogToDb").ObjToBool())
            {
                return loggerConfiguration;
            }

            var exampleSink = new LogBatchingSink();

            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = 500,
                Period = TimeSpan.FromSeconds(1),
                EagerlyEmitFirstEvent = true,
                QueueLimit = 10000
            };

            var batchingSink = new PeriodicBatchingSink(exampleSink, batchingOptions);

            return loggerConfiguration.WriteTo.Sink(batchingSink);
        }
    }
}
