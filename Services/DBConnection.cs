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
                var connectionString = App.Configuration?.GetConnectionString("MySqlConnection");
                return $"{connectionString};Pooling=true;Min Pool Size=5;Max Pool Size=100;";
            }
        }

        public static MySqlConnection? GetConnection()
        {
            return new(ConnectionString);
        }
    }
}
