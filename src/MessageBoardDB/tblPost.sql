CREATE TABLE tblPost
(
PostID int IDENTITY(1,1) PRIMARY KEY,
[UserID] INT NOT NULL ,
[PostText] NVARCHAR(300) NOT NULL,
PostImage IMAGE,
IsPublished BIT NOT NULL,
[CreationDate] DATETIME NOT NULL, 
    CONSTRAINT [FK_tblPost_tblUser] FOREIGN KEY ([UserID]) REFERENCES [tblUser]([UserID]),
);