CREATE TABLE [dbo].[ErrorLog]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ErrorMessage] VARCHAR(MAX) NOT NULL, 
    [ErrorSource] VARCHAR(MAX) NOT NULL, 
    [ErrorStackTrace] VARCHAR(MAX) NOT NULL, 
    [ErrorTimestamp] DATETIME NOT NULL, 
    [ErrorUserId] NCHAR(50) NOT NULL
)
