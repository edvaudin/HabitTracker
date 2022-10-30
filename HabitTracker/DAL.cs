﻿using Microsoft.Data.Sqlite;
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

        public void CreateHabit(string name, string measurement)
        {
            string sql = "INSERT INTO habits (name, measurement) VALUES (@name, @measurement);";
            SqliteCommand cmd = new SqliteCommand(sql, conn);
            AddParameter("@name", name, cmd);
            AddParameter("@measurement", measurement, cmd);
            cmd.ExecuteNonQuery();
        }

        public List<Habit> GetHabits()
        {
            string sql = "SELECT * FROM habits;";
            SqliteCommand cmd = new SqliteCommand(sql, conn);
            return GetQueriedList(cmd, reader => new Habit(reader));
        }

        public Habit GetHabit(int id)
        {
            try
            {
                string sql = "SELECT * FROM habits WHERE id = @id;";
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                AddParameter("@id", id, cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Habit(reader);
                    }
                }
                return new Habit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Habit();
            }
        }
        public void AddEntry(string date, int quantity, int habitId)
        {
            string sql = "INSERT INTO tracker (date, quantity, habit_id) VALUES (@date, @quantity, @habit_id);";
            SqliteCommand cmd = new SqliteCommand(sql, conn);
            AddParameter("@date", date, cmd);
            AddParameter("@quantity", quantity, cmd);
            AddParameter("@habit_id", habitId, cmd);
            cmd.ExecuteNonQuery();
        }

        public void DeleteEntry(int id)
        {
            string sql = "DELETE FROM tracker WHERE id = @id;";
            SqliteCommand cmd = new SqliteCommand(sql, conn);
            AddParameter("@id", id, cmd);
            cmd.ExecuteNonQuery();
        }

        public void UpdateEntry(int id, int quantity)
        {
            string sql = "UPDATE tracker SET quantity = @quantity WHERE id = @id;";
            SqliteCommand cmd = new SqliteCommand(sql, conn);
            AddParameter("@id", id, cmd);
            AddParameter("@quantity", quantity, cmd);
            cmd.ExecuteNonQuery();
        }

        public Entry GetHighestEntryForHabit(int habitId)
        {
            try
            {
                string sql = "SELECT tracker.id, tracker.date, tracker.quantity, habits.measurement FROM tracker " +
                             "JOIN habits ON tracker.habit_id = habits.id " +
                             "WHERE tracker.habit_id = @habitId " +
                             "ORDER BY quantity DESC;";
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                AddParameter("@habitId", habitId, cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Entry(reader);
                    }
                }
                return new Entry();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Entry();
            }
        }

        public List<Entry> GetEntries()
        {
            try
            {
                string sql = "SELECT tracker.id, tracker.date, tracker.quantity, habits.measurement FROM tracker " +
                             "JOIN habits on tracker.habit_id = habits.id;";
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                return GetQueriedList(cmd, reader => new Entry(reader));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Entry>();
            }
        }

        protected static void AddParameter<T>(string name, T value, SqliteCommand cmd)
        {
            SqliteParameter param = new SqliteParameter(name, SqlDbTypeConverter.GetDbType(value.GetType()));
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        protected static List<T> GetQueriedList<T>(SqliteCommand cmd, Func<SqliteDataReader, T> creator)
        {
            List<T> results = new List<T>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    try
                    {
                        results.Add(creator(reader));
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                }
            }
            return results;
        }

        public void Dispose()
        {
            if (conn != null) { conn.Close(); }
        }
    }
}
