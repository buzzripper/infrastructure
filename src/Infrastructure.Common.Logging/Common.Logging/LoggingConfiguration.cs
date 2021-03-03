using System;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace ProData.Infrastructure.Common.Logging
{
    public static class LoggingConfiguration
    {
        private const string ConfigSectionName = "DbLogging";

        public static ILogger CreateGlobalLogger(IConfiguration configuration)
        {
            var builder = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.With(new LoggingEnricher());

            var dbLoggingConfig = configuration.GetSection(ConfigSectionName).Get<DbLoggingConfig>();
            if (dbLoggingConfig != null)
            {
                var connStr = GetConnectionString(configuration, dbLoggingConfig);
                if (string.IsNullOrWhiteSpace(connStr))
                    return null;

                var sinkOptions = BuildSinkOptions(dbLoggingConfig);
                var columnOptions = BuildColumnOptions();

                if (!Enum.TryParse<LogEventLevel>(dbLoggingConfig.MinLevel, out var logEventLevel))
                {
                    logEventLevel = LogEventLevel.Debug;    // Default
                }

                builder.WriteTo.MSSqlServer(
                        connectionString: connStr,
                        sinkOptions: sinkOptions,
                        restrictedToMinimumLevel: logEventLevel,
                        columnOptions: columnOptions
                );
            }
            return builder.CreateLogger();
        }

        private static string GetConnectionString(IConfiguration configuration, DbLoggingConfig dbLoggingConfig)
        {
            string connStr = null;

            if (!string.IsNullOrWhiteSpace(dbLoggingConfig.ConnectionStringName))
            {
                connStr = configuration.GetConnectionString(dbLoggingConfig.ConnectionStringName);
            }
            if (string.IsNullOrWhiteSpace(connStr))
            {
                connStr = dbLoggingConfig.ConnectionString;
            }

            return connStr;
        }

        private static MSSqlServerSinkOptions BuildSinkOptions(DbLoggingConfig dbLoggingConfig)
        {
            var batchPeriod = TimeSpan.FromSeconds(dbLoggingConfig.BatchPeriodSecs > 0 ? dbLoggingConfig.BatchPeriodSecs : 5);
            var schemaName = dbLoggingConfig.SchemaName ?? "dbo";
            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                SchemaName = schemaName,
                AutoCreateSqlTable = false,
                BatchPostingLimit = 1000,
                BatchPeriod = batchPeriod
            };
            return sinkOptions;
        }

        private static ColumnOptions BuildColumnOptions()
        {
            var columnOptions = new ColumnOptions();

            columnOptions.TimeStamp.ConvertToUtc = true;
            columnOptions.TimeStamp.ColumnName = "TimeStamp";
            columnOptions.DisableTriggers = true;
            columnOptions.ClusteredColumnstoreIndex = false;
            columnOptions.Store.Remove(StandardColumn.Id);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Properties);

            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn
                {
                    ColumnName = "LevelValue",
                    DataType = SqlDbType.Int,
                    AllowNull = true,
                    DataLength = 32,
                    NonClusteredIndex = true
                },
                new SqlColumn
                {
                    ColumnName = "AppId",
                    PropertyName = "ApplicationName",
                    DataType = SqlDbType.VarChar,
                    AllowNull = true,
                    DataLength = 50
                },
                new SqlColumn
                {
                    ColumnName = "UserName",
                    PropertyName = "UserName",
                    DataType = SqlDbType.VarChar,
                    AllowNull = true,
                    DataLength = 50
                },
                new SqlColumn
                {
                    ColumnName = "SourceContext",
                    DataType = SqlDbType.VarChar,
                    AllowNull = true,
                    DataLength = 300
                },
                new SqlColumn
                {
                    ColumnName = "CorrelationId",
                    DataType = SqlDbType.VarChar,
                    AllowNull = true,
                    DataLength = 100
                },
                new SqlColumn
                {
                    ColumnName = "MachineName",
                    PropertyName = "MachineName",
                    DataType = SqlDbType.VarChar,
                    AllowNull = true,
                    DataLength = 100
                }
            };

            return columnOptions;
        }
    }
}
