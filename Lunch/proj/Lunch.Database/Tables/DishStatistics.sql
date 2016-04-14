CREATE TABLE [dbo].[DishStatistics]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserId] INT NOT NULL, 
	[DishId] INT NOT NULL, 
	[SelectionCount] INT NULL, 
	[Rating] NVARCHAR(250) NULL, 
	CONSTRAINT [FK_DishStatistics_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_DishStatistics_Dish] FOREIGN KEY ([DishId]) REFERENCES [Dish]([Id]),
)
