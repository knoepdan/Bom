CREATE TABLE [dbo].[iRole] (
    [RoleId]		    INT            IDENTITY (1, 1) NOT NULL,
	[RoleName]          NVARCHAR (255)  NULL,
    [AppFeatures]      INT  NOT NULL -- IDEA: to avoid joins, features as bits (and in C# Enums with flags)
    CONSTRAINT [PK_iRole] PRIMARY KEY CLUSTERED ([RoleId] ASC) DEFAULT 0
 );