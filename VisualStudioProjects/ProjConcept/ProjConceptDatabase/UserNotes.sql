CREATE TABLE [dbo].[UserNotes]
(
	[NoteId] INT NOT NULL , 
    [UserId] NVARCHAR(50) NOT NULL, 
    [Note] NVARCHAR(MAX) NOT NULL, 
    [NoteLastUpdate] DATETIME NOT NULL DEFAULT GETDATE(), 
    PRIMARY KEY ([NoteId])
)
