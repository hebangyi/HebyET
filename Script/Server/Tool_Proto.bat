@echo off

:: 查找 wt.exe 是否存在
where wt.exe >nul 2>&1
set WTERELEV=%ERRORLEVEL%

IF %WTERELEV% EQU 0 (
	wt PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/Tool_Proto.ps1" "wt"  > nul 2>&1
) ELSE (
	PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/Tool_Proto.ps1" "cmd" > nul 2>&1
)

PAUSE
@echo on