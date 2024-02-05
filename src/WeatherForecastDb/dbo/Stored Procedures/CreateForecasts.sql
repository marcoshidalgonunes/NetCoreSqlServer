CREATE PROCEDURE CreateForecasts
(
	@RegionId int,
	@Forecasts Forecasts READONLY
) AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO RegionForecasts
	SELECT 
		@RegionId, 
		[Date], 
		TemperatureC,
		Summary 
	FROM @Forecasts
END