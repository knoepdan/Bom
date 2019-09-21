CREATE PROCEDURE dbo.[MoveNodeProc]
	@pathId AS INT NULL,
	@newParentPathId AS INT NULL,
	@moveChildrenToo AS BIT NULL
AS
BEGIN
	
	-- simple params checking
	IF (@pathId IS NULL)
	BEGIN
		print 'passed param @pathId may not be NULL';
		THROW 50201, 'passed param @pathId may not be NULL', 1;
	END
	IF (@newParentPathId IS NULL)
	BEGIN
		print 'passed param @@newParentPathId may not be NULL (value 0 is allowed to create a new root node!)';
		THROW 50201, 'passed param @@newParentPathId may not be NULL', 1;
	END
	IF (@moveChildrenToo IS NULL)
	BEGIN
		print 'passed param @@moveChildrenToo may not be NULL';
		THROW 50201, 'passed param @@moveChildrenToo may not be NULL', 1;
	END
	-- end params checking

	-- Example: 
	-- pilosa 12      ( /1/2/11/12/  )
	-- old parent 11  ( /1/2/11/   ) 
	-- reptiles: 4    ( /1/4/ )

	-- new path:  /1/4/12/  
	--SET @pathId = 12
	--SET @newParentPathId = 4
	----

	BEGIN TRANSACTION;
    SAVE TRANSACTION MovePathSavePoint;
	BEGIN TRY

		DECLARE @oldParentPathId INT
		DECLARE @oldParentPath HIERARCHYID
		DECLARE @newParentPath HIERARCHYID
		DECLARE @moveNodePath HIERARCHYID
		SELECT  @oldParentPath = NodePath.GetAncestor(1), @moveNodePath = NodePath FROM dbo.[Path] WHERE PathId = @pathId
		SELECT @oldParentPathId = pp.PathId FROM dbo.[Path] pp  WHERE pp.NodePath = @oldParentPath
		SELECT @newParentPath = pp.NodePath FROM dbo.[Path] pp  WHERE pp.PathId = @newParentPathId

		IF @moveChildrenToo = 0
		BEGIN
			-- children of node/path to move are moved to the current parent tree

			DECLARE @currentPath HIERARCHYID
			DECLARE @currentLevel INT
			SELECT  @currentPath = NodePath, @currentLevel = [Level] FROM dbo.[Path] WHERE PathId = @pathId

			-- loop over all child nodes and call sp recursivly
			DECLARE @childPathId INT
			DECLARE db_cursor CURSOR FOR 
			SELECT PathId FROM dbo.[Path]
				WHERE NodePath.IsDescendantOf(@currentPath) = 1
				AND [Level] =  (@currentLevel + 1)

			OPEN db_cursor  
			FETCH NEXT FROM db_cursor INTO @childPathId  

			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				EXEC dbo.MoveNodeProc @pathId = @childPathId, @newParentPathId = @oldParentPathId, @moveChildrenToo = 1
				FETCH NEXT FROM db_cursor INTO @childPathId 
			END 

			CLOSE db_cursor  
			DEALLOCATE db_cursor 

			-- move node that is childless now (we need to requery the paths because when we move a node down the paths of the new parents also change)
			SELECT  @oldParentPath = NodePath.GetAncestor(1), @moveNodePath = NodePath FROM dbo.[Path] WHERE PathId = @pathId
			SELECT @oldParentPathId = pp.PathId FROM dbo.[Path] pp  WHERE pp.NodePath = @oldParentPath
			SELECT @newParentPath = pp.NodePath FROM dbo.[Path] pp  WHERE pp.PathId = @newParentPathId
			print @moveNodePath.GetReparentedValue(@oldParentPath, @newParentPath).ToString()

			IF @newParentPathId > 0  
				UPDATE dbo.[Path] 
					SET NodePath = @moveNodePath.GetReparentedValue(@oldParentPath, @newParentPath).ToString()
					WHERE PathId = @pathId
			ELSE
				BEGIN
				print 'path ' + CAST(@pathId AS NVARCHAR(MAX)) + ' is set as a new root';
				UPDATE dbo.[Path] 
					SET NodePath = '/' + CAST(NodeId AS NVARCHAR(MAX)) + '/'
					WHERE PathId = @pathId
				END
		END
		ELSE 
		BEGIN

			-- prevent moving a node down as this would create endless loops
			IF @newParentPath.IsDescendantOf(@moveNodePath) = 1
			BEGIN
				print 'forbidden to move node down its own tree as this would cause an endless tree';
				THROW 50221, 'forbidden to move node down its own tree as this would cause an endless tree', 1;
			END

			-- move node (including children... in case the didnt need to be moved, they would have hinged to the parent node already)
			IF @newParentPathId <= 0  
			BEGIN
				print 'path ' + CAST(@pathId AS NVARCHAR(MAX)) + ' is set as a new root (including children)';
				SELECT @newParentPath = '/' FROM dbo.[Path] WHERE PathId = @pathId
			END

			UPDATE dbo.[Path] 
			SET NodePath = t.NewNodePath
					FROM (
			
						SELECT PathId, NodePath.GetReparentedValue(@oldParentPath, @newParentPath).ToString() AS NewNodePath
						FROM dbo.[Path] p
						WHERE     p.PathId = @pathId OR p.NodePath.IsDescendantOf(@moveNodePath) = 1
						) t
						WHERE t.PathId = dbo.[Path].PathId

			-- no need to return changed node (besides.. this seemed not always to be correct)-> SELECT * FROM  [dbo].[Path] WHERE PathId = @pathId

		END
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION MovePathSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH
END
