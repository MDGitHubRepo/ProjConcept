CREATE TABLE [dbo].[UserNotes]
(
	[NoteId] INT NOT NULL IDENTITY , 
    [UserId] NVARCHAR(50) NOT NULL, 
    [Note] NVARCHAR(MAX) NOT NULL, 
    [NoteLastUpdate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [NoteTitle] NVARCHAR(50) NOT NULL, 
    PRIMARY KEY ([NoteId])
)
