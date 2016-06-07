using BackOffice.Common;
using System;
using System.Data.SqlClient;
using System.IO;

namespace BackOffice.EventProviders.SqlEventProvider
{
    class SqlHelper
    {
        internal static bool DbExists(SqlConnection connection)
        {
            Logging.Log().Debug("Checking if BackOffice DB exists in localdb...");

            string scriptPath = new Uri(Path.Combine(PathFinder.ExecutionPath, "SqlScripts", "CheckIfDatabaseExists.sql")).LocalPath;

            Logging.Log().Debug("Loading script file from {path}", scriptPath);

            var scriptContent = File.ReadAllText(scriptPath);

            Logging.Log().Debug("Executing sql script: \r\n{script}", scriptContent);

            connection.OpenIfClosed();

            SqlCommand cmd = new SqlCommand(scriptContent, connection);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int result = reader.GetInt32(0);
                    Logging.Log().Debug("Received response {result}", result);
                    bool dbExists = Convert.ToBoolean(result);

                    if (!dbExists) Logging.Log().Warning("BackOffice DB does not exist!");
                    else Logging.Log().Information("BackOffice DB exists.");

                    return dbExists;
                }
            }

            throw new InvalidOperationException("That should not happen");
        }

        internal static void InitDb(SqlConnection connection)
        {

            Logging.Log().Debug("Initializing BackOffice DB...");
            CreateDb(connection);
            EnableBroker(connection);
        }

        private static void CreateDb(SqlConnection connection)
        {
            string scriptPath = new Uri(Path.Combine(PathFinder.ExecutionPath, "SqlScripts", "CreateDB.sql")).LocalPath;

            Logging.Log().Debug("Loading script file from {path}", scriptPath);

            var scriptContentFormat = File.ReadAllText(scriptPath);
            var dbFilesDir = new Uri(Path.Combine(PathFinder.SolutionDir.FullName, "DBs")).LocalPath;
            var mdfFilePath = Path.Combine(dbFilesDir, "BackOffice.mdf");
            var ldfFilePath = Path.Combine(dbFilesDir, "BackOffice_log.ldf");
            var scriptContent = string.Format(scriptContentFormat, mdfFilePath, ldfFilePath);

            Logging.Log().Debug("Executing sql script: \r\n{script}", scriptContent);

            connection.OpenIfClosed();

            SqlCommand cmd = new SqlCommand(scriptContent, connection);
            cmd.ExecuteNonQuery();
        }

        private static void EnableBroker(SqlConnection connection)
        {
            Logging.Log().Debug("Enabling service broker...");
            string scriptPath = new Uri(Path.Combine(PathFinder.ExecutionPath, "SqlScripts", "EnableBroker.sql")).LocalPath;

            Logging.Log().Debug("Loading script file from {path}", scriptPath);

            var scriptContent = File.ReadAllText(scriptPath);

            Logging.Log().Debug("Executing sql script: \r\n{script}", scriptContent);

            connection.OpenIfClosed();

            SqlCommand cmd = new SqlCommand(scriptContent, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
