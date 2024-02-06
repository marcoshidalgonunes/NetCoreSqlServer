# NetCoreSqlServer
Example of an Web API using Entity Framework as ORM for SQL Server database, including stored procedures calls.

## Database

The "WeatherForecast" is a [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16) database, suitable to demonstrate the usage of stored procedures with Entity Framework.

To create the database use the [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/relational-databases/databases/create-a-database?view=sql-server-ver16#SSMSProcedure) feature, which helps to choose a folder to create the corresponding files.

## Projects

This example contains a Visual Studio solution. Since one of the projects is a SQL Server Database Project, part of [SQL Server Data Tools (SSDT)](https://learn.microsoft.com/en-us/sql/ssdt/sql-server-data-tools?view=sql-server-ver16), it is needed Visual Studio 2022 or later with Data storage and processing workload.


### API

This project shows how to use Entity Framework to persist data in a SQL Server database.

Persistence in "RegionForecast" table is made using a Service. This service process additional validation and calls the stored procedures related with the table. One method of service calls a stored procedure passing a DataTable to send a list defined as User Defined Table parameter in SQL Server side.

Additionally, there is a middleware to centralize exception handling.

### Console App

This project shows how to bulk insert lines in a SQL Server table for maximum performance.

Additionally it shows how to read Json data from a file and usage of [System.CommandLine package](https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial) to pass arguments to console application. Below there is an example of usage (note that the application name is different from project name):

```wfload --regionId 1 -- jsonFile "C:\Forecasts\2024-01_January.json"```

> "forecasts.json" is an example of file containing weather forecasts in JSON formatted accordingly "WeatherForecast" database.

### Database

This project maintains the "WeatherForecast" database in the SQL Server LocalDB. It contains a Schema Compare to create the database objects (tables, user defined table and stored procedures), which should be run once to let the solution ready to run.
