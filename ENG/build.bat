@echo off
chcp 65001
dotnet build --configuration Release Mist_GUI.sln
@echo on
pause
