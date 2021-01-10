CREATE TABLE [dbo].[UserRole] (
    [RoleId]		    INT       NOT NULL,
	[Username]          NVARCHAR (255)  NOT NULL,
  
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([RoleId], [Username] ASC),
    CONSTRAINT [FK_UserRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRole_Username] FOREIGN KEY ([Username]) REFERENCES [User] ([Username]) ON DELETE CASCADE
 );