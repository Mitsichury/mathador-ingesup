using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathadorDatabase;
using MathadorLibrary;

namespace MathadorDatabaseTest
{
    [TestClass]
    [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    public class UnitTest1
    {
        [TestMethod]
        public void CanSelectAllIfEmpty()
        {

            DatabaseHelper databaseHelper = new MathadorDatabase.DatabaseHelper();

            List<DatabaseEntry> databaseEntries = databaseHelper.SelectAll();

            Assert.AreEqual(databaseEntries.Count, 0);
        }


        [TestMethod]
        public void CanInsert()
        {

            DatabaseHelper databaseHelper = new MathadorDatabase.DatabaseHelper();
            DatabaseEntry databaseEntry = new DatabaseEntry("nicolas", 18, 50, 1, 20);
            databaseHelper.Flush();
            databaseHelper.Insert(databaseEntry);
            List<DatabaseEntry> databaseEntries = databaseHelper.SelectAll();


            Assert.AreEqual(databaseEntries.Count, 1);
            Assert.AreEqual(databaseEntries[0].NomJoueur, databaseEntry.NomJoueur);
            Assert.AreEqual(databaseEntries[0].NombreMatahdor, databaseEntry.NombreMatahdor);
            Assert.AreEqual(databaseEntries[0].Points, databaseEntry.Points);
            Assert.AreEqual(databaseEntries[0].PointsMoyen, databaseEntry.PointsMoyen);
            Assert.AreEqual(databaseEntries[0].TempsMoyen, databaseEntry.TempsMoyen);
        }
    }
}
