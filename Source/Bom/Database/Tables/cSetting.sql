CREATE TABLE [dbo].[cSetting] (
    [SettingId]		INT            IDENTITY (1, 1) NOT NULL,
    [Key]				NVARCHAR(50) NOT NULL,
	[Value]				NVARCHAR(255) NOT NULL,
    CONSTRAINT [PK_cSetting] PRIMARY KEY CLUSTERED ([SettingId] ASC),
);

