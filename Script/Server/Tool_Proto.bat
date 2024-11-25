@echo off
CALL "%~dp0.\Build\BuildTool.bat"

IF %ERRORLEVEL% EQU 0 (
    wt PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/Tool_Proto.ps1" "wt" > nul 2>&1
) ELSE (
     ECHO "Build Server failed!!!"
     PAUSE
     Exit 0
)
@echo on