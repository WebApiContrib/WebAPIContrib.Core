@echo off

dotnet restore build.proj
dotnet fake build %*