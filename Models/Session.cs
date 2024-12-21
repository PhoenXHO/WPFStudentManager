using System;

namespace StudentManager.Models
{
    public class Session
    {
        public static DateTime DefaultExpiry => DateTime.Now.AddHours(6);

        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime Expiry { get; set; }
    }
}
