CREATE TABLE [dbo].[tblUser] (
    [UserID]              INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]           NVARCHAR (100) NOT NULL,
    [LastName]            NVARCHAR (100) NOT NULL,
    [Username]            NVARCHAR (255) NOT NULL,
    [PasswordHash]        NVARCHAR (255) NOT NULL,
    [PasswordSalt]        NVARCHAR (255) NOT NULL,
    [Country]             NVARCHAR (50)  NOT NULL,
    [City]                NVARCHAR (50)  NOT NULL,
    [Function]            NVARCHAR (50)  NOT NULL,
    [ProfileImage]        IMAGE          NULL,
    [IsAdministrator]     BIT            NOT NULL,
    [IsActive]            BIT            NOT NULL,
    [AccountCreationDate] DATETIME       NOT NULL,
    [LanguageID]          INT            NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_tblUser_tblLanguage] FOREIGN KEY ([LanguageID]) REFERENCES [dbo].[tblLanguage] ([LanguageID])
);



