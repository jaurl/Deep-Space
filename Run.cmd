@echo off
cd /d "%~dp0"
dotnet run --no-restore
if errorlevel 1 pause
