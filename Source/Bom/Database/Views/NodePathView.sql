CREATE VIEW NodePathView
	AS 

SELECT n.NodeId, n.Title, n.MainPathId, p.[PathId], p.[Level], p.[NodePathString]
			FROM [dbo].[Path] p
			 INNER JOIN [dbo].[Node] n ON p.NodeId = n.NodeId;
