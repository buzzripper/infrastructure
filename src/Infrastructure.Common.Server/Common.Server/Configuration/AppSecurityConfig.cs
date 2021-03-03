using System.Collections.Generic;

namespace ProData.Infrastructure.Common.Server.Configuration
{
    public class AppSecurityConfig
    {
        public const string ConfigSectionName = "AppSecurity";

        public AppSecurityConfig()
        {
            this.Scopes = new List<string>();
        }

        public string ClientId { get; set; }
        public string Authority { get; set; }
        public List<string> Scopes { get; }
    }
}
