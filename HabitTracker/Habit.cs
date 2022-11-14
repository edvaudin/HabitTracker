using MapDataReader;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    [GenerateDataReaderMapper]
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Measurement { get; set; } = string.Empty;

        public string GetString() => $"[#{Id}] {Name} (Measured in {Measurement})";
    }
}
