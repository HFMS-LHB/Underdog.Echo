﻿using Underdog.Serilog.Extensions;
using Serilog.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using Underdog.Common;
using Mapster;
using Underdog.Common.Helper;
using Underdog.Model.Logs;
using Serilog.Sinks.PeriodicBatching;

namespace Underdog.Serilog.Sink
{
    public class LogBatchingSink : IBatchedLogEventSink
    {
        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            var sugar = App.GetService<ISqlSugarClient>(false);

            await WriteSqlLog(sugar, batch.FilterSqlLog());
            await WriteLogs(sugar, batch.FilterRemoveOtherLog());
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }

        #region Write Log

        private async Task WriteLogs(ISqlSugarClient db, IEnumerable<LogEvent> batch)
        {
            if (!batch.Any())
            {
                return;
            }

            var group = batch.GroupBy(s => s.Level);
            foreach (var v in group)
            {
                switch (v.Key)
                {
                    case LogEventLevel.Information:
                        await WriteInformationLog(db, v);
                        break;
                    case LogEventLevel.Warning:
                        await WriteWarningLog(db, v);
                        break;
                    case LogEventLevel.Error:
                    case LogEventLevel.Fatal:
                        await WriteErrorLog(db, v);
                        break;
                }
            }
        }

        private async Task WriteInformationLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
        {
            if (!batch.Any())
            {
                return;
            }

            var logs = new List<GlobalInformationLog>();
            foreach (var logEvent in batch)
            {
                var log = logEvent.Adapt<GlobalInformationLog>();
                log.Message = logEvent.RenderMessage();
                log.Properties = logEvent.Properties.ToJson();
                log.DateTime = logEvent.Timestamp.DateTime;
                logs.Add(log);
            }

            await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        }

        private async Task WriteWarningLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
        {
            if (!batch.Any())
            {
                return;
            }

            var logs = new List<GlobalWarningLog>();
            foreach (var logEvent in batch)
            {
                var log = logEvent.Adapt<GlobalWarningLog>();
                log.Message = logEvent.RenderMessage();
                log.Properties = logEvent.Properties.ToJson();
                log.DateTime = logEvent.Timestamp.DateTime;
                logs.Add(log);
            }

            await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        }

        private async Task WriteErrorLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
        {
            if (!batch.Any())
            {
                return;
            }

            var logs = new List<GlobalErrorLog>();
            foreach (var logEvent in batch)
            {
                var log = logEvent.Adapt<GlobalErrorLog>();
                log.Message = logEvent.RenderMessage();
                log.Properties = logEvent.Properties.ToJson();
                log.DateTime = logEvent.Timestamp.DateTime;
                logs.Add(log);
            }

            await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        }

        private async Task WriteSqlLog(ISqlSugarClient db, IEnumerable<LogEvent> batch)
        {
            if (!batch.Any())
            {
                return;
            }

            var logs = new List<AuditSqlLog>();
            foreach (var logEvent in batch)
            {
                var log = logEvent.Adapt<AuditSqlLog>();
                log.Message = logEvent.RenderMessage();
                log.Properties = logEvent.Properties.ToJson();
                log.DateTime = logEvent.Timestamp.DateTime;
                logs.Add(log);
            }

            await db.AsTenant().InsertableWithAttr(logs).SplitTable().ExecuteReturnSnowflakeIdAsync();
        }

        #endregion
    }
}
