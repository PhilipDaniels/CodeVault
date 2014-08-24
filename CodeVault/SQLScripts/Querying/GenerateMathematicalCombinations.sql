create table #Dlv(Id int identity(1,1), TankId int not null, nLocalOrderNo int, Volume int not null)
insert into #Dlv(TankId, nLocalOrderNo, Volume) values
	(1, 1000, 50),
	(1, 1001, 100),
	(1, 1002, 300),
	(1, 1003, 20),
	(2, 1004, 70),
	(3, 1005, 300),
	(3, 1006, 400),
	(3, 1007, 500);


select
	*
from
	(
	select
		D1.TankId,
		1 as CountOfDlv,
		coalesce(D1.Volume, 0) as SumOfDlv,
		cast(D1.nLocalOrderNo as varchar) as DeliveryNumbers
	from
		#Dlv D1

	union

	select
		D1.TankId,
		2 as CountOfDlv,
		coalesce(D1.Volume, 0) + coalesce(D2.Volume, 0) as SumOfDlv,
		cast(D1.nLocalOrderNo as varchar) + ',' + cast(D2.nLocalOrderNo as varchar) as DeliveryNumbers
	from
		#Dlv D1
		left join #Dlv D2 on D1.TankId = D2.TankId and D2.Id < D1.Id

	union 

	select
		D1.TankId,
		3 as CountOfDlv,
		coalesce(D1.Volume, 0) + coalesce(D2.Volume, 0) + coalesce(D3.Volume, 0) as SumOfDlv,
		cast(D1.nLocalOrderNo as varchar) + ',' + cast(D2.nLocalOrderNo as varchar) + ',' + cast(D3.nLocalOrderNo as varchar) as DeliveryNumbers
	from
		#Dlv D1
		left join #Dlv D2 on D1.TankId = D2.TankId and D2.Id < D1.Id
		left join #Dlv D3 on D1.TankId = D3.TankId and D3.Id < D2.Id

	union 

	select
		D1.TankId,
		4 as CountOfDlv,
		coalesce(D1.Volume, 0) + coalesce(D2.Volume, 0) + coalesce(D3.Volume, 0) + coalesce(D4.Volume, 0) as SumOfDlv,
		cast(D1.nLocalOrderNo as varchar) + ',' + cast(D2.nLocalOrderNo as varchar) + ',' + cast(D3.nLocalOrderNo as varchar) + ',' + cast(D4.nLocalOrderNo as varchar) as DeliveryNumbers
	from
		#Dlv D1
		left join #Dlv D2 on D1.TankId = D2.TankId and D2.Id < D1.Id
		left join #Dlv D3 on D1.TankId = D3.TankId and D3.Id < D2.Id
		left join #Dlv D4 on D1.TankId = D4.TankId and D4.Id < D3.Id
	) X
where
	X.DeliveryNumbers is not null
order by
	X.TankId, X.SumOfDlv;




drop table #Dlv;

