﻿CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] VARCHAR(MAX) NOT NULL, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [LastName] VARCHAR(MAX) NOT NULL
)
