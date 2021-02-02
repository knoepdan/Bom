CREATE TABLE [dbo].[nBlobData] (
    [BlobDataId]		INT            IDENTITY (1, 1) NOT NULL,
    [Data]				VARBINARY(MAX) NOT NULL,
    CONSTRAINT [PK_nBlobData] PRIMARY KEY CLUSTERED ([BlobDataId] ASC),
);

