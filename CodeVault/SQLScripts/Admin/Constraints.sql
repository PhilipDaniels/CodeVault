-- Turn constraints off.
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

-- Turn constraints on.
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'

