/*
 * Solution for preventing overlapping date ranges in a table.
 * Given a table TABLENAME (TABLENAMEID, NK1, NK2, StartDate, EndDate)
 * where TABLENAMEID is the Identity of the table, NK1..2 are the natural key 
 * columns of the table. We wish to prevent overlapping date ranges
 * with the sets defined by the natural key columns NK1..2.
 *
 * You should
 *
 * 1. Add a table constraint EndDate >= StartDate
 * 2. Create a unique index on the natural key (as usual).
 * 3. Create single column indexes on the foreign key columns (as usual).
 * 4. Create 2 single column indexes on the StartDate and EndDate columns.
 * 5. Create a scalar function WC.ovr_TABLENAME (see below). This function
 *		has an identical form for each table.
 * 6. Use the function to create a named constraint on the table which uses
 *		the function to check for overlapping ranges.
 * 
 * This solution performs very well (tested on a table with up to 17.5 million rows,
 * response is essentially instantaneous).
 *
 * EndDate can be null or not-null and use a sentinel value ('2999-01-01').
 * 
 * If working on "days" rather than "times", dates should be pre-processed 
 * before they are entered in the table to ensure that they are either 
 * start-of-day or end-of-day. Two functions have been written to help with this.
 * This is very important to prevent BETWEENs from failing.
 *
 * Alternative solutions that perform cross-joins to the table do not scale.
 * Note: in order to change the function it is necessary to first drop the
 * constraint.
 *
 * Downside of this solution: you need to create a separate check function
 * for each table.
 *
 * WORKED EXAMPLE FOLLOWS.
 */

-- ============================================================================================================================================
-- ============================================================================================================================================
-- Create the table. (Assume you create indexes manually).

