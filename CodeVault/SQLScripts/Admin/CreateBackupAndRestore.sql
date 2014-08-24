-- CREATE DATABASE. 
-- Sizes default to MB.
CREATE DATABASE PD1
ON     (NAME = PD1DAT, FILENAME = 'D:\MSSQL7\DATA\PD1.MDF', SIZE = 5)
LOG ON (NAME = PD1LOG, FILENAME = 'D:\MSSQL7\DATA\PD1.LDF', SIZE = 5) 

ALTER DATABASE PD1 MODIFY FILE (NAME = PD1DAT, SIZE = 10)
DROP DATABASE PD1

-- A quick way to copy a database to another server is to first detach it, copy the
-- files to the destination server, then reattach it under a different name.
EXEC sp_detach_db 'pubs2', 'true'

EXEC sp_attach_db @dbname = N'pubs', 
    @filename1 = N'c:\mssql7\data\pubs.mdf', 
    @filename2 = N'c:\mssql7\data\pubs_log.ldf'

-- or for single file databases...
EXEC sp_attach_single_file_db @dbname = 'pubs', @physname = 'c:\mssql7\data\pubs.mdf'

-- To copy a database on the same server, use the Database Backup technique below.
-- Copy the DB.DAT file to somewhere on the new server, then on the destination server
-- run this SQL. If you want to copy the database on the same server, just use different
-- filenames in the MOVE clause.
BACKUP DATABASE [Northwind] TO DISK = 'C:\TEMP\Northwind.bak' WITH INIT,SKIP

-- To perform out-of-line backups use COPY_ONLY.
backup database CSO_Malaysia
to disk = 'I:\dbbackup\2012-06-08\CSO_Malaysia.bak'
with copy_only, stats

RESTORE DATABASE [Northwind2]
FROM DISK='C:\Temp\Northwind.bak'
WITH RECOVERY, REPLACE,
MOVE 'Northwind' TO 'C:\MSSQL7\DATA\Northwind2_DATA.MDF',
MOVE 'Northwind_Log' TO 'C:\MSSQL7\DATA\Northwind2_LOG.LDF'

-- Getting info on databases.
sp_helpdb 'pubs'		-- dbname is optional
sp_helpfile
sp_helplogins
sp_helpsort




-------------------------------------------------------------------------
-- Backup a single table? Not tested.
USE AdventureWorks
GO
DECLARE @table VARCHAR(128), @file VARCHAR(255),@cmd VARCHAR(512)
-- If i need to create CSV file Product table then
SET @table = 'Production.Product'
SET @file = 'D:\BCP_OUTPUT\' + @table + '_' + CONVERT(CHAR(8), GETDATE(), 112) + '.csv'
SET @cmd = 'bcp "AdventureWorks.' + @table + '" out "' + @file + '" -S. -T -c -t,'
EXEC master..xp_cmdshell @cmd


-- To backup.
--bcp "select * from [MyDatabase].dbo.Customer " queryout "Customer.bcp" -N -S localhost -T -E
-- To restore.
--bcp [MyDatabase].dbo.Customer in "Customer.bcp" -N -S localhost -T -E


-- Step 1. Script off the CREATE TABLE statement and create your target table.

-- Step 2. Generate a format file for the table.
EXEC master..xp_cmdshell 'bcp PDTEMP.CSO.RFCTransactionLog format nul -T -n -f c:\temp\CSO_RFCTransactionLog-n.fmt';
EXEC master..xp_cmdshell 'bcp PDTEMP..RFCTransactionLog format nul -T -n -f c:\temp\CSO_RFCTransactionLog-n.fmt';

-- Step 3. Extract the source table.
EXEC master..xp_cmdshell 'bcp PDTEMP.CSO.RFCTransactionLog out c:\temp\CSO_RFCTransactionLog.data -N -T';
BCP "SELECT TOP 20 * FROM AdventureWorks.Person.Address" QUERYOUT d:\PersonAddressByQuery.txt -T -c


-- Step 4. Import the data into the target.
BULK INSERT CSO.RFCTransactionLog FROM 'c:\temp\CSO_RFCTransactionLog.data' WITH (DATAFILETYPE='widenative'); 

