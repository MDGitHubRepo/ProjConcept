CREATE TABLE [dbo].[Users]
(
	[UserLoginId] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [LastName] NVARCHAR(30) NOT NULL, 
    [FirstName] NVARCHAR(30) NOT NULL, 
    [EmailAddress] NVARCHAR(254) NOT NULL, 
    [AuthorizationLevel] TINYINT NOT NULL DEFAULT 0
)
