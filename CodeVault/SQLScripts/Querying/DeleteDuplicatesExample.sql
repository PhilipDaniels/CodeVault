
-- Delete duplicates query.
-- You may need to add a fake ID column, such as an IDENTITY, in
-- order to uniquefiy rows.


DELETE
	H
FROM
	EmployeesToProjectWork H
	JOIN 
		(
		SELECT	
			MAX(nFakeID) AS nFakeID_KEEP,
			nEmployeeID, nProjectID, nRole, dtRateStart
		FROM 
			EmployeesToProjectWork
		GROUP BY 
			nEmployeeID, nProjectID, nRole, dtRateStart
		HAVING 
			COUNT(*) > 1
		) AS SQ ON H.nEmployeeID = SQ.nEmployeeID
			AND H.nProjectID = SQ.nProjectID
			AND H.nRole = SQ.nRole
			AND H.dtRateStart = SQ.dtRateStart
			-- Keep one row.
			AND H.nFakeID < SQ.nFakeID_KEEP
