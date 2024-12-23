using dotenv.net;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace StudentManager.Services
{
    internal class DBConnection
    {
        public static string? ConnectionString
        {
            get
            {
                DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
                var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");

                if (string.IsNullOrEmpty(mysqlUrl))
                    throw new InvalidOperationException("MYSQL_URL environment variable not found");

                // Parse URL components
                var uri = new Uri(mysqlUrl);
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";
                var database = uri.AbsolutePath.TrimStart('/');

                // Build connection string
                return $"Server={uri.Host};" +
                       $"Port={uri.Port};" +
                       $"Database={database};" +
                       $"Uid={username};" +
                       $"Pwd={password};" +
                       "Pooling=true;" +
                       "Min Pool Size=5;" +
                       "Max Pool Size=100;";
            }
        }

        public static MySqlConnection? GetConnection()
        {
            return new(ConnectionString);
        }
    }
}
