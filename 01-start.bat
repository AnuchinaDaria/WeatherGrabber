@echo off
echo ========================================
echo 01. Starting WeatherGrabber
echo ========================================

echo.
echo Stopping existing processes...
taskkill /f /im WeatherGrabber.exe 2>nul
timeout /t 2 /nobreak >nul

echo.
echo Starting application...
cd WeatherGrabber
dotnet run

echo.
echo Application started!
echo API: http://localhost:5000
echo Swagger: http://localhost:5000/swagger 