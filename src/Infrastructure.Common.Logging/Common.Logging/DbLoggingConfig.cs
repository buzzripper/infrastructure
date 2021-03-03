
namespace ProData.Infrastructure.Common.Logging
{
    public class DbLoggingConfig
    {
        public string ConnectionStringName { get; set; }
        public string ConnectionString { get; set; }
        public int BatchPeriodSecs { get; set; }
        public string SchemaName { get; set; }
        public string MinLevel { get; set; }
    }
}
