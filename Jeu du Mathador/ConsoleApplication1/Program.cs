using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathadorDatabase;
using MathadorLibrary;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelper databaseHelper = new MathadorDatabase.DatabaseHelper();

            List<DatabaseEntry> databaseEntries = databaseHelper.SelectAll();

            Console.WriteLine(databaseEntries.Count);
        }
    }
}