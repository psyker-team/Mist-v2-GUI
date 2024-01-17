@echo off
chcp 65001
dotnet build --configuration Release Mist_GUI_Booster.sln
@echo on
pause
