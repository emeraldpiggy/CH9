echo off
if "%PROGRAMFILES(x86)%"=="" (set PROGRAMDIR=%PROGRAMFILES%) else set PROGRAMDIR=%PROGRAMFILES(x86)%
echo Program files directory is: %PROGRAMDIR%
if "%DevEnvDir%"=="" call "%PROGRAMDIR%\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"  x86
