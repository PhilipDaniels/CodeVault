select top 10 * from sysjobs where name like '%export%'
select top 10 * from sysjobsteps
select top 100 * from sysjobhistory

-- instance_id, job_id, step_id, step_name, sql_message_id, sql_severity, message, run_status, run_date, run_time, run_duration
-- operator_id_emailed, operator_id_netsent, operator_id_paged, retries_attempted server                         

SELECT * FROM SYSJOBHISTORY
SELECT * FROM SYSJOBS

select * from j
select * from jh

------------------------------------------------------------------------------------

CREATE TABLE #JobSteps
  (
  PKID		INT IDENTITY PRIMARY KEY,
  JobID 	UNIQUEIDENTIFIER NOT NULL,
  JobName	SYSNAME NOT NULL,
  StepID	INT NOT NULL,
  StepName	VARCHAR(128) NULL,
  StartDate 	DATETIME NULL,
  EndDate 	DATETIME NULL  
  )

-- Get every job step, with its start and end time.
INSERT INTO #JobSteps (JobID, JobName, StepID, StepName, StartDate, EndDate)
SELECT
	J.JOB_ID, 
	J.NAME,
	JH.STEP_ID,
	JH.STEP_NAME,
	-- This lot is the start time of the step.
	SUBSTRING(CAST(JH.RUN_DATE AS CHAR(8)), 1, 4) +	'-' +	-- Year
	SUBSTRING(CAST(JH.RUN_DATE AS CHAR(8)), 5, 2) + '-' +	-- Month
	SUBSTRING(CAST(JH.RUN_DATE AS CHAR(8)), 7, 2) + ' ' +	-- Day
	SUBSTRING(RIGHT('000000' + CAST(JH.RUN_TIME AS VARCHAR), 6), 1, 2) + ':' + 		-- Hours
	SUBSTRING(RIGHT('000000' + CAST(JH.RUN_TIME AS VARCHAR), 6), 3, 2) + ':' + 		-- Minutes
	SUBSTRING(RIGHT('000000' + CAST(JH.RUN_TIME AS VARCHAR), 6), 5, 2) AS StartDate,	-- Seconds

	-- This lot is the end time of the step (same again plus the run duration)
	-- The format of the RUN_DURATION field is HHMMSS but it is not padded by default. eg it can be "0", "1", "12", "731" etc
        DATEADD(ss, CAST(SUBSTRING(RIGHT('000000' + CAST(JH.RUN_DURATION AS VARCHAR), 6), 5, 2) AS INT),
        DATEADD(mi, CAST(SUBSTRING(RIGHT('000000' + CAST(JH.RUN_DURATION AS VARCHAR), 6), 3, 2) AS INT),
        DATEADD(hh, CAST(SUBSTRING(RIGHT('000000' + CAST(JH.RUN_DURATION AS VARCHAR), 6), 1, 2) AS INT),
		CAST (
		SUBSTRING(CAST(JH.RUN_DATE AS CHAR(8)), 1, 4) +	'-' +	-- Year
		SUBSTRING(CAST(JH.RUN_DATE AS CHAR(8)), 5, 2) + '-' +	-- Month
		SUBSTRING(CAST(JH.RUN_DATE AS CHAR(8)), 7, 2) + ' ' +	-- Day
		SUBSTRING(RIGHT('000000' + CAST(JH.RUN_TIME AS VARCHAR), 6), 1, 2) + ':' + 	-- Hours
		SUBSTRING(RIGHT('000000' + CAST(JH.RUN_TIME AS VARCHAR), 6), 3, 2) + ':' + 	-- Minutes
		SUBSTRING(RIGHT('000000' + CAST(JH.RUN_TIME AS VARCHAR), 6), 5, 2) 		-- Seconds
		AS DATETIME)))) AS EndDate
FROM 
	SYSJOBS J
	INNER JOIN SYSJOBHISTORY JH ON J.JOB_ID = JH.JOB_ID
WHERE
--	J.NAME LIKE '%EXPORT%'
--	AND
JH.STEP_ID <> 0 	-- JobStep 0 appears to be a "dummy step" and is not the real start time of the job.

DROP TABLE #JobSteps

-- Now Report on the results. 

-- The EndDate of the job is the EndDate of the step which is the
-- highest end date of all the steps before the next start step.



