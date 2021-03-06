﻿CREATE TRIGGER Products_AfterDelete_Trigger
    ON [BackOffice].[dbo].[Products]
 AFTER DELETE
AS
BEGIN
	DECLARE @h UNIQUEIDENTIFIER ;
	DECLARE @id varchar(10);
	DECLARE @name varchar(10);
	DECLARE @description varchar(15);

	SELECT @id = CONVERT(varchar, i.id) FROM deleted i;
	SELECT @name = i.name FROM deleted i;
	SELECT @description = i.description FROM deleted i;

	BEGIN DIALOG CONVERSATION @h 
	   FROM SERVICE ProductChanged_Service_Sender 
	   TO SERVICE 'ProductChanged_Service_Receiver' 
	   ON CONTRACT ProductChanged_Contract 
	   WITH ENCRYPTION=OFF 

	;SEND ON CONVERSATION @h  
	MESSAGE TYPE ProductChanged_Msg 
	('<Message Action="Product_Deleted"><Product><Id>'+@id+'</Id><Name>'+@name+'</Name><Description>'+@description+'</Description></Product></Message>') 

END