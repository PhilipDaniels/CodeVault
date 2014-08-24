select 
	*, StartTime, TextData, NTUserName, HostName 
from 
	::fn_trace_gettable('F:\SQL2005\MSSQL\SERVERLOG\log_54.trc', default)
	join sys.trace_events on eventclass = trace_event_id
where 
	eventclass = 20;
