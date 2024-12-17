using Microsoft.Extensions.Configuration;

namespace StudentManager.Models
{
    internal class DBConnection
    {
        public static string? ConnectionString
        {
            get => App.Configuration?.GetConnectionString("MySqlConnection");
        }
    }
}
