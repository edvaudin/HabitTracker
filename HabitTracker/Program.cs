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
            string userInput = GetUserInput();
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
            case "a":
                AddEntry();
                break;
            case "d":
                DeleteEntry();
                break;
            case "u":
                UpdateEntry();
                break;
            case "0":
                ExitApp();
                break;
            default:
                break;
        }
    }

    private static void ExitApp()
    {
        Environment.Exit(0);
    }

    private static void UpdateEntry()
    {
        Console.WriteLine("Not implemented yet.");
    }

    private static void DeleteEntry()
    {
        Console.WriteLine("Not implemented yet.");
    }

    private static void AddEntry()
    {
        Console.WriteLine("Not implemented yet.");
    }

    private static void ViewTable()
    {
        Console.WriteLine("Not implemented yet.");
    }

    private static string GetUserInput()
    {
        string input = Console.ReadLine();
        while (!IsValidInput(input))
        {
            Console.Write("This is not valid input. Please enter one of the above options: ");
            input = Console.ReadLine();
        }
        return input;
    }

    private static bool IsValidInput(string? input)
    {
        string[] validOptions = { "v", "a", "d", "u", "0" };
        foreach (string validOption in validOptions)
        {
            if (input == validOption)
            {
                return true;
            }
        }
        return false;
    }

    private static void DisplayOptionsMenu()
    {
        Console.WriteLine("Choose an action from the following list:");
        Console.WriteLine("\tv - View your tracker");
        Console.WriteLine("\ta - Add a new entry");
        Console.WriteLine("\td - Delete an entry");
        Console.WriteLine("\tu - Update an entry");
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