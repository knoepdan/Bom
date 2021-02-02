CREATE PROCEDURE [dbo].[AddPathToExistingNodeProc]
	@nodeId AS INT NULL,
	@parentPath AS NVARCHAR(MAX) NULL,
	@setAsMainPath AS BIT NULL
AS
BEGIN

	BEGIN TRANSACTION;
    SAVE TRANSACTION AddPathToExistingSavePoint;
	BEGIN TRY

		-- add path
		DECLARE @pathIdOutput INT
		EXEC  [dbo].[AddPathProc] @nodeId, @parentPath, @setAsMainPath, @pathIdOutput OUTPUT

		SELECT * FROM  [dbo].[nPath] WHERE PathId = @pathIdOutput
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AddPathToExistingSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH

END
