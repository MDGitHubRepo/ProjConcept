CREATE TABLE [dbo].[ErrorLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ErrorMessage] VARCHAR(MAX) NOT NULL, 
    [ErrorSource] VARCHAR(MAX) NOT NULL, 
    [ErrorStackTrace] VARCHAR(MAX) NOT NULL, 
    [ErrorTimestamp] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ErrorUserId] NCHAR(50) NOT NULL
)
