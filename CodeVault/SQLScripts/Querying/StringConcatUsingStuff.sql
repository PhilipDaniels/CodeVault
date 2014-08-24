create table #People(Id int, Name varchar(255))
insert into #People(Id, Name) values (1, 'Phil'), (2, 'Marvin'), (3,'Taylor')

create table #Sport(PeopleId int, Sport varchar(20))
insert into #Sport(PeopleId, Sport) values (1, 'Cricket'), (1, 'Cycling'), (2, 'Football'), (3, 'Golf'), (3, 'Rugby'), (3, 'Snooker')

-- You can do string concatenation like this, without using a variable and coalesce().
select
	P.Id, P.Name,
	stuff(
		(
		-- If you give this column a name in here you get a different result set
		-- which is not what you want.
		select ', ' + S.Sport
		from #Sport S
		where S.PeopleId = P.Id
		order by S.Sport desc
		for xml path('')
		), 1, 1, '') as Sports
from 
	#People P


drop table #People
drop table #Sport