SELECT
	JS.JobName,
	JS.StartDate,
	(SELECT MAX(EndDate) FROM #JobSteps JS2 
		WHERE JS.JobID = JS2.JobID
		AND JS2.StartDate > JS.StartDate 
		AND JS2.StartDate <= (SELECT MIN(StartDate)
				      FROM #JobSteps JS3
				      WHERE JS2.JobID = JS3.JobID AND JS3.StepID = J.START_STEP_ID AND JS3.StartDate > JS2.StartDate)
		) As JobEndDate
FROM
	SYSJOBS J
	INNER JOIN #JobSteps JS ON J.JOB_ID = JS.JobID
WHERE
	JS.StepID = J.START_STEP_ID
	AND JS.StartDate IS NOT NULL
	--AND JS.JobName LIKE '%shrink%' 
ORDER BY
	1, 2

-- V2.
SELECT
	JS1.JobName,
	JS1.StartDate,
	(SELECT MAX(EndDate) FROM #JobSteps JS2 
		WHERE JS1.JobID = JS2.JobID
		AND JS2.StartDate = (SELECT MAX(StartDate) FROM #JobSteps JS3
				     WHERE JS3.JobID = JS2.JobID AND JS3.StartDate > JS1.StartDate
				     AND JS3.StartDate < '2999-12-31'
					)
		) As JobEndDate
FROM
	SYSJOBS J
	INNER JOIN #JobSteps JS1 ON J.JOB_ID = JS1.JobID
WHERE
	JS1.StepID = J.START_STEP_ID
	AND JS1.StartDate IS NOT NULL
	--AND JS.JobName LIKE '%shrink%' 
ORDER BY
	1, 2


-- V3.
-- Join all the start steps to all the other steps, then just select the ones with the 
SELECT
	JS1.JobName,
	JS1.StartDate AS JS1Start,
	JS2.EndDate AS JS2End
FROM
	SYSJOBS J
	INNER JOIN #JobSteps JS1 ON J.JOB_ID = JS1.JobID
	-- JS2 gives us the rows that may be terminating the job.
	INNER JOIN #JobSteps JS2 ON JS1.JobID = JS2.JobID AND JS2.StartDate >= JS1.StartDate -- >= because may be one step. Rows that may end it.
	AND 
		(
		-- Find the EndDate that is the latest EndDate while still being lower than the next run (if there is one)
		JS2.EndDate = (SELECT MAX(EndDate) FROM #JobSteps JS3 WHERE JS1.JobID = JS3.JobID 
			       AND JS3.EndDate <= (SELECT MIN(StartDate)
				      FROM #JobSteps JS4
				      WHERE JS3.JobID = JS4.JobID AND JS4.StepID = J.START_STEP_ID AND JS4.StartDate > JS1.StartDate)
			       )
--		OR
--		-- The last job does not have a successor.
--		JS2.EndDate = (SELECT MAX(EndDate) FROM #JobSteps JS3 WHERE JS1.JobID = JS3.JobID )
		)
WHERE
	JS1.StepID = J.START_STEP_ID
	AND JS1.JobName LIKE 'Check Object Status' 
--	AND JS1.StartDate = '2003-08-24 21:00:01.000'
	--AND JS1.StartDate = '2003-09-01 08:51:20.000'
ORDER BY
	1, 2


-- V4.
SELECT
	JS1.JobName,
	JS1.StartDate AS JS1Start, JS1.StartDate, JS2.EndDate AS JS2End
FROM
	SYSJOBS J
	INNER JOIN #JobSteps JS1 ON J.JOB_ID = JS1.JobID
	-- JS2 gives us the rows that may be terminating the job.
	INNER JOIN #JobSteps JS2 ON JS1.JobID = JS2.JobID
WHERE
	JS1.StepID = J.START_STEP_ID
	AND JS2.StepID = J.START_STEP_ID
	AND JS1.JobName LIKE 'Check Object Status' 
--	AND JS1.StartDate = '2003-08-24 21:00:01.000'
	--AND JS1.StartDate = '2003-09-01 08:51:20.000'
ORDER BY
	1, 2, 3






SELECT * FROM #JobSteps

where jobname like 'Check Object Status'
order by jobname, pkid





