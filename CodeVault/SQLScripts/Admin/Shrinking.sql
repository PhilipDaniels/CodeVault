-- The most reliable way to shrink the log is to detach the db,
-- physically delete the file, then reattach the db. A new minimal
-- log file will be created.


use WebCAT
go
exec sp_helpfile
GO
BACKUP LOG WebCAT WITH TRUNCATE_ONLY
GO
DBCC SHRINKFILE (WebCat_log, 1)
GO
DBCC SHRINKFILE (WebCat, 1)
GO
exec sp_helpfile




Another way to truncate logfile without taking backup is,

DBCC sqlperf ('logspace')
check for the %used column for your database

do the following,
alter database MyDatabase set recovery simple
checkpoint
alter database MyDatabase set recovery full
checkpoint    

DBCC sqlperf ('logspace')
check for the %used column for your database 



