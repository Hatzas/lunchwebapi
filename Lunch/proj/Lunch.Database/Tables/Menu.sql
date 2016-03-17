CREATE TABLE [dbo].[Menu]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[DishId] INT NULL, 
	[DishCategoryId] INT NULL, 
	[Date] DATETIME NULL, 
	[Serial] NVARCHAR(50) NULL, 
	CONSTRAINT [FK_Menu_Dish] FOREIGN KEY ([DishId]) REFERENCES [Dish]([Id]), 
	CONSTRAINT [FK_Menu_DishCategory] FOREIGN KEY ([DishCategoryId]) REFERENCES [DishCategory]([Id])
)
