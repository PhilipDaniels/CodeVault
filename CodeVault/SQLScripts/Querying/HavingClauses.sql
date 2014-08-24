-- Create a test table.
drop table #foo; 
create table #foo(a int, b int)

-- all values in b are distinct
delete #foo;
insert into #foo(a, b)
values  (1, 0), (1, 1), (1, 2),
		(2, 0), (2, 0);

select a from #foo group by a
having count(distinct b) = count(b)


-- no nulls in b
delete #foo;
insert into #foo(a, b)
values	(1, 0), (1, 1), (1, 2),
	(2, 0), (2, null);

select a from #foo group by a
having count(*) = count(b)


-- b is either all positive or all negative
delete #foo;
insert into #foo(a, b)
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, -2), (3, -3),
		(4, 4), (4, -2), (4, -3),
		(5, 0), (5, 0), (5, 0)

select a from #foo group by a
having min(b) * max(b) > 0


-- b is all positive, all negative, or all zero
delete #foo;
insert into #foo(a, b)
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, -2), (3, -3),
		(4, 4), (4, -2), (4, -3),
		(5, 0), (5, 0), (5, 0)

select a from #foo group by a
having sign(min(b)) = sign(max(b))


-- min is negative, max isn't.
delete #foo;
insert into #foo(a, b) 
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, -2), (3, -3),
		(4, 4), (4, -2), (4, 4),
		(5, 0), (5, 0), (5, 0),
		(6, -10)

select a from #foo group by a
having min(b) * max(b) < 0


-- b has at least one zero
delete #foo;
insert into #foo(a, b) 
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, -2), (3, -3),
		(4, 4), (4, -2), (4, 4),
		(5, 0), (5, 0), (5, 0),
		(6, -10)

select a from #foo group by a
having min(abs(b)) = 0


-- either min or max or both is 0
delete #foo;
insert into #foo(a, b) 
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, 0), (3, -3),
		(4, -4), (4, 0), (4, 4),
		(5, 0), (5, 0), (5, 0)

select a from #foo group by a
having min(b) * max(b) = 0


-- b has more than 1 value
-- (possibly faster than count(b) > 1 ??)
delete #foo;
insert into #foo(a, b) 
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, 0), (3, -3),
		(4, -4), (4, 0), (4, 4),
		(5, 0), (5, 0), (5, 0),
		(6, -10),
		(7, -10), (7, -10)

select a from #foo group by a
having min(b) < max(b)



-- b has only 1 distinct value, or nulls.
delete #foo;
insert into #foo(a, b) 
values	(1, 0), (1, 1), (1, 2),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, 0), (3, -3),
		(4, -4), (4, 0), (4, 4),
		(5, 0), (5, 0), (5, 0),
		(6, -10),
		(7, -10), (7, -10),
		(9, 99), (9, null)

select a from #foo group by a
having min(b) = max(b)


-- b deviates above and below const by the same amount
-- (eliminate the const for deviation around zero)
delete #foo;
insert into #foo(a, b) 
values	(1, 2), (1, 3), (1, 8),
		(2, 1), (2, 2), (2, 3),
		(3, -1), (3, 0), (3, -3),
		(4, -4), (4, 0), (4, 14),
		(5, 0), (5, 0), (5, 0),
		(6, -10),
		(7, -10), (7, -10),
		(8, null), (8, null),		-- n.b.!
		(9, 99), (9, null)

select a from #foo group by a
having min(b - 5) = -max(b - 5)	-- const = 5


-- The sequential numbers in b have no gaps
delete #foo;
insert into #foo(a, b) 
values	(1, 2), (1, 3), (1, 4),
		(2, -1), (2, 0), (2, 1),
		(3, 1), (3, 2), (3, 3),
		(4, -4), (4, 0), (4, 14),
		(5, 0), (5, 0), (5, 0),
		(6, -10),
		(7, -10), (7, -10),
		(8, null), (8, null),
		(9, 99), (9, null)

select a from #foo group by a
having (max(b) - min(b) + 1) = count(b)


