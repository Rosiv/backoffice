CREATE DATABASE BackOffice 
GO 

USE BackOffice 
GO 
SELECT is_broker_enabled 
FROM sys.databases 
WHERE name = 'BackOffice' 
GO

CREATE MESSAGE TYPE ProductChanged_Msg 
VALIDATION = WELL_FORMED_XML 
GO

CREATE CONTRACT ProductChanged_Contract 
(ProductChanged_Msg SENT BY ANY) 
GO 


CREATE QUEUE ProductChanged_Queue_Sender 
CREATE QUEUE ProductChanged_Queue_Receiver 
GO

SELECT * 
FROM ProductChanged_Queue_Sender 
GO 

CREATE SERVICE ProductChanged_Service_Sender 
ON QUEUE ProductChanged_Queue_Sender 
(ProductChanged_Contract) 

CREATE SERVICE ProductChanged_Service_Receiver 
ON QUEUE ProductChanged_Queue_Receiver 
(ProductChanged_Contract) 
GO 

DECLARE @h UNIQUEIDENTIFIER 

BEGIN DIALOG CONVERSATION @h 
   FROM SERVICE ProductChanged_Service_Sender 
   TO SERVICE 'ProductChanged_Service_Receiver' 
   ON CONTRACT ProductChanged_Contract 
   WITH ENCRYPTION=OFF 

--Don't lose this GUID!
--2734DC7E-7F2C-E611-9BCE-844BF5C3B00E 
SELECT @h 
GO

--Now we'll actually send a message. The GUID we generated in 
--the last section is used by the initiator to send messages 
--to the target. 
DECLARE @h UNIQUEIDENTIFIER 
--Insert the GUID from the last section 
SET @h = '7A1A1495-882C-E611-9BCE-844BF5C3B00E' 

--Actually do the send 
--Note the semicolon. This is necessary to help SQL Server  
--correctly parse this statement. 
;SEND ON CONVERSATION @h  
MESSAGE TYPE ProductChanged_Msg 
('<Hello_Simple_Talk/>') 
GO


SELECT * 
FROM ProductChanged_Queue_Receiver 