using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
