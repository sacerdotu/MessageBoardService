CREATE TABLE [dbo].[tblTranslation] (
    [TranslationID]  INT            IDENTITY (1, 1) NOT NULL,
    [TranslationKey] NVARCHAR (100) NULL,
    [LanguageID]     INT            NOT NULL,
    [Translation]    NCHAR (150)    NULL,
    [DateAdded]      DATETIME       NULL,
    [DateModified]   DATETIME       NULL,
    CONSTRAINT [PK_tblTranslation] PRIMARY KEY CLUSTERED ([TranslationID] ASC),
    CONSTRAINT [FK_tblLanguage_tblTranslation] FOREIGN KEY ([LanguageID]) REFERENCES [dbo].[tblLanguage] ([LanguageID]),
    CONSTRAINT [UC_LangID_TranslationKey] UNIQUE NONCLUSTERED ([LanguageID] ASC, [TranslationKey] ASC)
);









