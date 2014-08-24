
declare @x1 datetime = cast(getdate() as date), @x2 datetime = getdate()
select @x1, @x2

-- Watch it! x1 and x2 both used to show some decent examples.
select cast(@x1 as date), cast(@x2 as time)			-- Components.
select convert(varchar(10), @x1, 120)				-- YYYY-MM-DD
select convert(varchar(19), @x2, 120)				-- YYYY-MM-DD HH:MM:SS
select right(convert(varchar(19), @x1, 120), 8)		-- HH:MM:SS

-- Add date and time together (from separate variables)
select convert(varchar(10), @x1, 120) + ' ' + right(convert(varchar(19), @x2, 120), 8)



