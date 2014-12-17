/*
Object type:
AF = Aggregate function (CLR)
C = CHECK constraint
D = DEFAULT (constraint or stand-alone)
F = FOREIGN KEY constraint
FN = SQL scalar function
FS = Assembly (CLR) scalar-function
FT = Assembly (CLR) table-valued function
IF = SQL inline table-valued function
IT = Internal table
P = SQL Stored Procedure
PC = Assembly (CLR) stored-procedure
PG = Plan guide
PK = PRIMARY KEY constraint
R = Rule (old-style, stand-alone)
RF = Replication-filter-procedure
S = System base table
SN = Synonym
SO = Sequence object
SQ = Service queue
TA = Assembly (CLR) DML trigger
TF = SQL table-valued-function
TR = SQL DML trigger
TT = Table type
U = Table (user-defined)
UQ = UNIQUE constraint
V = View
X = Extended stored procedure
*/

-- =====================================================================================
-- TABLES - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[TableName]', 'U') IS NOT NULL BEGIN
	PRINT 'Dropping table [dbo].[TableName]'
	EXEC('DROP TABLE [dbo].[TableName]');
END
CREATE TABLE [TableName]
	(
	ID INT,
	CONSTRAINT [PK_T1] PRIMARY KEY CLUSTERED ([Id])
	)

-- ALTER SYNTAX FORM - NA for tables.

-- =====================================================================================
-- TEMP TABLES - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('tempdb..#TempTableName', 'U') IS NOT NULL BEGIN
	PRINT 'Dropping temp table #TempTableName'
	EXEC('DROP TABLE #TempTableName');
END
CREATE TABLE #TempTableName(ID INT)

-- =====================================================================================
-- VIEWS - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[ViewName]', 'V') IS NOT NULL BEGIN
	PRINT 'Dropping view [dbo].[ViewName]'
	EXEC('DROP VIEW [dbo].[ViewName]');
END
GO
CREATE VIEW [dbo].[ViewName] AS
SELECT 1 as Id
GO

-- ALTER SYNTAX FORM
IF OBJECT_ID('[dbo].[ViewName]', 'V') IS NULL BEGIN
	PRINT '[dbo].[ViewName] did not exist - creating dummy view so ALTER will succeed'
	EXEC('CREATE VIEW [dbo].[ViewName] AS SELECT 1 as Id')
END
GO
ALTER VIEW [dbo].[ViewName] AS
SELECT 1 as Id
GO

-- =====================================================================================
-- PROCS - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[ProcName]', 'P') IS NOT NULL BEGIN
	PRINT 'Dropping proc [dbo].[ProcName]'
	EXEC('DROP PROCEDURE [dbo].[ProcName]');
END
GO
CREATE PROCEDURE [dbo].[ProcName] AS
SELECT 1 as Id
GO

-- ALTER SYNTAX FORM
IF OBJECT_ID('[dbo].[ProcName]', 'P') IS NULL BEGIN
	PRINT '[dbo].[ProcName] did not exist - creating dummy proc so ALTER will succeed'
	EXEC('CREATE PROCEDURE [dbo].[ProcName] AS SELECT 1 as Id')
END
GO
ALTER PROCEDURE [dbo].[ProcName] AS
SELECT 1 as Id
GO

-- =====================================================================================
-- INLINE TABLE VALUED FUNCTIONS - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[FunctionName]', 'IF') IS NOT NULL BEGIN
	PRINT 'Dropping ITVF [dbo].[FunctionName]'
	EXEC('DROP FUNCTION [dbo].[FunctionName]')
END
GO
CREATE FUNCTION [dbo].[FunctionName]()
RETURNS TABLE AS RETURN 
(
SELECT 1 as Id
)
GO

-- ALTER SYNTAX FORM
IF OBJECT_ID('[dbo].[FunctionName]', 'IF') IS NULL BEGIN
	PRINT '[dbo].[FunctionName] did not exist - creating dummy ITVF so ALTER will succeed'
	EXEC('CREATE FUNCTION [dbo].[FunctionName] RETURNS TABLE AS RETURN (SELECT 1 as Id)')
