go
-- Needs tweaking. Search for TODO in this file.
-- See https://gist.github.com/scott4dev/872260
-- http://sqljunkies.com/Article/4CD01686-5178-490C-A90A-5AEEF5E35915.scuk


-- =============================================================================================
-- Block 1. One-time setup. This should be done manually.

/*
-- Configure the server for CLR operation.
sp_configure 'clr enabled', 1
GO
reconfigure
GO
*/


-- Error: System.Security.SecurityException: Request for the permission of type 'System.Data.SqlClient.SqlClientPermission
-- Fix: alter database PD set trustworthy on

-- Error: The database owner SID recorded in the master database differs from the database owner SID recorded in database 'YourDatabaseName'.
-- You should correct this situation by resetting the owner of database 'YourDatabaseName' using the ALTER AUTHORIZATION statement.
-- Fix: 
/*
DECLARE @Command VARCHAR(MAX) = 'ALTER AUTHORIZATION ON DATABASE::<<DatabaseName>> TO [<<LoginName>>]' 
SELECT @Command = REPLACE(REPLACE(@Command, '<<DatabaseName>>', SD.Name), '<<LoginName>>', SL.Name)
FROM master..sysdatabases SD JOIN master..syslogins SL ON  SD.SID = SL.SID
WHERE SD.Name = DB_NAME()

PRINT @Command
EXEC(@Command)
*/

-- =============================================================================================
-- Block 2. Installation and re-installation.

-- TODO: Set the name of the database that you want to install into.
go
use PD
go

-- TODO Where is the assembly? These are server-side paths. You can use UNC names as well, which can simplify things.
declare @AssemblyPath varchar(1000) = 'D:\Pd\Debug'
declare @AssemblyName varchar(100) = 'SQLCLR'
declare @AssemblyObject varchar(1000) = @AssemblyPath + '\' + @AssemblyName + '.dll'
declare @AssemblyDebug varchar(1000) = @AssemblyPath + '\' + @AssemblyName + '.pdb'
declare @sql varchar(max), @objname varchar(255), @objtype varchar(255);

print 'The assembly in ''' + @AssemblyObject + ''' will be (re)created with a name of ''' + @AssemblyName + ''' in the database ' + db_name();

-- First drop any procs and functions from the assembly, then drop the assembly itself.
declare CSR cursor for
select distinct
	C.name as [ObjectName], C.type
	from Sys.Assemblies A
		join SYS.ASSEMBLY_MODULES B on a.assembly_id=B.assembly_id
		join SYS.OBJECTS C on B.object_id = C.object_id
	where a.name = @AssemblyName
open CSR

fetch next from CSR Into @objName, @objType
while @@FETCH_STATUS = 0 begin
	--print @objType 
	if @objType = 'PC' set @sql = 'drop procedure ' + @objName		-- Proc.
	if @objType = 'AF' set @sql = 'drop aggregate ' + @objName		-- Aggregate function.
	if @objType = 'FT' set @sql = 'drop function ' + @objName		-- TVF.
	if @objType = 'FS' set @sql = 'drop function ' + @objName		-- Scalar function.
	print @sql
	exec (@sql)
	fetch next from CSR into @objName, @objType
end
close CSR
deallocate CSR

if exists(select name from sys.assemblies where name = @AssemblyName) begin
	set @sql = 'drop assembly [' + @AssemblyName + ']'
	print @sql
	exec (@sql)
end


-- Options are SAFE, EXTERNAL and UNSAFE
set @sql = 'create assembly [' + @AssemblyName + '] from ''' + @AssemblyObject + ''' with permission_set = external_access'
print @sql
exec (@sql)

begin try
    -- This adds debugging info; the file will not be present in your "release" version
    -- (as opposed to your "debug" version), so we don't want to fail if it's not there.
	set @sql = 'alter assembly [' + @AssemblyName + '] add file from ''' + @AssemblyDebug + ''''
	print @sql
	exec (@sql)
end try
begin catch
end catch
GO




-- Create procs bound to the CLR code.
-- The syntax is "external name assembly_name.class_name.method_name"
go

create procedure dbo.LogMsg
	(
	@message nvarchar(max),
	@numRows int
	)
with execute as caller
as external name [SQLCLR].[SQLCLR.ProceduresAndFunctions].[LogMsg];
go


CREATE FUNCTION dbo.SplitString
(
   @List NVARCHAR(MAX),
   @Delimiter NVARCHAR(255)
)
RETURNS TABLE (Item NVARCHAR(4000))
EXTERNAL NAME [SQLCLR].[SQLCLR.ProceduresAndFunctions].[SplitString_Multi];
go

CREATE AGGREGATE dbo.Concat
( 
	@Value NVARCHAR(MAX),
	@Delimiter NVARCHAR(4000) 
)
RETURNS NVARCHAR(MAX) 
EXTERNAL NAME [SQLCLR].[SQLCLR.Concat];
go

-- =============================================================================================
/*
-- Block 3. Testing.

begin tran
exec dbo.LogToTable 'pd test'
rollback

select dbo.Concat(s, ',') from (values ('a'), ('b'), ('a')) x(s)
select dbo.Concat(distinct s, ',') from (values ('a'), ('b'), ('a')) x(s)

select table_schema, PD.dbo.concat(TABLE_NAME, ', ') Tables
from information_schema.tables
group by table_schema

select * from dbo.SplitString('hello,world,again', ',')

*/
