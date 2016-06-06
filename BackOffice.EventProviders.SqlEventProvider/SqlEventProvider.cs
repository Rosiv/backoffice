using BackOffice.Common;
using BackOffice.Events;
using BackOffice.Interfaces;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace BackOffice.EventProviders.SqlEventProvider
{
    internal class SqlEventProvider : IEventProvider
    {
        string dataBaseFile = Path.Combine(PathFinder.SolutionDir.FullName, "DBs", "BackOffice.mdf");
        string connectionStringFormat = @"Data Source=(localdb)\MSSQLLocalDB;AttachDBFilename=""{0}"";Integrated Security=True;Connection Timeout=7";
        SqlConnection connection;

        public SqlEventProvider()
        {
            this.connection = new SqlConnection(string.Format(connectionStringFormat, dataBaseFile);
            if (!SqlHelper.DbExists(this.connection))
            {
                SqlHelper.InitDb(this.connection);
            }
        }


        public IEvent Next()
        {
            Logging.Log().Debug("Waiting for upcoming event...");

            this.connection.OpenIfClosed();



            Thread.Sleep(new Random().Next(1, 10000));

            return new SimpleEvent("SQL Event");
        }
    }
}
