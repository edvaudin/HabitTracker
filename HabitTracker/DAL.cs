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
        protected SqliteConnection conn = null;
        public DAL()
        {

        }
    }
}
