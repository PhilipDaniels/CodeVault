-- ===============================================================================================
-- List permissions of all objects (if any).
select
	SO.type,
	SO.type_desc,
	SS.name as [Schema],
	SO.name as [Object],
	PRIN.name as username,
	PERM.type as permissions_type,
	PERM.permission_name as permission_name,
	PERM.state as permission_state,
	PERM.state_desc as state_desc,
	PERM.state_desc + ' ' + PERM.permission_name + ' on ['+ SS.name + '].[' + SO.name + '] to [' + PRIN.name + ']' COLLATE LATIN1_General_CI_AS 
		as permissionsql
from
	sys.objects SO
	join sys.schemas SS on SO.schema_id = SS.schema_id 
	left join sys.database_permissions PERM on SO.object_id  = PERM.major_id
	left join sys.database_principals PRIN on PERM.grantee_principal_id =  PRIN.principal_id 
where
	SO.type in ('AF', 'FN', 'FS', 'FT', 'IF', 'P', 'PC', 'TF', 'U', 'V', 'X')
	and (PERM.type is null or SS.name = 'AUT')
	and SS.NAME not in ('ADM', 'BRA', 'SIF')
order by
	1, 2, 3, 4, 5, 6, 7


/*
From MSDN:

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
