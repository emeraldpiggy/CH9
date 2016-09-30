echo off
call .\SetVsToolsPath.cmd

setlocal
set STARTTIME=%TIME%
if "%1"=="-nopause" set NoPause=1 & shift

::argument shifted
if "%1"=="release" (
	echo ### Step 1. Build Solution in release
	msbuild .\CH9.sln /p:RunCodeAnalysis=false /maxcpucount /verbosity:quiet /p:Configuration=Release
	if ERRORLEVEL 1 popd & goto FAILED 
	goto SUCCEED
)

echo ### Step 1. Build Solution in debug
msbuild .\CH9.sln /p:RunCodeAnalysis=false /maxcpucount /verbosity:quiet /p:Configuration=Debug
if ERRORLEVEL 1 popd & goto FAILED 


:SUCCEED
echo Build is GREEN
echoTimeTaken -t %STARTTIME% -c "Build Took:"
endlocal
if NOT DEFINED NoPause pause

exit /B 0

:FAILED
echo Build is RED
echoTimeTaken -t %STARTTIME% -c "Build Took:"
if NOT DEFINED NoPause pause
endlocal
exit /B 1