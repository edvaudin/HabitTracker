using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    public static class Viewer
    {

        public static void DisplayOptionsMenu()
        {
            Console.WriteLine("\nChoose an action from the following list:");
            Console.WriteLine("\tv - View your tracker");
            Console.WriteLine("\th - View your biggest entry for a habit");
            Console.WriteLine("\ta - Add a new entry");
            Console.WriteLine("\td - Delete an entry");
            Console.WriteLine("\tu - Update an entry");
            Console.WriteLine("\tc - Create new habit");
            Console.WriteLine("\t0 - Quit this application");
            Console.Write("Your option? ");
        }

        public static void DisplayTitle()
        {
            Console.WriteLine("Habit Tracker\r");
            Console.WriteLine("-------------\n");
        }

        public static void DisplayEntryTable()
        {
            DataAccessor dal = new();
            List<Entry> entries = dal.GetEntries();
            string output = string.Empty;
            foreach (Entry entry in entries)
            {
                output += $"{entry.GetString()}\n";
            }
            Console.WriteLine(output);
        }
    }
}
