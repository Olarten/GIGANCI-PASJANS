@echo off
title Pasjans Launcher
cls

echo Launching Pasjans...
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo .NET SDK is not installed! Please install it first.
    pause
    exit /b 1
)

REM Restore dependencies
echo Restoring dependencies...
dotnet restore

REM Build the project
echo Building project...
dotnet build --configuration Release

REM Run the game
echo Starting the game...
echo.
cd Pasjans
dotnet run

REM If game exits, wait for user input
echo.
pause
