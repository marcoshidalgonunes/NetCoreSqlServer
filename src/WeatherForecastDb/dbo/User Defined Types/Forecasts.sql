CREATE TYPE [dbo].[Forecasts] AS TABLE (
    [Date]        DATE           NOT NULL,
    [TemperatureC] INT            NOT NULL,
    [Summary]     NVARCHAR (255) NOT NULL);

