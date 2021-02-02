CREATE PROCEDURE [dbo].[AddNodeWithPathProc]
	@title AS NVARCHAR(255) NULL,
	@parentPath AS NVARCHAR(MAX) NULL,
	@setAsMainPath AS INT = 1
AS
BEGIN

	BEGIN TRANSACTION;
    SAVE TRANSACTION AddNodePathSavePoint;
	BEGIN TRY
		
		-- insert node
		DECLARE @genId INT
		INSERT INTO [dbo].[nNode] ([Title] ,[MainPathId])
		VALUES (@title, NULL);
		SELECT @genId =   (SELECT SCOPE_IDENTITY());

		-- add path
		DECLARE @pathIdOutput INT
		EXEC  [dbo].[AddPathProc] @genId, @parentPath, @setAsMainPath, @pathIdOutput OUTPUT

		SELECT * FROM  [dbo].[nPath] WHERE PathId = @pathIdOutput
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AddNodePathSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH

END
