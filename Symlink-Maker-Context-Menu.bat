@echo off
echo 1. Register
echo 2. Unregister
set /p ask="Select option [1/2] : "
if "%ask%" == "1" (
    Symlink-Maker.exe /reg
) else (
    Symlink-Maker.exe /unreg
)