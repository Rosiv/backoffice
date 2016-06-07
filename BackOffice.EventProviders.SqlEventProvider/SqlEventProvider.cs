using BackOffice.Common;
using BackOffice.Events;
using BackOffice.Interfaces;
using System;
using System.Data.SqlClient;
using System.IO;

namespace BackOffice.EventProviders.SqlEventProvider
{
    internal class SqlEventProvider : IEventProvider
    {
        const string CheckAndInitConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connection Timeout=7";
        const string ConnectionString = CheckAndInitConnectionString + ";Initial Catalog=BackOffice;";
        string dataBaseFile = Path.Combine(PathFinder.SolutionDir.FullName, "DBs", "BackOffice.mdf");
        SqlConnection connection;

        public SqlEventProvider()
        {
            using (var checkAndInitConnection = new SqlConnection(CheckAndInitConnectionString))
            {
                if (!SqlHelper.DbExists(checkAndInitConnection))
                {
                    SqlHelper.InitDb(checkAndInitConnection);
                }
            }

            this.connection = new SqlConnection(ConnectionString);
        }

        public IEvent Next()
        {
            Logging.Log().Debug("Waiting for upcoming event...");

            this.connection.OpenIfClosed();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.connection;
            string scriptPath = new Uri(Path.Combine(PathFinder.ExecutionPath, "SqlScripts", "WaitFor.sql")).LocalPath;
            cmd.CommandText = File.ReadAllText(scriptPath);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Logging.Log().Debug(reader.GetString(0));
                    Logging.Log().Debug(reader.GetString(1));
                }
            }

            return new SimpleEvent("SQL Event");
        }
    }
}
