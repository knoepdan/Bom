CREATE PROCEDURE dbo.[ClearDatabaseProc]
AS
BEGIN


	DECLARE @version NVARCHAR(255)
	SELECT  @version = [Value] FROM dbo.[Setting] WHERE [Key] = 'Version' AND [Value] LIKE 'test%'


	-- ## 1. Set mainPath Node
		IF NOT @version IS NULL AND  NOT @version = ''
			BEGIN
				UPDATE dbo.[Node] SET MainPathId = NULL;
				DELETE FROM dbo.[Path];
				DELETE FROM dbo.[Node];
				
				DELETE FROM dbo.DbPicture;
				DELETE FROM dbo.BlobData;
			END
		ELSE
			BEGIN
				print 'It seems that clearing the database was not possible! Probably not a testing database. Version: ' + @version;
				THROW 50911, 'Could not clear database because it seems this is not a testing database', 1;
			END

END