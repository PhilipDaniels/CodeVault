
CREATE PROCEDURE #adm_ProcSearch
	@cSearch1	VarChar(255)
	,@cSearch2	VarChar(255) = ''
	,@cSearch3	varchar(255) = ''
AS

if @cSearch3 <> ''
begin
	select distinct
			'exec sp_helptext [' + S.name + '.' + left(P.name, 60) + ']' as 'Object'
		from
			sys.objects as P
			inner join sys.sql_modules as C
				on C.object_id = P.object_id
				and C.definition like '%' + @cSearch1 +'%'
				and C.definition like '%' + @cSearch2 +'%'
				and C.definition like '%' + @cSearch3 +'%'
			inner join sys.schemas as S
				on S.schema_id = P.schema_id
		where
			P.type in ('P','TR','FN','TF','V')
			and left(P.name,3) <> 'dt_'
		order by
			1
end
else if @cSearch2 <> ''
begin
	select distinct
		'exec sp_helptext [' + S.name + '.' + left(P.name, 60) + ']' as 'Object'
	from
		sys.objects as P
		inner join sys.sql_modules as C
			on C.object_id = P.object_id
			and C.definition like '%' + @cSearch1 +'%'
			and C.definition like '%' + @cSearch2 +'%'
		inner join sys.schemas as S
			on S.schema_id = P.schema_id
	where
		P.type in ('P','TR','FN','TF','V')
		and left(P.name,3) <> 'dt_'
	order by
		1
end
else
begin
	select distinct
		'exec sp_helptext [' + S.name + '.' + left(P.name, 60) + ']' as 'Object'
	from
		sys.objects as P
		inner join sys.sql_modules as C
			on C.object_id = P.object_id
			and C.definition like '%' + @cSearch1 +'%'
		inner join sys.schemas as S
			on S.schema_id = P.schema_id
	where
		P.type in ('P','TR','FN','TF','V')
		and left(P.name,3) <> 'dt_'
	order by
		1
end