CREATE TABLE [WC].[PersonToSite]
	(
	[PersonToSiteId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[SiteId] [smallint] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	CONSTRAINT [PK_PersonToSite] PRIMARY KEY CLUSTERED 
		(
		[PersonToSiteId] ASC
		) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [WC].[PersonToSite]  WITH CHECK ADD  CONSTRAINT [CK_PersonToSite_RelativeDates] CHECK  (([EndDate]>[StartDate]))


-- ============================================================================================================================================
-- ============================================================================================================================================
-- Create the "check for overlapping dates" function.

create function WC.ovr_PersonToSite
	(
	@PersonToSiteId int,
	@PersonId int,			
	@SiteId smallint,				
	@StartDate datetime,
	@EndDate datetime
	)
returns bit
as

/*
<SUMMARY CREATED="06/03/2009" CREATOR="PDaniels">
	Function to prevent overlapping date ranges on this table.
	This function is used in a table constraint which must be 
	dropped before this function can be ALTERed.

	All the "overlaps" functions have the same form of
	parameters and body.
</SUMMARY>
		
<HISTORY>
</HISTORY>

<PERMISSIONS> </PERMISSIONS>
*/

begin
    declare @result bit

    if exists 
		(
        select 
			*
        from 
			WC.PersonToSite
        where 
			-- Not the same row...
			PersonToSiteId <> @PersonToSiteId
			-- ...but within the same set of records...
			and PersonId = @PersonId
			and SiteId = @SiteId
			-- ...does there exist an overlapping record?
			and @StartDate <= coalesce(EndDate, '2999-01-01')
			and StartDate <= coalesce(@EndDate, '2999-01-01')
        )
        set @result = 0
    else
        set @result = 1

    return @result
end
go

-- ============================================================================================================================================
-- ============================================================================================================================================
-- Add the constraint to the table. "with nocheck" is necessary if the table has millions of rows.
-- But beware that this may leave you with some invalid data in the table.
alter table WC.PersonToSite with check add constraint 
	ck_PersonToSite_Overlaps check (WC.ovr_PersonToSite(PersonToSiteId, PersonId, SiteId, StartDate, EndDate) = 1)

-- Drop the constraint. Must do this if you want to alter the function.
alter table WC.PersonToSite drop constraint ck_PersonToSite_Overlaps 



-- ============================================================================================================================================
-- ============================================================================================================================================
-- Create the start and end of day functions.

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE function [IIS].[f_StartOfDay]
(
	@Date datetime
)
returns datetime
as
/*
<SUMMARY CREATED="05/03/2009" CREATOR="PDaniels" 
	Given a date, returns the start of day (00:00:00)
</SUMMARY>
		
<HISTORY>
</HISTORY>

<PERMISSIONS> </PERMISSIONS>
*/

begin

declare @StartOfDay datetime

set @StartOfDay = cast(convert(char(10), @Date, 120) as datetime)

return @StartOfDay

end
go


SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE function [IIS].[f_EndOfDay]
(
	@Date datetime
)
returns datetime
as
/*
<SUMMARY CREATED="05/03/2009" CREATOR="PDaniels" 
	Given a date, returns the end of day (23:59:59.997).
	Uses format 120, which is ODBC canonical, so it should
	work whatever the user's locale.
</SUMMARY>
		
<HISTORY>
</HISTORY>

<PERMISSIONS> </PERMISSIONS>
*/

begin

declare @EndOfDay datetime

set @EndOfDay = cast(convert(char(10), @Date, 120) + ' 23:59:59.997' as datetime)

return @EndOfDay

end


-- ============================================================================================================================================
-- ============================================================================================================================================
-- Misc stuff (for reference info).

-- Find all overlapping dates in table (slow for large tables).
select 
	* 
from 
	WC.PersonToSite T1
	cross join WC.PersonToSite T2 
where
	T1.PersonToSiteId <> T2.PersonToSiteId											-- exclude rows joined to 
	and T1.PersonId = T2.PersonId and T1.SiteId = T2.SiteId				-- split into sets based on natural key 	
	and T1.StartDate between T2.StartDate and T2.EndDate				-- find overlapping dates within those sets


-- Allow only one setting of a thing in the sets defined by the natural keys.
select
	PersonId, SiteId
from
	WC.PersonToSite
where
	getdate() between StartDate and EndDate
	and IsPrimary = 1
group by
	PersonId, SiteId
having
	count(*) > 1



-- ======================================================================
-- Trigger solution, bad performance for large tables.

USE [WebCAT]
GO
/****** Object:  Trigger [WC].[trg_PersonToSite_Overlaps]    Script Date: 03/05/2009 14:53:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [WC].[trg_PersonToSite_Overlaps] on [WC].[PersonToSite] for Insert, Update as

/*
<SUMMARY CREATED="05/03/2009" CREATOR="PDaniels">
	Prevent overlapping date ranges on this table.
	A deletion can never cause on overlap, so we just
	need to worry about INSERT and UPDATE statements,
	both of which create an Inserted table.
</SUMMARY>
		
<HISTORY>
</HISTORY>

<PERMISSIONS> </PERMISSIONS>
*/
begin try

set nocount on


if exists
	(
	select 
		* 
	from 
		Inserted I
		cross join WC.PersonToSite T2 
	where
			I.PersonToSiteId <> T2.PersonToSiteId									-- exclude rows joined to self
			and I.PersonId = T2.PersonId and I.SiteId = T2.SiteId				-- split into sets based on natural key 	
			and I.StartDate <= coalesce(T2.EndDate, '2999-01-01')		-- find overlapping dates within those sets
			and T2.StartDate <= coalesce(I.EndDate, '2999-01-01')
	)
begin
	raiserror('Insert causes duplicate date range', 16, -1)
end


end try
begin catch
	if @@trancount > 0 rollback
	declare @cProcName varchar(255)
	select @cProcName=object_name(@@procid)
	exec WC.ap_Sys_RaiseError @cProcName=@cProcName
end catch



-- ======================================================================
-- ======================================================================
-- Data population for testing purposes.

set nocount on
truncate table wc.persontosite

declare @i int
set @i = 0

while @i < 10 * 1000 begin
	insert into wc.persontosite(personid, siteid, startdate, enddate)
	select
		p.personid, p.primarysiteid,
		dateadd(year, @i, '2000-01-01'), 
		dateadd(year, @i, '2000-06-01')
	from
		wc.person p		

	set @i = @i + 1
end

select count(*) from wc.persontosite with (nolock)
select min(startdate) from wc.persontosite with (nolock)


	-- Add some more stuff after the bulk population.
	insert into wc.persontosite(personid, siteid, startdate, enddate)
	select 
		p.personid, p.primarysiteid,
		dateadd(year, -10, '1974-01-01'), 
		dateadd(year, -10, '1974-06-01')
	from
		wc.person p		

	union 
	select 
		p.personid, p.primarysiteid,
		dateadd(year, -10, '1975-01-01'), 
		dateadd(year, -10, '1975-06-01')
	from
		wc.person p		

	union 
	select 
		p.personid, p.primarysiteid,
		dateadd(year, -10, '1976-01-01'), 
		dateadd(year, -10, '1976-06-01')
	from
		wc.person p		

	union 
	select 
		p.personid, p.primarysiteid,
		dateadd(year, -10, '1977-01-01'), 
		dateadd(year, -10, '1977-06-01')
	from
		wc.person p		


