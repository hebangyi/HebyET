@echo off
PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/StopAll.ps1"
CALL "%~dp0.\Build\Build.bat"

IF %ERRORLEVEL% EQU 0 (
    wt PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/Server.ps1" "wt" > nul 2>&1
) ELSE (
     ECHO "Build Server failed!!!"
     PAUSE
     Exit 0
)
@echo on