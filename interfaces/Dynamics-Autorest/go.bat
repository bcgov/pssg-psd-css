@echo off

echo Updating meta data

dotnet run -p ..\OData.OpenAPI\odata2openapi\odata2openapi.csproj csu

echo Updating client

autorest --verbose Readme.md
