declare
	@DbName varchar(128),
	@SQL nvarchar(max),
	@OriginalSQL nvarchar(max),
	@WantViewsInColumnInfo bit = 0;

declare
	@Databases table(Name varchar(128) primary key);


-- ACLASQL02.
insert into @Databases(Name) values ('C3Admin'), ('C3Logs'), ('C3Tools'), ('LAAdmin'), ('LAMailer'), ('LMATransfer'), ('M3Admin'), ('M3AreaGuide'), ('M3Errors'), ('M3Listings'), ('M3Logs'), ('M3SEO'), ('M3SPV');
-- ACSQL06.
--insert into @Databases(Name) values ('C3Admin'), ('C3CMA'), ('C3Logs'), ('C3Tools'), ('Email_Processing'), ('LAMailer'), ('LMATransfer'), ('M3Admin'), ('M3AreaGuide'), ('M3Errors'), ('M3Listings'), ('M3Logs'), ('M3Mailer'), ('M3SEO'), ('M3SPV');
-- Photofeeds.
--insert into @Databases(Name) values ('M3Importer'), ('M3ImportVebra'), ('URLDownloader2'), ('URLDownloaderPDF');
-- ACCALDEV1.
--insert into @Databases(Name) values ('C3Admin'), ('C3Logs'), ('C3Tools'), ('LAAdmin'), ('LAMailer'), ('M3Admin'), ('M3AreaGuide'), ('M3Errors'), ('M3Listings'), ('M3Logs'), ('M3SEO'), ('M3SPV'),
--	('M3Importer'), ('M3Importer_Dev');

--insert into @Databases(Name) values ('M3Importer')




-- ===============================================================================================================================================================================
-- SECTION 1 : Table sizes.

declare @TableSize Table
	(
	ObjectId int, DateCreated datetime, DateModified datetime,
	[Database] nvarchar(128), [Schema] nvarchar(128), [Name] nvarchar(128), Fullname nvarchar(255), [Type] nvarchar(128),
	NumRows int, ReservedKB int, DataKB int, IndexKB int, UnusedKB int, MDFKB int, LDFKB int,
	PercentMDF decimal(20,10), PercentIndex decimal(20,10), KBPerRow decimal(20,10), MBPerMillionRows decimal(20,10),
	ReservedPages int, DataPages int, IndexPages int, UnusedPages int,
	PrimaryKey nvarchar(128),
	NumUniqueConstraints int,
	HasClusteredIndex tinyint,
	NumNonClusteredIndexes int,
	NumForeignKeys int,
	NumCheckConstraints int,
	NumDefaultConstraints int,
	IdentityColumn nvarchar(128),
	RowGuidColumn nvarchar(128)
	);

set @OriginalSQL = '
select 
	X.ObjectId, X.DateCreated, X.DateModified,
	X.[Database], X.[Schema], X.Name,
	quotename(X.[Database]) + ''.'' + quotename(X.[Schema]) + ''.'' + quotename(X.Name) as Fullname,
	X.Type,
	X.NumRows, X.ReservedKB, X.DataKB, X.IndexKB, X.UnusedKB,
	DatabaseSize.MDFKB,
	DatabaseSize.LDFKB,
	100.0 * X.ReservedKB / MDFKB as PercentMDF,
	case when X.ReservedKB = 0 then 0 else 100.0 * X.IndexKB / X.ReservedKB end as PercentIndex,
	case when X.NumRows = 0 then 0 else 1.0 * X.ReservedKB / X.NumRows end as KBPerRow,
	case when X.NumRows = 0 then 0 else 1024.0 * X.ReservedKB / X.NumRows end as MBPerMillionRows,
	X.ReservedPages, X.DataPages, X.IndexPages, X.UnusedPages,
	X.PrimaryKey,
	X.NumUniqueConstraints,
	X.HasClusteredIndex,
	X.NumNonClusteredIndexes,
	X.NumForeignKeys,
	X.NumCheckConstraints,
	X.NumDefaultConstraints,
	X.IdentityColumn,
	X.RowGuidColumn
