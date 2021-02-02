CREATE VIEW nNodePathView
	AS 

SELECT n.NodeId, n.Title, n.MainPathId, p.[PathId], p.[Level], p.[NodePathString]
			FROM [dbo].[nPath] p
			 INNER JOIN [dbo].[nNode] n ON p.NodeId = n.NodeId;
