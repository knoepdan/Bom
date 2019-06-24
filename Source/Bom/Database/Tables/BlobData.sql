CREATE TABLE [dbo].[BlobData] (
    [BlobDataId]		INT            IDENTITY (1, 1) NOT NULL,
    [Data]				VARBINARY(MAX) NOT NULL,
    CONSTRAINT [PK_BlobData] PRIMARY KEY CLUSTERED ([BlobDataId] ASC),
);

