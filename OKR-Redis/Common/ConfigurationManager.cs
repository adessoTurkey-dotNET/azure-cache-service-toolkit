namespace OKR_Redis.Common
{
    internal class ConfigurationManager
    {
        private static ConfigurationManager instance;
        public static ConfigurationManager Instance => instance ?? (instance = new ConfigurationManager());

        private ConfigurationManager()
        {
            this.APPINSIGHTS_INSTRUMENTATIONKEY = Util.GetConfig("APPINSIGHTS_INSTRUMENTATIONKEY", "");
            this.CacheConnection = Util.GetConfig("CacheConnection", "");
        }
        public string CacheConnection { get; set; }
        public string APPINSIGHTS_INSTRUMENTATIONKEY { get; private set; }

    }
}
