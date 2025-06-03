using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shopify_Manager.ErrorLogging
{
    public class AppErrorLog
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string DeviceName { get; set; }
        public string WindowsUser { get; set; }
        public string IPAddress { get; set; }
        public string FormName { get; set; } // New field
    }


    public static class ErrorLogger
    {
        private static readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.db");

        static ErrorLogger()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();

                string createTable = @"
                CREATE TABLE IF NOT EXISTS AppErrorLog (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp TEXT,
                    Message TEXT,
                    StackTrace TEXT,
                    DeviceName TEXT,
                    WindowsUser TEXT,
                    IPAddress TEXT
                )";

                using (var cmd = new SQLiteCommand(createTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Log(Exception ex)
        {
            var error = new AppErrorLog
            {
                Timestamp = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                DeviceName = Environment.MachineName,
                WindowsUser = Environment.UserName,
                IPAddress = GetLocalIPAddress()
            };

            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();

                string insertQuery = @"
                INSERT INTO AppErrorLog 
                (Timestamp, Message, StackTrace, DeviceName, WindowsUser, IPAddress) 
                VALUES 
                (@Timestamp, @Message, @StackTrace, @DeviceName, @WindowsUser, @IPAddress)";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Timestamp", error.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Message", error.Message);
                    cmd.Parameters.AddWithValue("@StackTrace", error.StackTrace);
                    cmd.Parameters.AddWithValue("@DeviceName", error.DeviceName);
                    cmd.Parameters.AddWithValue("@WindowsUser", error.WindowsUser);
                    cmd.Parameters.AddWithValue("@IPAddress", error.IPAddress);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetLocalIPAddress()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                          .AddressList
                          .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

    }
}
