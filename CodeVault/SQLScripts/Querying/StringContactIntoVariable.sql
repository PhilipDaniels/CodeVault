
-- The way this works is that the line "select @s = isnull(@s+',','') + cast(employeeid as varchar)"
-- is run multiple times, once for each row in x. The variable s is repeatedly appended to.
-- Try "set rowcount 3" to see it in action.

set rowcount 0


declare @s varchar(8000)

select @s = isnull(@s+',','') + cast(employeeid as varchar)
from
	(select top 100 percent employeeid
	from northwind..employees
	order by employeeid) x

select @s
