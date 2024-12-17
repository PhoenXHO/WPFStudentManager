using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace StudentManager.Models
{
    internal class DBConnection
    {
        public static string? ConnectionString
        {
            get => App.Configuration?.GetConnectionString("MySqlConnection");
        }

        public static MySqlConnection? GetConnection()
        {
            return new(ConnectionString);
        }
    }
}
