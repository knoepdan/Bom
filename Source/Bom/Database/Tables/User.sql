CREATE TABLE [dbo].[User] (
    [Username]		        NVARCHAR (255)  NOT NULL,
	[PasswordHash]      NVARCHAR (MAX)  NULL,
    [Salt]      NVARCHAR (255)  NULL,
    [ActivationToken]   NVARCHAR (255)  NULL,
    [FaceBookId]        NVARCHAR (1000) NULL,
    [UserStatus]        TINYINT  NULL,

    -- TODO - information about lockout etc. adit staff etc.

    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Username] ASC),
 );