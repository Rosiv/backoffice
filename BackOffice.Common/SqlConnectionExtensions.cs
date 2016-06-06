using System.Data.SqlClient;

namespace BackOffice.Common
{
    public static class SqlConnectionExtensions
    {
        public static void OpenIfClosed(this SqlConnection connection)
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}
