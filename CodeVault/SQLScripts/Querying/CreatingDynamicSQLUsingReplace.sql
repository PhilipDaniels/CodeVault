-- n.b. You will need to do results to text to get the newline.

-- one replace:
select
	REPLACE(
		'insert into #Backup(TankId, PBLNR, TNKNR, MATNR, nCapacity, nHeel, nOverillProtection, DynamicMinHours, DynamicMaxHours) ' + char(13) + char(10) +
		'    values (@TID, ''@PBL'', ''@TNK'', ''@MAT'', @CAP, @HEEL, @OVER, @DMIN, @DMAX)',
		'@TID', T.TankId)
	as SQL
from
	whatever

-- For more add an extra replace at the front and copy the last insert (you will need to add a comma at the end of the previous line.
select
	REPLACE(REPLACE(
		'insert into #Backup(TankId, PBLNR, TNKNR, MATNR, nCapacity, nHeel, nOverillProtection, DynamicMinHours, DynamicMaxHours) ' + char(13) + char(10) +
		'    values (@TID, ''@PBL'', ''@TNK'', ''@MAT'', @CAP, @HEEL, @OVER, @DMIN, @DMAX)',
		'@TID', T.TankId),
		'@PBL', T.PBLNR)
from
	whatever

-- You need to be careful with NULLS.
select
	REPLACE(REPLACE(REPLACE(
		'insert into #Backup(TankId, PBLNR, TNKNR, MATNR, nCapacity, nHeel, nOverillProtection, DynamicMinHours, DynamicMaxHours) ' + char(13) + char(10) +
		'    values (@TID, ''@PBL'', ''@TNK'', ''@MAT'', @CAP, @HEEL, @OVER, @DMIN, @DMAX)',
		'@TID', T.TankId),
		'@PBL', T.PBLNR),
		'@CAP', coalesce(cast(T.nCapacity as varchar), 'null'))		-- can use a coalesce or case .. end.
from
	whatever


-- General form for dynamic SQL generation wit REPLACE.
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
		'insert into #Backup(TankId, PBLNR, TNKNR, MATNR, nCapacity, nHeel, nOverillProtection, DynamicMinHours, DynamicMaxHours) ' + char(13) + char(10) +
		'    values (@TID, ''@PBL'', ''@TNK'', ''@MAT'', @CAP, @HEEL, @OVER, @DMIN, @DMAX)',
		'@TID', T.TankId),
		'@PBL', T.PBLNR),
		'@TNK', T.TNKNR),
		'@MAT', T.MATNR),
		'@CAP', case when T.nCapacity is null then 'null' else CAST(T.nCapacity as varchar) end),
		'@HEEL', case when T.nHeel is null then 'null' else CAST(T.nHeel as varchar) end),
		'@OVER', case when T.nOverfillProtection is null then 'null' else CAST(T.nOverfillProtection as varchar) end),
		'@DMIN', case when T.DynamicMinHours is null then 'null' else CAST(T.DynamicMinHours as varchar) end),
		'@DMAX', case when T.DynamicMaxHours is null then 'null' else CAST(T.DynamicMaxHours as varchar) end)
	as SQL
