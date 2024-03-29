﻿CREATE PROCEDURE dbo.[DeletePathWithMainPathLogicOldProc]
	@pathId AS INT NULL,
	@alsoDeleteNode AS BIT  NULL,
	@newMainPathId AS INT NULL = NULL
AS
BEGIN

	-- simple params checking
	IF (@pathId IS NULL)
	BEGIN
		print 'passed param @pathId may not be NULL';
		THROW 50001, 'passed param @pathId may not be NULL', 1;
	END
	IF (@alsoDeleteNode IS NULL)
	BEGIN
		print 'passed param @@alsoDeleteNode may not be NULL';
		THROW 50002, 'passed param @@alsoDeleteNode may not be NULL', 1;
	END
	-- end params checking

	BEGIN TRANSACTION;
    SAVE TRANSACTION DeletePathSavePoint;
	BEGIN TRY

		DECLARE @nodeId INT
		SELECT  @nodeId = NodeId FROM dbo.[nPath] WHERE PathId = @pathId

		-- ## 1. Set mainPath Node
		IF NOT @newMainPathId IS NULL 
		BEGIN
			IF NOT EXISTS (SELECT PathId FROM dbo.[nPath] WHERE PathId = @newMainPathId AND NodeId = @nodeId)
			BEGIN
				print 'error 1. execution aborted';
				THROW 50101, 'passed replacement pathId does not exist or does not belong to node', 1;
			END

			UPDATE dbo.[nNode]
				SET MainPathId = @newMainPathId
				WHERE NodeId = @nodeId AND MainPathId = @pathId
		END
		ELSE
		BEGIN
			DECLARE @otherPathId INT
			SET @otherPathId = 0
			SELECT @otherPathId = PathId FROM dbo.[nPath] WHERE PathId <> pathId AND NodeId = @nodeId

			IF @otherPathId > 0 
				UPDATE dbo.[nNode] SET MainPathId = @otherPathId WHERE NodeId = @nodeId AND MainPathId = @pathId
			ELSE
				IF @alsoDeleteNode = 0 
				BEGIN
					print 'error 2. execution aborted';
					THROW 50104, 'No replacement for main path id found and node is not to be deleted.. will not work', 1;
				END
				-- set to null
				UPDATE dbo.[nNode] SET MainPathId = NULL WHERE NodeId = @nodeId AND MainPathId = @pathId

		END

		-- ## 2. Move all the children to parent
		-- TODO - check what children would become new root paths
		DECLARE @currentPath HIERARCHYID
		DECLARE @parentPath HIERARCHYID
		DECLARE @parentPathId INT
		DECLARE @currentLevel INT
		SELECT  @currentPath = NodePath, @currentLevel = [Level], @parentPath = NodePath.GetAncestor(1) FROM dbo.[nPath] WHERE PathId = @pathId
		SELECT @parentPathId = PathId FROM dbo.[nPath] WHERE NodePath = @parentPath

		-- special rule: root paths cannot be deleted when they have children
		IF @currentLevel <= 1 AND EXISTS(SELECT PathId FROM dbo.[nPath] WHERE NodePath.IsDescendantOf(@currentPath) = 1 AND [Level] = (@currentLevel + 1))
		BEGIN
				print 'error execution aborted because root node is to be deleted and has children';
				THROW 50151, 'root was about to be deleted but had children. First delete children before deleting before root node', 1;
		END

		-- loop over all child nodes and call sp recursivly
		DECLARE @childPathId INT
		DECLARE db_cursor CURSOR FOR 
		SELECT PathId FROM dbo.[nPath]
			WHERE NodePath.IsDescendantOf(@currentPath) = 1
			AND [Level] =  (@currentLevel + 1)

		OPEN db_cursor  
			FETCH NEXT FROM db_cursor INTO @childPathId  

			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				EXEC dbo.MoveNodeProc @pathId = @childPathId, @newParentPathId = @parentPathId, @moveChildrenToo = 1
				FETCH NEXT FROM db_cursor INTO @childPathId 
			END 

		CLOSE db_cursor  
		DEALLOCATE db_cursor 
	
		-- ## 3. Delete Path (and node)
		DELETE FROM [dbo].[nPath] WHERE PathId = @pathId
		IF @alsoDeleteNode = 0 
		BEGIN
			DELETE FROM dbo.[nNode] WHERE NodeId = @nodeId
		END

		COMMIT TRANSACTION 
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION DeletePathSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH
END