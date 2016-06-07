﻿using BackOffice.Common;
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
            this.connection = new SqlConnection(string.Format(connectionStringFormat, dataBaseFile));
            if (!SqlHelper.DbExists(this.connection))
            {
                SqlHelper.InitDb(this.connection);
            }
        }


        public IEvent Next()
        {
            Logging.Log().Debug("Waiting for upcoming event...");

            this.connection.OpenIfClosed();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.connection;
            cmd.CommandText = @"DECLARE
    @conversation uniqueidentifier,
    @senderMsgType nvarchar(100),
    @msg varchar(max);

WAITFOR (
    RECEIVE TOP(1)
        @conversation=conversation_handle,
        @msg=message_body,
        @senderMsgType=message_type_name
    FROM Products_Queue);

SELECT @msg AS RecievedMessage,
       @senderMsgType AS SenderMessageType;

END CONVERSATION @conversation;";

            using(var reader = cmd.ExecuteReader())
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
