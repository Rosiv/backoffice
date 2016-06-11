using BackOffice.Common;
using BackOffice.Events;
using BackOffice.Interfaces;
using System;
using System.Data.SqlClient;
using System.IO;

namespace BackOffice.EventProviders.SqlEventProvider
{
    public class SqlEventProvider : IEventProvider
    {
        const string CheckAndCreateDbConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True";
        const string ConnectionString = CheckAndCreateDbConnectionString + ";Initial Catalog=BackOffice;";
        string dataBaseFile = Path.Combine(PathFinder.SolutionDir.FullName, "DBs", "BackOffice.mdf");
        SqlConnection connection;

        public SqlEventProvider()
        {
            this.connection = new SqlConnection(ConnectionString);

            using (var checkAndCreateConnection = new SqlConnection(CheckAndCreateDbConnectionString))
            {
                if (!SqlHelper.DbExists(checkAndCreateConnection))
                {
                    SqlHelper.CreateDb(checkAndCreateConnection);
                    SqlHelper.InitDb(this.connection);
                }
            }
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
                if (reader.Read())
                {
                    var message = reader.GetString(0).Replace("\0", string.Empty);
                    var messageType = reader.GetString(1);

                    return new SqlEvent("SqlEvent", messageType, message);
                }
            }

            throw new InvalidOperationException();
        }
    }
}
