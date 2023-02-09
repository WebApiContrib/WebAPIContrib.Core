@echo off

dotnet tool restore
dotnet fake --version
dotnet fake build %*