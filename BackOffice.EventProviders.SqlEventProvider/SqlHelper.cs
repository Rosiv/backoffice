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
            //Logging.Log().Debug("Initializing BackOffice DB...");
            //string scriptPath = new Uri(Path.Combine(executionPath, "SqlScripts", "CreateDB.sql")).LocalPath;

            //Logging.Log().Debug("Loading script file from {path}", scriptPath);

            //var scriptContent = File.ReadAllText(scriptPath);

            //Logging.Log().Debug("Executing sql script: \r\n{script}", scriptContent);

            //connection.OpenIfClosed();

            //SqlCommand cmd = new SqlCommand(scriptContent, connection);

//            cmd.ExecuteNonQuery();
        }

    }
}
