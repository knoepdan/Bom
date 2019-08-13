CREATE PROCEDURE [dbo].[AddNodeWithParentPathProc]
	@title AS NVARCHAR(255) NULL,
	@parentPath AS NVARCHAR(MAX) NULL
AS
BEGIN

	BEGIN TRANSACTION;
    SAVE TRANSACTION AddNodePathSavePoint;
	BEGIN TRY

		DECLARE @genId INT
		DECLARE @genPathId INT
		SET @genPathId = -1

		-- insert node
		INSERT INTO [dbo].[Node] ([Title] ,[MainPathId])
		VALUES (@title, NULL);
		SELECT @genId =   (SELECT SCOPE_IDENTITY());

		-- insert path
		DECLARE @path  NVARCHAR(MAX)
		SET @path = @parentPath + '/' + TRIM(STR(@genId)) + '/'
		IF @path NOT LIKE '/%'
			SET @path = '/' + @path;
		DECLARE @depth INT
		SET @depth = LEN(@path) - LEN(REPLACE(@path, '/', '')) - 1 -- example: /12/46/45/ -> depth: 3  (always starts/ends with /)

		INSERT INTO [dbo].[Path]
			   ([NodePath],
			   [SetParentPath] ,[SetDepth] ,[NodeId])
				VALUES (CAST(@path AS Hierarchyid),
				@parentPath,@depth, @genId )
		SELECT @genPathId =  (SELECT SCOPE_IDENTITY());

		-- insert as main path
		UPDATE [dbo].[Node]
			SET [MainPathId] =  @genPathId
			WHERE NodeId = @genId;

		SELECT * FROM  [dbo].[Path] WHERE PathId = @genPathId
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AddNodePathSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH

ENd
