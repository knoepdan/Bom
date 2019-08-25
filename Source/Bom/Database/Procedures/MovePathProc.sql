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
		print 'passed param @@newParentPathId may not be NULL';
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
		SELECT  @oldParentPath = NodePath.GetAncestor(1) FROM dbo.[Path] WHERE PathId = @pathId
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
		END

		-- move node (including children... in case the didnt need to be moved, they would have hinged to the parent node already)
		UPDATE dbo.[Path] 
		SET NodePath = t.NewNodePath
			FROM (
			/*
				-- this code would also move siblings
				SELECT PathId, NodePath.GetReparentedValue(@oldParentPath, @newParentPath).ToString() AS NewNodePath
				FROM dbo.[Path] p
				WHERE NodePath.IsDescendantOf(@oldParentPath) = 1
					AND p.PathId <> @oldParentPathId) t
					*/
				SELECT PathId, NodePath.GetReparentedValue(@oldParentPath, @newParentPath).ToString() AS NewNodePath
					FROM dbo.[Path] p
					WHERE p.PathId = @pathId ) t
			WHERE t.PathId = dbo.[Path].PathId


		-- return new node (not the child nodes)
		SELECT * FROM  [dbo].[Path] WHERE PathId = @pathId
		COMMIT TRANSACTION
	END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION MovePathSavePoint; -- rollback to MySavePoint

		-- rethrow
		THROW 
    END CATCH
END
