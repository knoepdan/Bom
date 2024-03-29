﻿CREATE TABLE [dbo].[nDbPicture] (
    [DbPictureId]		INT            IDENTITY (1, 1) NOT NULL,
    [Name]				NVARCHAR (255) NOT NULL,
    [PictureUid]		NVARCHAR (255) NOT NULL,
	[TimeStamp]			DATETIME2 NULL,
	[Longitude]			REAL NULL,
	[Latitude]			REAL NULL,
	[Place]				NVARCHAR (255)            NULL,
	[Width]				INT NOT NULL,
	[Height]			INT NOT NULL,
	[FileLenght]		BIGINT NOT NULL,
	[BlobDataId] INT NOT NULL,
    CONSTRAINT [PK_nDbPicture] PRIMARY KEY CLUSTERED ([DbPictureId] ASC),
	CONSTRAINT [FK_DbPicture_BlobDataId] FOREIGN KEY ([BlobDataId]) REFERENCES [dbo].[nBlobData] ([BlobDataId]) ON DELETE CASCADE,
	CONSTRAINT [Uc_DbPicture_PictureUid] UNIQUE([PictureUid]) 

);

