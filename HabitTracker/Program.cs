﻿using HabitTracker;
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

    private static void ExitApp()
    {
        Environment.Exit(0);
    }

    private static void UpdateEntry()
    {
        using (DAL dal = new DAL())
        {
            ViewTable();
            int id = GetIdForUpdate();
            int habitId = GetHabitId();
            int quantity = GetEntryQuantity(dal.GetHabit(habitId).measurement);
        
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

    private static void DeleteEntry()
    {
        ViewTable();
        int id = GetIdForRemoval();
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

    private static void AddEntry()
    {
        using (DAL dal = new DAL())
        {
            try
            {
                int habitId = GetHabitId();
                string date = GetEntryDate();
                int quantity = GetEntryQuantity(dal.GetHabit(habitId).measurement);
            
                dal.AddEntry(date, quantity, habitId);
                Console.WriteLine("Successfully added new entry.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static int GetHabitId()
    {
        string input = string.Empty;
        using (DAL dal = new DAL())
        {
            try
            {
                string listOfHabits = string.Empty;
                List<Habit> validHabits = dal.GetHabits();
                foreach (Habit habit in validHabits)
                {
                    listOfHabits += $"{habit}\n";
                }
                Console.WriteLine(listOfHabits);
                List<int> validHabitIds = validHabits.Select(h => h.id).ToList();
                Console.Write("Enter the number corresponding to the habit this entry is for: ");
                while (true)
                {
                    if (Int32.TryParse(Console.ReadLine(), out int result))
                    {
                        if (validHabitIds.Contains(result))
                        {
                            return result;
                        }
                    }
                    Console.Write("This is not a valid id, please enter a number: ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        return Int32.Parse(input);
    }

    private static void CreateHabit()
    {
        string name = GetHabitName();
        string measurement = GetHabitMeasurement();
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

    private static string GetHabitMeasurement()
    {
        Console.Write("What is the measurement of your habit? ");
        string input = Console.ReadLine();
        while (input.Length == 0)
        {
            Console.Write("Habit measurement should not be empty. Try again: ");
            input = Console.ReadLine();
        }
        return input;
    }

    private static string GetHabitName()
    {
        Console.Write("What is the name of your habit? ");
        string input = Console.ReadLine();
        while (input.Length == 0)
        {
            Console.Write("Habit name should not be empty. Try a new name: ");
            input = Console.ReadLine();
        }
        return input;
    }

    private static int GetIdForRemoval()
    {
        Console.Write("Which entry do you want to remove? ");
        using (DAL dal = new DAL())
        {
            List<int> validIds = dal.GetEntries().Select(o => o.id).ToList();
            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int result))
                {
                    if (validIds.Contains(result))
                    {
                        return result;
                    }
                }
                Console.Write("This is not a valid id, please enter a number: ");
            }
        }
    }

    private static int GetIdForUpdate()
    {
        Console.Write("Which entry do you want to update? ");
        using (DAL dal = new DAL())
        {
            List<int> validIds = dal.GetEntries().Select(o => o.id).ToList();
            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int result))
                {
                    if (validIds.Contains(result))
                    {
                        return result;
                    }
                }
                Console.Write("This is not a valid id, please enter a number: ");
            }
        }
    }

    private static int GetEntryQuantity(string measurement)
    {
        Console.Write($"How many {measurement} did you do? ");
        string input = Console.ReadLine();
        while (!Int32.TryParse(input, out int parsed))
        {
            Console.Write("This is not a valid quantity, please enter a number: ");
            input = Console.ReadLine();
        }
        return Int32.Parse(input);
    }

    private static string GetEntryDate()
    {
        Console.Write("What date are you adding for? (yyyy-mm-dd): ");
        string input = Console.ReadLine();
        while (!DateTime.TryParse(input, out DateTime date))
        {
            Console.Write("This is not a valid date, please use the format 'yyyy-mm-dd'");
            input = Console.ReadLine();
        }
        return input;
    }


    private static string GetUserInput()
    {
        string input = Console.ReadLine();
        while (!IsValidInput(input))
        {
            Console.Write("This is not a valid input. Please enter one of the above options: ");
            input = Console.ReadLine();
        }
        return input;
    }

    private static bool IsValidInput(string? input)
    {
        string[] validOptions = { "v", "a", "d", "u", "c", "0" };
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
        Console.WriteLine("Steps Tracker\r");
        Console.WriteLine("-------------\n");
    }
}