from
	(
	SELECT
		A.object_id AS ObjectId,
		A.create_date AS DateCreated,
		A.modify_date AS DateModified,
		''??DbName??'' AS [Database],
		coalesce(SS.name, ''---'') AS [Schema],
		convert(sysname, A.name) AS [Name],
		case A.type
			when ''U'' THEN ''TABLE'' 
			when ''S'' THEN ''SYSTEM TABLE''
			when ''V'' THEN ''VIEW''
			else ''UNKNOWN''
		end AS [Type],
		INFO.NumRows AS NumRows,
		Info.ReservedPages AS ReservedPages,
		Info.ReservedPages * PageSize.InKb AS ReservedKB,
		Info.DataPages AS DataPages,
		Info.DataPages * PageSize.InKb AS DataKB,
		Info.ReservedPages - Info.DataPages - Info.UnusedPages AS IndexPages,
		(Info.ReservedPages - Info.DataPages - Info.UnusedPages) * PageSize.InKb AS IndexKB,
		Info.UnusedPages AS UnusedPages,
		Info.UnusedPages * PageSize.InKb AS UnusedKB,
		isnull(IndexInfo.PK, '''') AS PrimaryKey,
		isnull(IndexInfo.NumUniqueConstraints, 0) AS NumUniqueConstraints,
		(select count(*) from ??DbName??.sys.indexes SI where SI.object_id = A.object_id AND SI.index_id = 1) AS HasClusteredIndex,
		(select count(*) from ??DbName??.sys.indexes SI where SI.object_id = A.object_id AND SI.index_id not in (0, 1)) AS NumNonClusteredIndexes,
		isnull(IndexInfo.NumForeignKeys, 0) AS NumForeignKeys,
		isnull(IndexInfo.NumCheckConstraints, 0) AS NumCheckConstraints,
		isnull(IndexInfo.NumDefaultConstraints, 0) AS NumDefaultConstraints,
		-- There could be more than one Identity column, but that would be really rare.
		isnull((select min(SC.name) from ??DbName??.sys.columns SC where SC.object_id = A.object_id and SC.is_identity = 1), '''') AS IdentityColumn,
		-- Same pattern for simplicity, even though there can only be 1 RowGUID column.
		isnull((select min(SC.name) from ??DbName??.sys.columns SC where SC.object_id = A.object_id and SC.is_rowguidcol = 1), '''') AS RowGuidColumn
	from
		??DbName??.sys.objects A with (nolock)		-- User objects only
		left join ??DbName??.sys.schemas SS with (nolock) on A.schema_id = SS.schema_id
		cross apply
			(
			select
				low as InBytes,
				low / 1024 as InKb
			from
				master.dbo.spt_values with (nolock)
			where
				number = 1 and type = ''E''
			) PageSize
		join
			(
			select
				PS.object_id,
				sum(case when INDEX_ID IN (0, 1) then PS.ROW_COUNT else 0 end) as NumRows,
				sum(PS.RESERVED_PAGE_COUNT) AS ReservedPages,
				sum
					(
					case
						when INDEX_ID IN (0, 1)
						then PS.IN_ROW_DATA_PAGE_COUNT + PS.LOB_USED_PAGE_COUNT + PS.ROW_OVERFLOW_USED_PAGE_COUNT
						else 0 
					end
					) as DataPages,	-- num data pages in heap/CI
				sum(PS.RESERVED_PAGE_COUNT) - sum(PS.USED_PAGE_COUNT) AS UnusedPages
				-- Easy to generate IndexPages outside this subquery = ReservedPages - DataPages - UnusedPages
			from
				??DbName??.sys.dm_db_partition_stats PS with (nolock)
			group by
				PS.object_id
			) Info ON A.object_id = Info.object_id
		left join
			(
			select
				ChildObjects.parent_object_id,
				max(
					case when ChildObjects.type = ''PK'' then '''' else NULL end +
					case SI.[TYPE]
						when 0 then ''Heap''
						when 1 then ''Clustered'' 
						when 2 then ''Non-Clustered''
					end
					) AS PK,
				sum(case when ChildObjects.[type] = ''UQ'' then 1 else 0 end) as NumUniqueConstraints,
				sum(case when ChildObjects.[type] = ''F'' then 1 else 0 end) as NumForeignKeys,
				sum(case when ChildObjects.[type] = ''C'' then 1 else 0 end) as NumCheckConstraints,
				sum(case when ChildObjects.[type] = ''D'' then 1 else 0 end) as NumDefaultConstraints
			from
				??DbName??.sys.objects ChildObjects with (nolock)
				left join ??DbName??.sys.indexes SI with (nolock) on ChildObjects.parent_object_id = SI.object_id
					and SI.name = ChildObjects.name
			group by 
				ChildObjects.parent_object_id
			) IndexInfo on A.object_id = IndexInfo.parent_object_id
	where
		A.type IN (''U'') -- ''S'' includes system tables
		and Info.ReservedPages >= 0	
		and convert(sysname, A.name) <> ''sysdiagrams''
	) X
	cross apply
		(
		select
			sum(case when usage = ''data only'' then size else 0 end) as MDFKB,
			sum(case when usage != ''data only'' then size else 0 end) as LDFKB
		from
			(
			select 	
				convert(nvarchar(15), convert (bigint, DF.size) * 8) as Size,
				case when DF.data_space_id = 1 then ''data only'' else ''log only'' end as Usage
			from 
				??DbName??.sys.database_files DF
			) X
		) DatabaseSize
