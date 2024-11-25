@echo off
PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/StopAll.ps1"

IF %ERRORLEVEL% EQU 0 (
    wt PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/ServerTool.ps1" "wt"  > nul 2>&1
) ELSE (
     ECHO "Execute Last Faild"
     PAUSE
     Exit 0
)
IF %ERRORLEVEL% NEQ 0 (
    PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/ServerTool.ps1" "cmd"
)
@echo on