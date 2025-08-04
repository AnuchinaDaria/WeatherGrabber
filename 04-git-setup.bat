@echo off
echo ========================================
echo 04. Git Setup for GitHub
echo ========================================

echo.
echo Initializing Git repository...
git init

echo.
echo Adding files to Git...
git add .

echo.
echo Creating initial commit...
git commit -m "Initial commit: WeatherGrabber .NET project

- ASP.NET Core Minimal API
- Background Service for weather data collection
- Entity Framework Core with SQLite
- Swagger/OpenAPI documentation
- Clean architecture and professional code"

echo.
echo ========================================
echo Git repository ready!
echo ========================================
echo.
echo Next steps:
echo 1. Create repository on GitHub
echo 2. Run: git remote add origin YOUR_REPO_URL
echo 3. Run: git push -u origin main
echo.
pause 