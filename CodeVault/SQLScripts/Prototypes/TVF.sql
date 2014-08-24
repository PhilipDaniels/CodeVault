SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function dbo.MyFunc
	(
	@Param1 int
	)
returns table
as
/*
  Always use TVF as they can be inlined into the containing query plan, giving
  vastly better performance than scalar functions.
*/
return
select
    whatever
from
    [wherever]
GO
