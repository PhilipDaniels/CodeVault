select distinct
	'exec sp_helptext [' + S.name + '.' + left(P.name, 60) + ']' as 'Object'
from
	sys.objects as P
	inner join sys.sql_modules as C on C.object_id = P.object_id
	inner join sys.schemas as S on S.schema_id = P.schema_id
where
	P.type in ('P','TR','FN','TF','V')
	and left(P.name,3) <> 'dt_'
	and C.definition like '%Schema.Whatever%'
order by
	1
