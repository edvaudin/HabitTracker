using HabitTracker;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        using (DAL dal = new DAL())
        {
            dal.CreateMainTableIfMissing();
        }
    }
}