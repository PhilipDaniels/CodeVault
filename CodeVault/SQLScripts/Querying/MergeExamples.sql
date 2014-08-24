-- ========================================================================
-- Example 1. Nice and simple.
merge into dbo.AccountType as T
using
	(
			select 1, 'Name1', 'Desc 1'
	union	select 2, 'Name2', 'Desc 2'
	union	select 3, 'Name3', 'Desc 3'
	) 
as S(ID_AccountType, Name, Description)
on T.ID_AccountType = S.ID_AccountType
when not matched then
	insert (ID_AccountType, Name, Description)
	values (S.ID_AccountType, S.Name, S.Description)
when matched then
	update set
		T.Name = S.Name,
		T.Description = S.Description;

-- ========================================================================
-- Example 2.


merge into SAP.ASRForecasts as T
	using
		(	
		select
			T.PBLNR, T.MATNR, T.TNKNR, D.dtDate as AUDAT, 
			null as STARTINV,
			null as FSALESVOL,
			null as PDELVVOL,
			O.nHoursOpen * t1.SalesPerHour as SUMSALES,
			'M3' as VOLUOM
		from
			AUT.V_Tanks T
			join CSO.Dates D on D.dtDate between dateadd(day, 0, getdate()) and dateadd(day, 7, getdate())
			join CSO.V_OpeningHours O on O.PBLNR = T.PBLNR and O.nWeekday = datepart(weekday, D.dtDate)
			join
				(
				select
					FR.*
				from   
					(
					select
						ScenarioId, LogicalTankId, nWeekday
					from
						AUT.ScForecastRate   
					where
						ScenarioId = @ScenarioID
					group by
						ScenarioId, LogicalTankId, nWeekday   
					) LastRecs	
					join AUT.ScForecastRate FR on LastRecs.ScenarioId = FR.ScenarioId
						and LastRecs.LogicalTankId = FR.LogicalTankId
						and LastRecs.nWeekday = FR.nWeekday
				) t1 on t1.LogicalTankId = T.LogicalTankId and t1.nWeekday = datepart(weekday, D.dtDate)
		group by 
			T.PBLNR, T.MATNR, T.TNKNR, D.dtDate, O.nHoursOpen, t1.SalesPerHour
		) as S 
			(
			PBLNR,
			MATNR,
			TNKNR,
			AUDAT,
			STARTINV,
			FSALESVOL,
			PDELVVOL,
			SUMSALES,
			VOLUOM
			)
	on
		(T.PBLNR = S.PBLNR and T.MATNR = S.MATNR and S.TNKNR = T.TNKNR and T.AUDAT = S.AUDAT)
	when not matched then
		insert (PBLNR, MATNR, TNKNR, AUDAT, STARTINV, FSALESVOL, PDELVVOL, SUMSALES, VOLUOM) 
		values (S.PBLNR, S.MATNR, S.TNKNR, S.AUDAT, S.STARTINV, S.FSALESVOL, S.PDELVVOL, S.SUMSALES, S.VOLUOM)
	when matched then
		update set 
			T.STARTINV = S.STARTINV,
			T.FSALESVOL = S.FSALESVOL,
			T.PDELVVOL = S.PDELVVOL,
			T.SUMSALES = S.SUMSALES,
			T.VOLUOM = S.VOLUOM;
