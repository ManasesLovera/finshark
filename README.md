# API documentation

[youtube tutorial by Teddy Smith](https://www.youtube.com/watch?v=jMFaAc3sa04&list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc&index=2&ab_channel=TeddySmith)

## Instalations

1. Code environment: [Visual Studio](https://visualstudio.microsoft.com/es/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download)
2. Sql Server: [Sql Server Management Studio](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

If you are working on Visual Studio Code you may want to install the following extensions:
```
- C#
- C# Dev Kit
- .NET Extension Pack
- .NET Install Tool
- Nuget Gallery
- Prettier
```
## Dependencies
```
- Microsoft.AspNetCore.OpenApi v8.0.4
- Microsoft.EntityFrameworkCore.SqlServer v8.0.7
- Microsoft.EntityFrameworkCore.Tools v8.0.7
- Microsoft.EntityFrameworkCore.Design v8.0.7
- AutoMapper v13.0.1
- Newtonsoft.Json
- Microsoft.AspNetCore.Mvc.NewtonsoftJson
```
## First Steps
Create a database named `finshark` or your preferred name and update the `appsettings.json` file with your server name and the database name in the `DefaultConnection` string.

### Questions I made myself doing this project

1. Why Entity Framwork doesn't provide RemoveAsync method? <br>
    - Found useful content on [stackoverflow](https://stackoverflow.com/questions/42422656/entity-framework-doesnt-provide-deleteasync-or-addasync-why)