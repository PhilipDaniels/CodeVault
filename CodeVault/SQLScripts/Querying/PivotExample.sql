use webcat
set transaction isolation level read uncommitted

declare @NumDaysBack int, @StartDateForTracking datetime, @PivotColumns nvarchar(max), @SQL nvarchar(max)
set @NumDaysBack = 30
set @StartDateForTracking = cast(convert(char(8), dateadd(day, -(@NumDaysBack - 1), getdate()), 112) as datetime)

if object_id('tempdb..#Days') is not null drop table #Days
create table #Days
	(
	TheDay smalldatetime primary key, 
	TheDayString as convert(char(10), TheDay, 120),
	DayName as datename(weekday, TheDay)
	)
if object_id('tempdb..#LanId') is not null drop table #LanId
create table #LanId(LanId nvarchar(50) primary key)

insert into #LanId (LanId)
--select distinct upper(PR.LanId) from IIS.vPageRequest PR where PR.RequestDay >= @StartDateForTracking
select upper(LanId) from WC.Person where LanId in ('EA\AMARCOT', 'EA\PBOKOR', 'EA\CSIMON', 'EA\CDML', 'NA\MJLUC1', 'NA\SMTAN', 'NA\SACRIMI', 'NA\PDANIE1', 'NA\GALLEN')
union select suser_sname()
order by 1

insert into #Days(TheDay)
select dateadd(day, N.Number, @StartDateForTracking)
from IIS.Number N
where N.Number < @NumDaysBack

select @PivotColumns = coalesce(@PivotColumns + ',' + quotename(LanId), quotename(LanId)) from #LanId order by LanId


-- "PIVOT rotates a table-valued expression (PivotData) by *turning* the unique values from one column (LanId in this case) into 
-- multiple columns in the output, and performs aggregations where they are required on any remaining column values that are 
-- wanted in the final output.
--
-- So in this case the input is PivotData, the columns LanId and PageRequestId are removed from the final output.
set @SQL = 
'
select
	*
from
	(
	select
		D.*, L.LanId, PR.PageRequestId
	from 
		#Days D
		join IIS.vPageRequest PR on D.TheDay = PR.RequestDay
		join #LanId L on PR.LanId = L.LanId
	) PivotData
	PIVOT
	(
		count(PageRequestId)
		for LanId in (' + @PivotColumns+ ')
	) PivotTable
order by
	TheDay desc
'

exec (@SQL)

/*
Non dynamic version
select
	*
from
	(
	select
		D.*, L.LanId, PR.PageRequestId
	from 
		#Days D
		join IIS.vPageRequest PR on D.TheDay = PR.RequestDay
		join #LanId L on PR.LanId = L.LanId
	) PivotData
	PIVOT
	(
		count(PageRequestId)
		for LanId in ([EA\PBOKOR], [EA\CDML], [NA\GALLEN])
	) PivotTable
order by
	TheDay desc
*/


/*
Old style version

select
	convert(char(10), dateadd(day, N.Number, @StartDateForTracking), 120) as Day,
	datename(weekday, dateadd(day, N.Number, @StartDateForTracking)) as DayName,
	sum(case when PR.LanId = 'EA\AMARCOT' then 1 else 0 end) as [EA\AMARCOT],
	sum(case when PR.LanId = 'EA\PBOKOR' then 1 else 0 end) as [EA\PBOKOR],
	sum(case when PR.LanId = 'EA\CSIMON' then 1 else 0 end) as [EA\CSIMON],

	sum(case when PR.LanId = 'NA\PDANIE1' then 1 else 0 end) as [NA\PDANIE1],
	sum(case when PR.LanId = 'NA\GALLEN' then 1 else 0 end) as [NA\GALLEN],

	sum(case when PR.LanId = 'EA\CDML' then 1 else 0 end) as [EA\CDML],
	sum(case when PR.LanId = 'NA\MJLUC1' then 1 else 0 end) as [NA\MJLUC1],
	sum(case when PR.LanId = 'NA\SMTAN' then 1 else 0 end) as [NA\SMTAN],
	sum(case when PR.LanId = 'NA\SACRIMI' then 1 else 0 end) as [NA\SACRIMI]
from
	IIS.Number N
	left join IIS.vPageRequest PR on dateadd(day, N.Number, @StartDateForTracking) = PR.RequestDay
		and PR.LanId in ('EA\AMARCOT', 'EA\PBOKOR', 'EA\CSIMON', 'EA\CDML', 'NA\MJLUC1', 'NA\SMTAN', 'NA\SACRIMI', 'NA\PDANIE1', 'NA\GALLEN')
where
	N.Number < @NumDaysBack
group by 
	dateadd(day, N.Number, @StartDateForTracking)
order by
	dateadd(day, N.Number, @StartDateForTracking) desc

*/