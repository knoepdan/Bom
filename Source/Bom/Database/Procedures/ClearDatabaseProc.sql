CREATE PROCEDURE dbo.[ClearDatabaseProc]
AS
BEGIN

	BEGIN TRANSACTION;
    SAVE TRANSACTION ClearDatabaseSavePoint;
	BEGIN TRY

		DECLARE @version NVARCHAR(255)
		SELECT  @version = [Value] FROM dbo.[cSetting] WHERE [Key] = 'Version' AND [Value] LIKE 'test%'


		-- ## 1. Set mainPath Node
			IF NOT @version IS NULL AND  NOT @version = ''
				BEGIN
					UPDATE dbo.[nNode] SET MainPathId = NULL;
					DELETE FROM dbo.[nPath];
					DELETE FROM dbo.[nNode];
				
					DELETE FROM dbo.nDbPicture;
					DELETE FROM dbo.nBlobData;
				END
			ELSE
				BEGIN
					print 'It seems that clearing the database was not possible! Probably not a testing database. Version: ' + @version;
					THROW 50911, 'Could not clear database because it seems this is not a testing database', 1;
				END
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION ClearDatabaseSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH
END