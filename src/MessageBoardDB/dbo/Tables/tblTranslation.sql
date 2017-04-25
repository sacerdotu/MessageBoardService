CREATE TABLE [dbo].[tblTranslation] (
    [TranslationID] INT           IDENTITY (1, 1) NOT NULL,
    [FormName]      NVARCHAR (50) NULL,
    [ControlName]   NVARCHAR (50) NULL,
    [RowKey]        NVARCHAR (50) NULL,
    CONSTRAINT [PK_tblTranslation] PRIMARY KEY CLUSTERED ([TranslationID] ASC)
);



