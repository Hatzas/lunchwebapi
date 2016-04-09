CREATE TABLE [dbo].[UserMenu]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[UserId] INT NULL, 
	[MenuId] INT NULL, 
	[Date] DATE NULL, 
	CONSTRAINT [FK_UserMenu_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_UserMenu_Menu] FOREIGN KEY ([MenuId]) REFERENCES [Menu]([Id])
)
