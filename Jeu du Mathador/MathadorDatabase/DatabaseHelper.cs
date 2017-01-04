

using System;
using System.Data;
using Mono.Data.Sqlite;

namespace MathadorDatabase
{
    class DatabaseHelper
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
            _databaseConnection = new SqliteConnection(connectionString);
            _databaseConnection.Open();

            IDbCommand createDatabaseIfNeedeed = _databaseConnection.CreateCommand();            
            createDatabaseIfNeedeed.CommandText = TableQuery;
            createDatabaseIfNeedeed.ExecuteNonQuery();
            createDatabaseIfNeedeed.Dispose();

        }

        public void CloseConnexion()
        {            
            _databaseConnection.Close();
        }

        public int Insert(string nomJoueur, int points, int tempsMoyen, int nombreMatahdor, int pointsMoyen)
        {
            OpenConnexion();
            IDbCommand cmd = _databaseConnection.CreateCommand();            
            cmd.Prepare();

            
            const string sql =
               "INSERT INTO PARTIES(nomJoueur, points, tempsMoyen, nombreMathador, pointsMoyen) " +
               "VALUES(?,?,?,?,?)";
            cmd.CommandText = sql;
            cmd.Parameters.Add(nomJoueur);
            cmd.Parameters.Add(points);
            cmd.Parameters.Add(tempsMoyen);
            cmd.Parameters.Add(nombreMatahdor);
            cmd.Parameters.Add(pointsMoyen);
            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            CloseConnexion();
            return result;
        }
       
        public void SelectAll()
        {
            OpenConnexion();
            IDbCommand cmd = _databaseConnection.CreateCommand();         
            const string sql =
               "SELECT nomJoueur, points, tempsMoyen, nombreMathador, pointsMoyen " +
               "FROM Parties";
            cmd.CommandText = sql;
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string firstName = reader.GetString(0);
                string lastName = reader.GetString(1);
                Console.WriteLine("Name: {0} {1}",
                    firstName, lastName);
            }
            reader.Dispose();
            cmd.Dispose();
            CloseConnexion();
        }
    }
}