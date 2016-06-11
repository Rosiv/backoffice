CREATE TRIGGER Products_AfterInsert_Trigger
    ON [BackOffice].[dbo].[Products]
 AFTER INSERT
AS
BEGIN
	DECLARE @h UNIQUEIDENTIFIER ;
	DECLARE @id varchar(10);
	DECLARE @name varchar(10);
	DECLARE @description varchar(15);

	SELECT @id = CONVERT(varchar, i.id) FROM inserted i;
	SELECT @name = i.name FROM inserted i;
	SELECT @description = i.description FROM inserted i;

	BEGIN DIALOG CONVERSATION @h 
	   FROM SERVICE ProductChanged_Service_Sender 
	   TO SERVICE 'ProductChanged_Service_Receiver' 
	   ON CONTRACT ProductChanged_Contract 
	   WITH ENCRYPTION=OFF 

	;SEND ON CONVERSATION @h  
	MESSAGE TYPE ProductChanged_Msg 
	('<Message type="Product_Inserted"><Id>'+@id+'</Id><Name>'+@name+'</Name><Description>'+@description+'</Description></Message>') 

END