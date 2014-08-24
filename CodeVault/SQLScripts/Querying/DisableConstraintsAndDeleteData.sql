-- Disable all constraints.
exec sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- Delete data in all tables. Truncate will not work unless you drop the constraints.
-- Will need to drop audit triggers first as well!!!
exec sp_MSForEachTable "DELETE FROM ?"

-- Enable all constraints. Won't work if you have bad data.
exec sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"


