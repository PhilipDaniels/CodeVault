
-- In SQL 2012 it is much easier.
select *
from sys.objects
order by object_id
offset 10 rows				-- Skip 10 rows.
fetch next 15 rows only		-- And fetch 15 rows. This is optional, without it all remaining rows will be fetched.




/*
declare 
    @page_size INT = 20
    , @page_num INT = 20

SET NOCOUNT ON;

; WITH RESULTS AS
(
    SELECT
		*,
        ROW_NUMBER() OVER (ORDER BY dtASRSalesInv ASC, PBLNR ASC, TNKNR ASC, cRecId ASC) AS rn,
        ROW_NUMBER() OVER (ORDER BY dtASRSalesInv DESC, PBLNR DESC, TNKNR DESC, cRecId DESC) AS rn_reversed
    FROM SAP.ASRSalesInv
)
SELECT *
    , CAST(rn + rn_reversed - 1 AS INT) AS total_rows
    , CAST(CASE (rn + rn_reversed - 1) % @page_size
        WHEN 0 THEN (rn + rn_reversed - 1) / @page_size
        ELSE ((rn + rn_reversed - 1) / @page_size) + 1 
        END AS INT) AS total_pages
FROM RESULTS a
WHERE a.rn BETWEEN 1 + ((@page_num - 1) * @page_size) AND @page_num * @page_size
ORDER BY rn ASC 

select * from adm.vTableSize order by NuMRows desc
select top 10 * from [CSO_Main].[SIF].[OutASRVolumes]
sp_help 'SIF.OutASRVolumes'

select count(*) from SIF.OutASRVolumes
*/


/*
declare 
    @page_size INT = 20
    , @page_num INT = 20

SET NOCOUNT ON;

; WITH RESULTS AS
(
    SELECT
		*,
        ROW_NUMBER() OVER (ORDER BY nRowId ASC) AS rn,
        ROW_NUMBER() OVER (ORDER BY nRowId DESC) AS rn_reversed
    FROM SIF.OutASRVolumes
)
SELECT *
    , CAST(rn + rn_reversed - 1 AS INT) AS total_rows
    , CAST(CASE (rn + rn_reversed - 1) % @page_size
        WHEN 0 THEN (rn + rn_reversed - 1) / @page_size
        ELSE ((rn + rn_reversed - 1) / @page_size) + 1 
        END AS INT) AS total_pages
FROM RESULTS a
WHERE a.rn BETWEEN 1 + ((@page_num - 1) * @page_size) AND @page_num * @page_size
ORDER BY rn ASC 
*/


--select * from adm.vtablesize order by numrows desc
declare @page_size int = 20, @page_num int = 1;

;with Results as
	(
	-- Query to page
	select
		*,
		row_number() over (order by ScenarioId, LogicalTankid, dtPeriodStart) as nRowNumber
	from
		AUT.ScInvForecast
	)
select
	*,
	(select count(*) from Results) as nTotalRows
from
	Results R
where
	R.nRowNumber between 1 + ((@page_num - 1) * @page_size) and @page_num * @page_size
order by
	R.nRowNumber asc 





CREATE PROCEDURE [dbo].[StoredProcName] 
    @page_size INT
    , @page_num INT
AS
BEGIN

    SET NOCOUNT ON;

    ; WITH RESULTS AS
    (
        SELECT *
            , ROW_NUMBER() OVER (ORDER BY <order_col> DESC) AS rn
            , ROW_NUMBER() OVER (ORDER BY <order_col> ASC) AS rn_reversed
        FROM <table>
    )
    SELECT *
        , CAST(rn + rn_reversed - 1 AS INT) AS total_rows
        , CAST(CASE (rn + rn_reversed - 1) % @page_size
            WHEN 0 THEN (rn + rn_reversed - 1) / @page_size
            ELSE ((rn + rn_reversed - 1) / @page_size) + 1 
            END AS INT) AS total_pages
    FROM RESULTS a
    WHERE a.rn BETWEEN 1 + ((@page_num - 1) * @page_size) AND @page_num * @page_size
    ORDER BY rn ASC 

END

