CREATE TABLE [dbo].[iUserRole] (
    [RoleId]		    INT       NOT NULL,
	[UserId]            INT       NOT NULL,
  
    CONSTRAINT [PK_iUserRole] PRIMARY KEY CLUSTERED ([RoleId], [UserId] ASC),
    CONSTRAINT [FK_UserRole_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [iRole] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRole_UserId] FOREIGN KEY ([UserId]) REFERENCES [iUser] ([UserId]) ON DELETE CASCADE
 );