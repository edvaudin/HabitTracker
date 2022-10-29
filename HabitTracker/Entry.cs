﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    public class Entry
    {
        public int id = 0;
        public string date = string.Empty;
        public int quantity = 0;

        public Entry() { }

        public Entry(SqliteDataReader reader)
        {
            this.id = reader.GetInt32(reader.GetOrdinal("id"));
            this.date = reader.GetString(reader.GetOrdinal("date"));
            this.quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
        }

        public override string ToString()
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                return $"[#{id}] {quantity} counts on {parsedDate.ToLongDateString()}";
            }
            else
            {
                return $"[#{id}] {quantity} counts on {date}";
            }
        }
    }
}