-- http://geekswithblogs.net/bullpit/archive/2010/04/21/user-already-exists-in-the-current-database---sql-server.aspx

-- How to relink users in databases to logins without having to delete the user.
-- Useful when you try to do it via the SSMS login box.

-- First, make sure that this is the problem. This will lists the orphaned users:
EXEC sp_change_users_login 'Report'

-- If you already have a login id and password for this user, fix it by doing:
EXEC sp_change_users_login 'Auto_Fix', 'user'

-- If you want to create a new login id and password for this user, fix it by doing:
EXEC sp_change_users_login 'Auto_Fix', 'user', 'login', 'password'
