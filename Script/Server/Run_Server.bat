@echo off

NET SESSION >nul 2>&1
set SESSIONLEVEL=%ERRORLEVEL%

:: 查找 wt.exe 是否存在
where wt.exe >nul 2>&1
set WTERELEV=%ERRORLEVEL%

IF %SESSIONLEVEL% NEQ 0 (
	IF %WTERELEV% EQU 0 (
		powershell -Command "Start-Process wt.exe -ArgumentList 'cmd.exe /K %~dp0Run_Server.bat' -Verb runAs" >nul 2>&1
	) ELSE (
		powershell -Command "Start-Process cmd.exe -ArgumentList '/c, %~dp0%Run_Server.bat' -Verb runAs" >nul 2>&1
	)
	exit
)


echo "start execute run server..."
IF %WTERELEV% EQU 0 (
	wt PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/Server.ps1" "wt"  > nul 2>&1
) ELSE (
	PowerShell -ExecutionPolicy unrestricted -File "%~dp0./Run/Server.ps1" "cmd" > nul 2>&1
)


IF %ERRORLEVEL% NEQ 0 (
	ECHO "Execute Faild"
	pause
)
exit