CREATE TABLE [dbo].[tblLanguage] (
    [LanguageID] INT           IDENTITY (1, 1) NOT NULL,
    [Culture]    NVARCHAR (50) NULL,
    [Name]       NVARCHAR (50) NULL,
    CONSTRAINT [PK_tblLanguage] PRIMARY KEY CLUSTERED ([LanguageID] ASC)
);



