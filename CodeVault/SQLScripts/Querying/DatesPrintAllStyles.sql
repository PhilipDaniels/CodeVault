-- Print out all styles.
DECLARE
	@Style int = 0,
	@Now datetime = getdate(),
	@Message varchar(100);

WHILE @Style < 150 BEGIN
    BEGIN TRY
        SET @Message = convert(varchar, @Style) + ' : ' + convert(varchar, @Now, @Style)
        RAISERROR(@Message, 10, 1) WITH NOWAIT
    END TRY
    BEGIN CATCH
    END CATCH
    SET @Style = @Style + 1 
END
