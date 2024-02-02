CREATE TABLE [dbo].[Regions] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Regions_Name]
    ON [dbo].[Regions]([Name] ASC);
GO
