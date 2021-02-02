CREATE TABLE [dbo].[nNode] (
    [NodeId]     INT            IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (255) NOT NULL,
    [MainPathId] INT            NULL,
    CONSTRAINT [PK_nNode] PRIMARY KEY CLUSTERED ([NodeId] ASC),
	-- there should be a foreign key constraint but it is cumbersome to add it 
    --  CONSTRAINT [FK_Node_Path] FOREIGN KEY ([MainPathId]) REFERENCES [dbo].[Path] ([PathId] ON UPDATE CASCADE) -- should be here but 
);

