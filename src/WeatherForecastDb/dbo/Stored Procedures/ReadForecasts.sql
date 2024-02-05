CREATE PROCEDURE ReadForecasts
(
	@RegionId int
) AS
BEGIN
SELECT 
	[Date],
	TemperatureC,
	Summary
  FROM RegionForecasts
  WHERE @RegionId = @RegionId AND [Date] > GETDATE()
END