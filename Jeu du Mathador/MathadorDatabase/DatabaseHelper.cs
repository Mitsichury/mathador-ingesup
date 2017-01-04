

using System;
using System.Collections.Generic;
using System.Data;
using MathadorLibrary;
using System.Data.SQLite;

namespace MathadorDatabase
{
    public class DatabaseHelper
    {
        public static readonly string DatabaseFileName = "mathador.db";
        public static readonly string TableQuery = "CREATE TABLE IF NOT EXISTS Parties (" +                                                    
                                                    "nomJoueur VARCHAR(255) NOT NULL, "+
                                                    "points INTEGER NOT NULL,"+
                                                    "tempsMoyen INTEGER NOT NULL," +
                                                    "nombreMathador INTEGER NOT NULL," +
                                                    "pointsMoyen INTEGER NOT NULL" +
                                                   ")";
        private IDbConnection _databaseConnection;

        public void OpenConnexion()
        {
            string connectionString = "URI=file:"+DatabaseFileName;
            _databaseConnection = new SQLiteConnection(connectionString);
            _databaseConnection.Open();

            IDbCommand createDatabaseIfNeedeed = _databaseConnection.CreateCommand();            
            createDatabaseIfNeedeed.CommandText = TableQuery;
            createDatabaseIfNeedeed.ExecuteNonQuery();
            createDatabaseIfNeedeed.Dispose();
        }

        public void Flush()
        {
            OpenConnexion();
            IDbCommand createDatabaseIfNeedeed = _databaseConnection.CreateCommand();
            createDatabaseIfNeedeed.CommandText = "DELETE FROM Parties";
            createDatabaseIfNeedeed.ExecuteNonQuery();
            createDatabaseIfNeedeed.Dispose();
            CloseConnexion();
        }

        public void CloseConnexion()
        {            
            _databaseConnection.Close();
        }

        public int Insert(DatabaseEntry databaseEntry)
        {
            OpenConnexion();
            IDbCommand cmd = _databaseConnection.CreateCommand();            
            cmd.Prepare();
          
            const string sql =
               "INSERT INTO PARTIES(nomJoueur, points, tempsMoyen, nombreMathador, pointsMoyen) " +
               "VALUES(@nomJoueur, @points, @tempsMoyen, @nombreMathador, @pointsMoyen)";
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SQLiteParameter("@nomJoueur", databaseEntry.NomJoueur));
            cmd.Parameters.Add(new SQLiteParameter("@points", databaseEntry.Points));
            cmd.Parameters.Add(new SQLiteParameter("@tempsMoyen", databaseEntry.TempsMoyen));
            cmd.Parameters.Add(new SQLiteParameter("@nombreMathador", databaseEntry.NombreMatahdor));
            cmd.Parameters.Add(new SQLiteParameter("@pointsMoyen", databaseEntry.PointsMoyen));
            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            CloseConnexion();
            return result;
        }
       
        public List<DatabaseEntry> SelectAll()
        {
            List<DatabaseEntry> databaseEntries = new List<DatabaseEntry>();
            
            OpenConnexion();
            IDbCommand cmd = _databaseConnection.CreateCommand();         
            const string sql =
               "SELECT nomJoueur, points, tempsMoyen, nombreMathador, pointsMoyen " +
               "FROM Parties";
            cmd.CommandText = sql;
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                databaseEntries.Add(
                    new DatabaseEntry(
                        reader.GetString(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4)
                    )
                );
            }
            reader.Dispose();
            cmd.Dispose();
            CloseConnexion();
            return databaseEntries;
        }
    }
}