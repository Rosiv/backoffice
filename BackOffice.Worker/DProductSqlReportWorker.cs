using BackOffice.Common;
using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Reports;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace BackOffice.Worker
{
    public class DProductSqlReportWorker : IJobWorker
    {
        const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Initial Catalog=BackOffice;";
        SqlConnection connection;
        private readonly SqlReport report;

        public DProductSqlReportWorker(SqlReport report)
        {
            this.report = report;
            this.connection = new SqlConnection(ConnectionString);
        }

        public void Start()
        {
            this.connection.OpenIfClosed();

            string queryFormat = @"INSERT INTO [dbo].[ProductReports]
           ([id]
           ,[name]
           ,[description])
     VALUES
           ('{0}','{1}','{2}')";

            string query = string.Format(
                queryFormat,
                this.report.Data.Product.Id,
                this.report.Data.Product.Name,
                this.report.Data.Product.Description);

            var cmd = new SqlCommand(query, this.connection);

            Logging.Log().Debug("Executing query {query}", query);
            Thread.Sleep(10000);
            cmd.ExecuteNonQuery();
        }
    }
}
