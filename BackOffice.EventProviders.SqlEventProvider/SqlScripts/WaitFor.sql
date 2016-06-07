
DECLARE
    @conversation uniqueidentifier,
    @senderMsgType nvarchar(100),
    @msg varchar(max);

WAITFOR (
    RECEIVE TOP(1)
        @conversation=conversation_handle,
        @msg=message_body,
        @senderMsgType=message_type_name
    FROM ProductChanged_Queue_Receiver);

SELECT @msg AS RecievedMessage,
       @senderMsgType AS SenderMessageType;
