CREATE PROCEDURE dbo.[DeletePathProc]
	@pathId INT NULL,
	@newMainPathId INT NULL,
	@alsoDeleteNode BIT  NULL
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


	DECLARE @nodeId INT
	SELECT  @nodeId = NodeId FROM dbo.[Path] WHERE PathId = @pathId


	-- ## 1. Set mainPath Node
	IF NOT @newMainPathId IS NULL 
	BEGIN
		IF NOT EXISTS (SELECT PathId FROM dbo.[Path] WHERE PathId = @newMainPathId AND NodeId = @nodeId)
		BEGIN
			print 'error 1. execution aborted';
			THROW 50101, 'passed replacement pathId does not exist or does not belong to node', 1;
		END

		UPDATE dbo.[Node]
			SET MainPathId = @newMainPathId
			WHERE NodeId = @nodeId AND MainPathId = @pathId
	END
	ELSE
	BEGIN
		DECLARE @otherPathId INT
		SET @otherPathId = 0
		SELECT @otherPathId = PathId FROM dbo.[Path] WHERE PathId <> pathId AND NodeId = @nodeId

		IF @otherPathId > 0 
			UPDATE dbo.[Node] SET MainPathId = @otherPathId WHERE NodeId = @nodeId AND MainPathId = @pathId
		ELSE
			IF @alsoDeleteNode = 0 
			BEGIN
				print 'error 2. execution aborted';
				THROW 50104, 'No replacement for main path id found and node is not to be deleted.. will not work', 1;
			END
			-- set to null
			UPDATE dbo.[Node] SET MainPathId = NULL WHERE NodeId = @nodeId AND MainPathId = @pathId

	END

	-- ## 2. Move all the children to parent
	-- TODO - check what children would become new root paths
	DECLARE @currentPath HIERARCHYID
	DECLARE @parentPath HIERARCHYID
	DECLARE @parentPathId INT
	DECLARE @currentLevel INT
	SELECT  @currentPath = NodePath, @currentLevel = [Level], @parentPath = NodePath.GetAncestor(1) FROM dbo.[Path] WHERE PathId = @pathId
	SELECT @parentPathId = PathId FROM dbo.[Path] WHERE NodePath = @parentPath

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
			EXEC dbo.MoveNodeProc @pathId = @childPathId, @newParentPathId = @parentPathId, @moveChildrenToo = 1
			FETCH NEXT FROM db_cursor INTO @childPathId 
		END 

	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	
	-- ## 3. Delete Path (and node)
	DELETE FROM [dbo].[Path] WHERE PathId = @pathId
	IF @alsoDeleteNode = 0 
	BEGIN
		DELETE FROM dbo.[Node] WHERE NodeId = @nodeId
	END
END