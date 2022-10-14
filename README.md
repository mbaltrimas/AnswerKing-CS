# Answer King
Dotnet implementation of the Answer King Api

## The following libraries/technologies were used:
* [.NET Core (.NET is a free, cross-platform, open source developer platform)](https://dot.net)
* [LiteDb (An open source MongoDB-like database with zero configuration)](https://www.litedb.org/)
* [Swashbuckle.AspNetCore (Swagger / OpenAPI - Automatically generates Api Documentation)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [FluentValidation.AspNetCore (A popular .NET library for building strongly-typed validation rules)](https://fluentvalidation.net/)

## Running the solution

Ensure you have the latest [.NET 6 SDK (v6.0.401)](https://www.microsoft.com/net/download/) installed.

Clone the project:

`$ git clone git@gitlab.com:AnswerAcademy/Answer.King.git`

CD into the newly cloned repository:

`$ cd Answer.King`

Now run the project:

`$ dotnet run --project src/Answer.King.Api/Answer.King.Api.csproj`

Now open your browser and navigate to `https://localhost:5001` and you should be greeted by the swagger interface describing the api


## Unit Testing

The project is accompanied by unit tests. The project uses `xUnit` for testing.

[Learn about xUnit](https://xunit.github.io/)