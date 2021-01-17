CREATE TABLE [dbo].[UserRole] (
    [RoleId]		    INT       NOT NULL,
	[UserId]            INT       NOT NULL,
  
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([RoleId], [UserId] ASC),
    CONSTRAINT [FK_UserRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRole_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId]) ON DELETE CASCADE
 );