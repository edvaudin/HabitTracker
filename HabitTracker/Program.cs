using HabitTracker;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        DisplayTitle();

        InitializeDatabase();

        while (true)
        {
            DisplayOptionsMenu();
            string userInput = InputValidator.GetUserOption();
            ProcessInput(userInput);
        }
    }

    private static void ProcessInput(string userInput)
    {
        switch (userInput)
        {
            case "v":
                ViewTable();
                break;
            case "h":
                ViewHighest();
                break;
            case "a":
                AddEntry();
                break;
            case "d":
                DeleteEntry();
                break;
            case "u":
                UpdateEntry();
                break;
            case "c":
                CreateHabit();
                break;
            case "0":
                ExitApp();
                break;
            default:
                break;
        }
    }

    private static void ViewTable()
    {
        using (DAL dal = new DAL())
        {
            List<Entry> entries = dal.GetEntries();
            string output = string.Empty;
            foreach (Entry entry in entries)
            {
                output += $"{entry}\n";
            }
            Console.WriteLine(output);
        }
    }

    private static void ViewHighest()
    {
        using (DAL dal = new DAL())
        {
            int habitId = InputValidator.GetHabitId();
            try
            {
                Entry entry = dal.GetHighestEntryForHabit(habitId);
                Console.WriteLine($"Below is the most {entry.measurement} you have done in one go:");
                Console.WriteLine(entry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static void AddEntry()
    {
        using (DAL dal = new DAL())
        {
            try
            {
                int habitId = InputValidator.GetHabitId();
                string date = InputValidator.GetEntryDate();
                int quantity = InputValidator.GetEntryQuantity(dal.GetHabit(habitId).measurement);

                dal.AddEntry(date, quantity, habitId);
                Console.WriteLine("Successfully added new entry.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static void DeleteEntry()
    {
        ViewTable();
        int id = InputValidator.GetIdForRemoval();
        using (DAL dal = new DAL())
        {
            try
            {
                dal.DeleteEntry(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static void UpdateEntry()
    {
        using (DAL dal = new DAL())
        {
            ViewTable();
            int id = InputValidator.GetIdForUpdate();
            int habitId = InputValidator.GetHabitId();
            int quantity = InputValidator.GetEntryQuantity(dal.GetHabit(habitId).measurement);

            try
            {
                dal.UpdateEntry(id, quantity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static void CreateHabit()
    {
        string name = InputValidator.GetHabitName();
        string measurement = InputValidator.GetHabitMeasurement();
        using (DAL dal = new DAL())
        {
            try
            {
                dal.CreateHabit(name, measurement);
                Console.WriteLine("Successfully added new habit.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static void ExitApp()
    {
        Environment.Exit(0);
    }

    private static void DisplayOptionsMenu()
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

    private static void InitializeDatabase()
    {
        using (DAL dal = new DAL())
        {
            try
            {
                dal.CreateMainTableIfMissing();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static void DisplayTitle()
    {
        Console.WriteLine("Habit Tracker\r");
        Console.WriteLine("-------------\n");
    }
}