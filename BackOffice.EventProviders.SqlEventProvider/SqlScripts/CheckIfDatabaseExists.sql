DECLARE @dbname nvarchar(128)
SET @dbname = N'BackOffice'

IF (EXISTS (SELECT name 
FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = @dbname 
OR name = @dbname)))

-- code mine :)
SELECT 1
ELSE
SELECT 0