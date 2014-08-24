declare curTank cursor local static for 
<<SELECT STATEMENT>>

open curTank
while 1 = 1 begin
	fetch next from curTank into <<VARIABLES>>
	if @@fetch_status = -1 break

end

close curTank
deallocate curTank

