CREATE TABLE [dbo].[tblTranslation] (
    [Language]    NVARCHAR (50)  NOT NULL,
    [FormName]    NVARCHAR (50)  NOT NULL,
    [ControlName] NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (100) NULL,
    CONSTRAINT [PK_tblTranslation] PRIMARY KEY CLUSTERED ([Language] ASC, [FormName] ASC, [ControlName] ASC)
);





