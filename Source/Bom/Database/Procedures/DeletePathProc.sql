CREATE PROCEDURE dbo.[DeletePathProc]
	@pathId AS INT NULL,
	@alsoDeleteNode AS BIT = 0,
	@deleteSubTree AS BIT  = 0
AS
BEGIN

	-- simple params checking
	IF (@pathId IS NULL)
	BEGIN
		print 'passed param @pathId may not be NULL';
		THROW 50001, 'passed param @pathId may not be NULL', 1;
	END


	BEGIN TRANSACTION;
    SAVE TRANSACTION DeletePathSavePoint;
	BEGIN TRY

		DECLARE @nodeId INT
		SELECT  @nodeId = NodeId FROM dbo.[nPath] WHERE PathId = @pathId

		-- ## 2. Move all the children to parent
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

		IF @deleteSubTree = 0
		BEGIN

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
			UPDATE dbo.[nNode] SET MainPathId = NULL WHERE MainPathId = @pathId

			DELETE FROM [dbo].[nPath] WHERE PathId = @pathId

			IF @alsoDeleteNode = 1
				BEGIN
					DELETE FROM dbo.[nNode] WHERE NodeId = @nodeId
				END

		END
		ELSE
		BEGIN
			-- DELETE all descendants too
			UPDATE dbo.[nNode] SET MainPathId = NULL WHERE  MainPathId = @pathId 
								OR MainPathId IN (SELECT PathId FROM [dbo].[nPath] WHERE  NodePath.IsDescendantOf(@currentPath) = 1 ) 

			IF @alsoDeleteNode = 1
			BEGIN

				-- save nodes to delete in a temporary table
				CREATE TABLE #Temp 	(TempNodeId  INT);
				INSERT INTO #Temp  (TempNodeId)
					SELECT NodeId FROM dbo.[nNode] WHERE NodeId IN (SELECT NodeId FROM [dbo].[nPath] WHERE PathId = @pathId OR  NodePath.IsDescendantOf(@currentPath) = 1 ) 

				-- delete path
				DELETE FROM [dbo].[nPath] WHERE  NodePath.IsDescendantOf(@currentPath) = 1 OR  PathId = @pathId

				-- delete node
				DELETE FROM dbo.[nNode] WHERE NodeId IN (SELECT TempNodeId FROM  #Temp )

				DROP TABLE #Temp;
			END
			ELSE 
			BEGIN
				-- just delete path
				DELETE FROM [dbo].[nPath] WHERE  NodePath.IsDescendantOf(@currentPath) = 1 OR  PathId = @pathId
			END
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