namespace example.infrastructure.Configurations
{
    public class ApiConfig
    {
        public static CommonConfig Common;
        public static ConnectionStrings Connection;
    }

    public class CommonConfig
    {
        public string Environment { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnectionString { get; set; }
    }
}
