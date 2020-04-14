@echo off

dotnet build .\src\eagle.sln /nologo
dotnet test .\src\Eagle.Tests\Eagle.Tests.csproj