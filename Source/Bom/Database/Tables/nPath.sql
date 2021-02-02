CREATE TABLE [dbo].[nPath] (
    [PathId]		INT IDENTITY (1, 1) NOT NULL,
	
	[NodePath]			HierarchyId NOT NULL,
	[Level] as [NodePath].GetLevel(),   
	[NodePathString] AS (CONVERT([nvarchar](max),[NodePath])), -- also Hierarchy.ToString()
    [NodeId]		INT NOT NULL,
 
 	[SetParentPath]	NVARCHAR(MAX) NOT NULL, -- OBSOLOET list of NodeIds. Example: 1/34/44
	[SetDepth]			INT NOT NULL,   -- OBSOLET
    CONSTRAINT [PK_nPath] PRIMARY KEY CLUSTERED ([PathId] ASC),
	CONSTRAINT [FK_Path_Node] FOREIGN KEY ([NodeId]) REFERENCES [dbo].[nNode] ([NodeId]), -- ON DELETE CASCADE would work but dangerous as we cannot just remove a node from a tree.
	CONSTRAINT UX_Path_NodePath UNIQUE(NodePath)  
);

