CREATE TABLE [dbo].[iUser] (
    [UserId]		    INT            IDENTITY (1, 1) NOT NULL,
    [Username]		        NVARCHAR (255)  NOT NULL,
	[PasswordHash]      NVARCHAR (MAX)  NULL,
    [Salt]      NVARCHAR (255)  NULL,
    [ActivationToken]   NVARCHAR (255)  NULL,
    [FaceBookId]        NVARCHAR (1000) NULL,
    [UserStatus]        TINYINT  NOT NULL,

    [Email2]   NVARCHAR (255)  NULL,
    [Name]   NVARCHAR (255)  NULL,
  


    -- TODO - information about lockout etc. adit staff etc.

    CONSTRAINT [PK_iUser] PRIMARY KEY CLUSTERED ([UserId] ASC),


   
 );
 GO

 
GO


CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Username] ON [dbo].[iUser]  ([Username] ASC) WHERE ([Username] IS NOT NULL);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_FacebookId] ON [dbo].[iUser]  ([FaceBookId] ASC) WHERE ([FaceBookId] IS NOT NULL);
GO
