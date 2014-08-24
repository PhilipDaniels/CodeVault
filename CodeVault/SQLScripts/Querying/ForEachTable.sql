-- Perform a SQL statement against each table. ? is the table name.
EXEC sp_MSforeachtable 'UPDATE ? SET X_dtArchived = 01012000'

-- Can use @whereand parameter to restrict the set of tables. In this case PRINT ? is the operation
-- which just causes the matching table names to be printed (handy for testing).
-- You can also put an ORDER BY on the end of this parameter.
EXEC sp_MSforeachtable "PRINT '?'",  @whereand="AND name like 'Cus%'"

-- Can restrict an operation to tables with a certain number of rows, etc.
EXEC sp_MSforeachtable "DBCC CHECKTABLE('?')",@whereand=" AND (select rows from sysindexes 
       where sysindexes.id = o.id 
       and indid < 2)
       < 100000"
