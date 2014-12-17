-- In Server A, 
-- Launch Query Analyzer and Login as 'SA'.
-- Select 'Master' as your database
exec sp_AddLinkedServer @server='Server B'
-- [Server B] should now be added into Server A.  Type exec sp_linkedserver to display the info.

-- To create a SQL login:
exec sp_AddLinkedSrvLogin @rmtsrvname='Server B', @LocalLogin='sa', @rmtuser='sa', @rmtpassword='SA_PASSWORD'
-- To create a Windows login:
exec sp_AddLinkedSrvLogin @rmtsrvname='Server B', @LocalLogin='domain\account', @rmtuser='domain\account'


-- To test:
SELECT * FROM Server_B.Master.dbo.sysobjects 
