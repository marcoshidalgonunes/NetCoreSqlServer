CREATE TABLE [dbo].[RegionForecasts] (
    [RegionId]    INT            NOT NULL,
    [Date]        DATE           NOT NULL,
    [TemperatureC] INT            NOT NULL,
    [Summary]     NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_RegionForecasts] PRIMARY KEY NONCLUSTERED ([RegionId] ASC, [Date] ASC),
    CONSTRAINT [FK_Forecasts_Regions] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Regions] ([Id])
);

