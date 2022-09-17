CREATE TABLE [dbo].[Reservations]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [BookId] INT NOT NULL
)
