IF OBJECT_ID('DatabaseFiles') IS NULL
 BEGIN
     SELECT TOP 0 * INTO DatabaseFiles
     FROM sys.database_files    

     ALTER TABLE DatabaseFiles ADD DbName nvarchar(200)
     ALTER TABLE DatabaseFiles ADD CreationDate DATETIME DEFAULT(GETDATE())
 END
 EXECUTE sp_msforeachdb 'INSERT INTO DatabaseFiles SELECT *, ''?'', GETDATE() FROM [?].sys.database_files'

SELECT dbname, * FROM DatabaseFiles order by physical_name

drop table DatabaseFiles
