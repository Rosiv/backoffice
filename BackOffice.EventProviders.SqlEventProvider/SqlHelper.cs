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

        internal static void CreateDb(SqlConnection connection)
        {
            Logging.Log().Debug("Creating BackOffice DB...");
            ExecuteSqlScript(connection, @"CreateDB.sql");
            Logging.Log().Debug("Database created.");
        }

        internal static void InitDb(SqlConnection connection)
        {
            Logging.Log().Debug("Initializing BackOffice DB...");
            Logging.Log().Debug("Enabling service broker...");
            ExecuteSqlScript(connection, @"EnableBroker.sql");

            Logging.Log().Debug("Initializing messages...");
            ExecuteSqlScript(connection, @"Messages\ProductChanged_Msg.sql");

            Logging.Log().Debug("Initializing contracts...");
            ExecuteSqlScript(connection, @"Contracts\ProductChanged_Contract.sql");

            Logging.Log().Debug("Initializing queues...");
            ExecuteSqlScript(connection, @"Queues\ProductChanged_Queue_Sender.sql");
            ExecuteSqlScript(connection, @"Queues\ProductChanged_Queue_Receiver.sql");

            Logging.Log().Debug("Initializing services...");
            ExecuteSqlScript(connection, @"Services\ProductChanged_Service_Sender.sql");
            ExecuteSqlScript(connection, @"Services\ProductChanged_Service_Receiver.sql");

            Logging.Log().Debug("Initializing tables...");
            ExecuteSqlScript(connection, @"Tables\Products.sql");
            ExecuteSqlScript(connection, @"Tables\ProductReports.sql");

            Logging.Log().Debug("Initializing triggers...");
            ExecuteSqlScript(connection, @"Triggers\Products_AfterInsert_Trigger.sql");
            ExecuteSqlScript(connection, @"Triggers\Products_AfterUpdate_Trigger.sql");
            ExecuteSqlScript(connection, @"Triggers\Products_AfterDelete_Trigger.sql");

            Logging.Log().Debug("Initialization finished.");
        }

        private static void ExecuteSqlScript(SqlConnection connection, string scriptName)
        {
            string scriptPath = new Uri(Path.Combine(PathFinder.ExecutionPath, "SqlScripts", scriptName)).LocalPath;
            Logging.Log().Debug("Loading script file from {path}", scriptPath);

            var scriptContent = File.ReadAllText(scriptPath);

            Logging.Log().Debug("Executing sql script: \r\n{script}", scriptContent);

            connection.OpenIfClosed();

            SqlCommand cmd = new SqlCommand(scriptContent, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
