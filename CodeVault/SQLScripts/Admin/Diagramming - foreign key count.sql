-- The following query will tell you how many foreign keys on each table
-- in your database, higner numbers will have more lines on the diagram
-- and should be added first. Tables with 0 or 1 foreign key are
-- trivial to place on the diagram without overlapping lines.


;with NDL as (
	select
		X.SchemaName, X.TableName, count(*) as NumDiagramLines
	from
		(
		select
			schema_name(O.schema_id) as SchemaName,
			object_name(FKOBJ.ObjectId) as TableName,
			FKOBJ.ObjectId as ObjectId
		from
			(
			select parent_object_id as ObjectId from sys.foreign_key_columns
			union all select referenced_object_id from sys.foreign_key_columns
			) FKOBJ
			join sys.objects O on FKOBJ.ObjectId = O.object_id
		) X
	group by
		X.SchemaName, X.TableName
	)
select
	SCH.Name, T.name, coalesce(NDL.NumDiagramLines, 0) as NumDiagramLines
from
	sys.tables T
	cross apply (select schema_name(T.Schema_Id) as Name) SCH
	left join NDL on SCH.Name = NDL.SchemaName and T.name = NDL.TableName
order by
	coalesce(NDL.NumDiagramLines, 0) desc, SCH.Name, T.Name

