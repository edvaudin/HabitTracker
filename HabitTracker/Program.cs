using HabitTracker;
using Microsoft.Data.Sqlite;

class Program
{
    private static DataAccessor dal = new DataAccessor();
    private static bool userFinished = false;
    static void Main(string[] args)
    {
        Viewer.DisplayTitle();

        InitializeDatabase();

        while (!userFinished)
        {
            Viewer.DisplayOptionsMenu();
            string userInput = UserInput.GetUserOption();
            ProcessInput(userInput);
        }
        ExitApp();
    }

    private static void ProcessInput(string userInput)
    {
        switch (userInput)
        {
            case "v":
                Viewer.DisplayEntryTable();
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
                userFinished = true;
                break;
            default:
                break;
        }
    }

    private static void ViewHighest()
    {
        int habitId = UserInput.GetHabitId();
        Entry entry = dal.GetHighestEntryForHabit(habitId);
        Console.WriteLine($"Below is the most {entry.Measurement} you have done in one go:");
        Console.WriteLine(entry);
    }

    private static void AddEntry()
    {
        int habitId = UserInput.GetHabitId();
        string date = UserInput.GetEntryDate();
        int quantity = UserInput.GetEntryQuantity(dal.GetHabit(habitId).Measurement);
        dal.AddEntry(date, quantity, habitId);
        Console.WriteLine("\nSuccessfully added new entry.");
    }

    private static void DeleteEntry()
    {
        Viewer.DisplayEntryTable();
        int id = UserInput.GetIdForRemoval();
        dal.DeleteEntry(id);
    }

    private static void UpdateEntry()
    {
        Viewer.DisplayEntryTable();
        int id = UserInput.GetIdForUpdate();
        int habitId = UserInput.GetHabitId();
        int quantity = UserInput.GetEntryQuantity(dal.GetHabit(habitId).Measurement);
        string date = UserInput.GetEntryDate();
        dal.UpdateEntry(id, quantity, date);
    }

    private static void CreateHabit()
    {
        string name = UserInput.GetHabitName();
        string measurement = UserInput.GetHabitMeasurement();
        dal.CreateHabit(name, measurement);
        Console.WriteLine("Successfully added new habit.");
    }

    private static void ExitApp()
    {
        Environment.Exit(0);
    }

    private static void InitializeDatabase()
    {
        dal.CreateMainTableIfMissing();
    }
}