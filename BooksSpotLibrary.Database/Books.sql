CREATE TABLE [dbo].[Books]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] VARCHAR(MAX) NOT NULL, 
    [Author] VARCHAR(MAX) NOT NULL, 
    [Publisher] VARCHAR(MAX) NOT NULL, 
    [PublishDate] DATE NOT NULL, 
    [Genre] VARCHAR(MAX) NOT NULL, 
    [ISBN] BIGINT NOT NULL, 
    [BookStatus] VARCHAR(MAX) NOT NULL
)
