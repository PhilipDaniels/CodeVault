-- Shows how to exec a proc into a temp table dynamically.
-- This is useful if the proc returns a large recordset and you don't
-- want to, or can't, determine the types of all the columns.

-- This example uses a linked server that is in fact THE SAME AS THE LOCAL SERVER!
-- i.e. it is a loopback. This is done so we can use OPENQUERY().

-- First setup a linked server.
EXEC sp_addlinkedserver @server = 'LOCALSERVER',  @srvproduct = '', @provider = 'SQLOLEDB', @datasrc = @@servername
-- Run your proc
SELECT * INTO #MyTempTable FROM OPENQUERY(LOCALSERVER, 'exec DMS_Petron.dbo.CSO_WINDMSCSO ''2012-11-05''')

-- Check the table
select * from #MyTempTable 

-- Get columns (1)
use tempdb
exec sp_help '#MyTempTable'

-- Get columns (2)
Select * From tempdb.sys.columns Where object_id=OBJECT_ID('tempdb.dbo.#myTempTable');


drop table #MyTempTable 

-- Get rid of the linked server.
sp_dropserver 'LOCALSERVER'
