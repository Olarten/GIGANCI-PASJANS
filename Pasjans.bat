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

        REM Check again for dotnet
        where dotnet >nul 2>nul
        if %ERRORLEVEL% NEQ 0 (
            echo.
            echo Installation finished, but dotnet is still not found.
            echo You may need to restart your computer or log out and log in again.
            echo Alternatively, check if the dotnet folder is in your PATH environment variable.
            pause
            exit /b 1
        )

        del dotnet-installer.exe
    ) else (
        echo Failed to download .NET SDK installer.
        pause
        exit /b 1
    )
)

REM Restore dependencies
echo Restoring dependencies...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo Failed to restore dependencies. Make sure the SDK is properly installed.
    pause
    exit /b 1
)

REM Build the project
echo Building project...
dotnet build --configuration Release
if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    pause
    exit /b 1
)

REM Run the game
echo Starting the game...
echo.

if exist Pasjans (
    cd Pasjans
    dotnet run
) else (
    echo Error: 'Pasjans' folder not found!
)

echo.
pause