order by
	X.NumRows desc;
'



declare curDatabases cursor local static for select Name from @Databases
open curDatabases
while 1 = 1 begin
	fetch next from curDatabases into @DbName
	if @@fetch_status = -1 break
	set @SQL = replace(@OriginalSQL, '??DbName??', @DbName);
	--print @SQL
	insert into @TableSize(
		ObjectId , DateCreated , DateModified ,
		[Database] , [Schema] , [Name] , Fullname , [Type] ,
		NumRows , ReservedKB , DataKB , IndexKB , UnusedKB , MDFKB , LDFKB ,
		PercentMDF , PercentIndex , KBPerRow , MBPerMillionRows ,
		ReservedPages , DataPages , IndexPages , UnusedPages ,
		PrimaryKey ,
		NumUniqueConstraints ,
		HasClusteredIndex,
		NumNonClusteredIndexes, 
		NumForeignKeys,
		NumCheckConstraints ,
		NumDefaultConstraints ,
		IdentityColumn ,
		RowGuidColumn
		)
    exec (@SQL)
end

close curDatabases
deallocate curDatabases


select * from @TableSize order by ReservedKB desc;


-- ===============================================================================================================================================================================
-- SECTION 2 : Column schema information.

declare @ColumnInfo Table
	(
	TABLE_TYPE nvarchar(128),
	TABLE_CATALOG nvarchar(128),
	TABLE_SCHEMA nvarchar(128),
	TABLE_NAME nvarchar(128),
	COLUMN_NAME nvarchar(128),
	ORDINAL_POSITION int,
	COLUMN_DEFAULT nvarchar(128),
	IS_NULLABLE nvarchar(128),
	DATA_TYPE nvarchar(128),
	CHARACTER_MAXIMUM_LENGTH int,
	CHARACTER_OCTET_LENGTH int,
	NUMERIC_PRECISION int,
	NUMERIC_PRECISION_RADIX int,
	NUMERIC_SCALE int,
	DATETIME_PRECISION int,
	CHARACTER_SET_CATALOG nvarchar(128),
	CHARACTER_SET_SCHEMA nvarchar(128),
	CHARACTER_SET_NAME nvarchar(128),
	COLLATION_CATALOG nvarchar(128),
	COLLATION_SCHEMA nvarchar(128),
	COLLATION_NAME nvarchar(128),
	DOMAIN_CATALOG nvarchar(128),
	DOMAIN_SCHEMA nvarchar(128),
	DOMAIN_NAME nvarchar(128)
	);

set @OriginalSQL = 'select T.TABLE_TYPE, C.* from ??DbName??.INFORMATION_SCHEMA.COLUMNS C JOIN ??DbName??.INFORMATION_SCHEMA.TABLES T
	on C.TABLE_CATALOG = T.TABLE_CATALOG
	and C.TABLE_SCHEMA = T.TABLE_SCHEMA
	and C.TABLE_NAME = T.TABLE_NAME'

if @WantViewsInColumnInfo = 0 begin
	set @OriginalSQL = @OriginalSQL + ' and T.TABLE_TYPE = ''BASE TABLE'''
end


declare curDatabases cursor local static for select Name from @Databases
open curDatabases
while 1 = 1 begin
	fetch next from curDatabases into @DbName
	if @@fetch_status = -1 break
	set @SQL = replace(@OriginalSQL, '??DbName??', @DbName);
	--print @SQL

	insert into @ColumnInfo
		(
		TABLE_TYPE,
		TABLE_CATALOG,
		TABLE_SCHEMA,
		TABLE_NAME,
		COLUMN_NAME,
		ORDINAL_POSITION,
		COLUMN_DEFAULT,
		IS_NULLABLE,
		DATA_TYPE,
		CHARACTER_MAXIMUM_LENGTH,
		CHARACTER_OCTET_LENGTH,
		NUMERIC_PRECISION,
		NUMERIC_PRECISION_RADIX,
		NUMERIC_SCALE,
		DATETIME_PRECISION,
		CHARACTER_SET_CATALOG,
		CHARACTER_SET_SCHEMA,
		CHARACTER_SET_NAME,
		COLLATION_CATALOG,
		COLLATION_SCHEMA,
		COLLATION_NAME,
		DOMAIN_CATALOG,
		DOMAIN_SCHEMA,
		DOMAIN_NAME 
		)
    exec (@SQL)
end

close curDatabases
deallocate curDatabases

select * from @ColumnInfo 
order by TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION;

