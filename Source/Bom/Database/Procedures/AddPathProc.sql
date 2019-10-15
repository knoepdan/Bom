CREATE PROCEDURE [dbo].[AddPathProc]
	@nodeId AS INT NULL,
	@parentPath AS NVARCHAR(MAX) NULL,
	@setAsMainPath AS BIT NULL,
	@pathIdOutput INT OUTPUT 
AS
BEGIN

	IF (@nodeId IS NULL)
	BEGIN
		print 'passed param @@nodeId may not be NULL';
		THROW 50401, 'passed param @@nodeId may not be NULL', 1;
	END

	BEGIN TRANSACTION;
    SAVE TRANSACTION AddPathSavePoint;
	BEGIN TRY

		DECLARE @genPathId INT
		SET @genPathId = -1

		-- insert path
		DECLARE @path  NVARCHAR(MAX)
		SET @path = @parentPath + '/' + TRIM(STR(@nodeId)) + '/'
		IF @path NOT LIKE '/%'
			SET @path = '/' + @path;
		DECLARE @depth INT
		SET @depth = LEN(@path) - LEN(REPLACE(@path, '/', '')) - 1 -- example: /12/46/45/ -> depth: 3  (always starts/ends with /)

		INSERT INTO [dbo].[Path]
			   ([NodePath],
			   [SetParentPath] ,[SetDepth] ,[NodeId])
				VALUES (CAST(@path AS Hierarchyid),
				@parentPath,@depth, @nodeId )
		SELECT @genPathId =  (SELECT SCOPE_IDENTITY());

		-- insert as main  paht
		IF @setAsMainPath IS NULL AND @setAsMainPath = 1 
			UPDATE [dbo].[Node]
				SET [MainPathId] =  @genPathId
				WHERE NodeId = @nodeId;

		SET @pathIdOutput = @genPathId; -- needed when called from other sp 
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AddPathSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH

ENd
