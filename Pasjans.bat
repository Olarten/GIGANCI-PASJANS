@echo off
title Pasjans Launcher
cls

echo Launching Pasjans...
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo .NET SDK is not installed! Attempting to download and install it...
    powershell -Command "Invoke-WebRequest -Uri https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.100-windows-x64-installer -OutFile dotnet-installer.exe"
    
    if exist dotnet-installer.exe (
        echo Running .NET SDK installer...
        start /wait dotnet-installer.exe
        
        REM After installation, clean up
        del dotnet-installer.exe
    ) else (
        echo Failed to download .NET SDK installer. Please install it manually from https://dotnet.microsoft.com/download
        pause
        exit /b 1
    )
)

REM Verify again if dotnet is now available
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo .NET SDK installation failed or was not completed.
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

REM Navigate to project folder (adjust if needed)
if exist Pasjans (
    cd Pasjans
    dotnet run
) else (
    echo Error: 'Pasjans' folder not found!
)

echo.
pause