END
GO
ALTER FUNCTION [dbo].[FunctionName]()
RETURNS TABLE AS RETURN 
(
SELECT 1 as Id
)
GO


-- =====================================================================================
-- COLUMNS - CREATE/ALTER SYNTAX FORM.
-- THESE ALL WORK ON TEMP TABLES TOO - JUST USE '#TempTableName#' instead of [dbo.[TableName]
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NULL BEGIN
	PRINT 'Adding column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ADD [ColumnName] NVARCHAR(MAX)
END ELSE BEGIN
	PRINT 'Altering column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ALTER COLUMN [ColumnName] NVARCHAR(MAX)
END

-- DROP FORM (WILL FAIL IF THERE ARE CONSTRAINTS OR INDEXES ON THE COLUMN).
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NOT NULL BEGIN
	PRINT 'Dropping column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] DROP COLUMN [ColumnName];
END

-- ADD NON-NULLABLE COLUMN WITH NAMED DEFAULT.
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NULL BEGIN
	PRINT 'Adding column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ADD [ColumnName] NVARCHAR(MAX) NOT NULL
		CONSTRAINT [DF_TableName_ColumnName] DEFAULT 'Hello World'
END

-- ADD COMPUTED COLUMN
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NULL BEGIN
	PRINT 'Adding column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ADD [ColumnName] AS 'Hello ' + cast(ID AS VARCHAR)
END


select * from dbo.TableName
insert into dbo.TableName values (4)

-- =====================================================================================
-- INDEXES - The web is ambiguous over whether you can alter the columns in an index.
--           This says you need Enterprise Edition: http://www.sqlservercentral.com/Forums/Topic913722-391-1.aspx
--           But this says nothing about that: http://msdn.microsoft.com/en-us/library/ms188783.aspx
-- THESE ALL WORK ON TEMP TABLES TOO - JUST USE '#TempTableName#' instead of [dbo].[TableName]


-- To drop and recreate.
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_Name' AND object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
	DROP INDEX [dbo].[TableName].[IDX_Name];
END
CREATE UNIQUE INDEX IDX_Name on [dbo].[TableName](Col1, Col2, Col3);


-- DROP_EXISTING variant - use of this option requires that the index exists!
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_Name' AND object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
	CREATE UNIQUE INDEX IDX_Name on [dbo].[TableName](Col1, Col2, Col3) WITH (DROP_EXISTING = ON, ONLINE = ON);
END ELSE BEGIN
	CREATE UNIQUE INDEX IDX_Name on [dbo].[TableName](Col1, Col2, Col3);
END


-- =====================================================================================
-- CONSTRAINTS - You cannot ALTER an existing constraint so all examples use DROP/CREATE.

-- DEFAULT CONSTRAINTS.
IF EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_TableName_ConsName' AND parent_object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
	ALTER TABLE [dbo].[TableName] DROP CONSTRAINT DF_TableName_ConsName
END
ALTER TABLE [dbo].[TableName] ADD CONSTRAINT DF_TableName_ConsName DEFAULT (10.0) FOR [ColumnName];


-- CHECK CONSTRAINTS.
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_TableName_ConsName' AND parent_object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
	ALTER TABLE [dbo].[TableName] DROP CONSTRAINT Ck_TableName_ConsName
END
ALTER TABLE [dbo].[TableName] ADD CONSTRAINT CK_TableName_ConsName CHECK (ColumnName >= 10);


-- PRIMARY KEY CONSTRAINTS.
IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_TableName' AND type = 'PK' AND parent_object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
	ALTER TABLE [dbo].[TableName] DROP CONSTRAINT PK_TableName;
END
ALTER TABLE [dbo].[TableName] ADD CONSTRAINT [PK_TableName] PRIMARY KEY CLUSTERED([ID]);


