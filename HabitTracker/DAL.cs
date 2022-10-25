using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal class DAL : IDisposable
    {
        static string connectionString = @"Data Source=habit_tracker.db";
        protected SqliteConnection? conn = null;
        public DAL()
        {
            conn = new SqliteConnection(connectionString);
            TryOpenConnection(conn);
        }

        private static void TryOpenConnection(SqliteConnection conn)
        {
            try
            {
                conn.Open();
                Console.WriteLine("Connected to database successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CreateMainTableIfMissing()
        {
            string sql = "CREATE TABLE IF NOT EXISTS habit (" +
                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                         "date TEXT NOT NULL, " +
                         "quantity INTEGER NOT NULL)";
            SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            if (conn != null) { conn.Close(); }
        }
    }
}
