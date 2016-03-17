CREATE TABLE [dbo].[Dish]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Name] NVARCHAR(MAX) NULL, 
	[Description] NVARCHAR(MAX) NULL, 
	[Type] NVARCHAR(MAX) NULL, 
	[DishPictureId] INT NULL, 
	CONSTRAINT [FK_Dish_DishPicture] FOREIGN KEY ([DishPictureId]) REFERENCES [DishPicture]([Id])
)
