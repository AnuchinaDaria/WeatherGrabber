@echo off
echo ========================================
echo 02. Testing WeatherGrabber API
echo ========================================

echo.
echo Testing API endpoints:
echo.

echo 1. GET /weather/Moscow:
curl -s http://localhost:5000/weather/Moscow

echo.
echo 2. GET /weather/Moscow/latest:
curl -s http://localhost:5000/weather/Moscow/latest

echo.
echo ========================================
echo Test completed!
echo ========================================
echo.
pause 