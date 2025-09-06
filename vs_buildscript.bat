@echo off
set "SINGLE_FILE=false" & if "%~1"=="-s" set "SINGLE_FILE=true"

set VERSION=0.0.1
set KDMT_BUILD_PLATFORM=Windows
set BUILD_FOLDER=build\KeepDrivingModTool_%VERSION%-%KDMT_BUILD_PLATFORM%
if %SINGLE_FILE%==true (
set BUILD_FOLDER=%BUILD_FOLDER%-SingleFile
)

ECHO SINGLE_FILE = %SINGLE_FILE%
ECHO VERSION = %VERSION%
ECHO KDMT_BUILD_PLATFORM = %KDMT_BUILD_PLATFORM%
ECHO BUILD_FOLDER = %BUILD_FOLDER%


echo Building UndertaleModTool...
rmdir /s /q %BUILD_FOLDER%
call msbuild UndertaleModCli\ /p:Configuration=Release

echo Building KeepDrivingModTools...
rmdir /s /q KeepDrivingModTool\KeepDrivingModCLI\bin
call dotnet publish KeepDrivingModTool\KeepDrivingModCLI -p:PublishProfile=FolderProfile


echo Migrating package files...
robocopy UndertaleModCli\bin\Release\net8.0\ %BUILD_FOLDER%\UMT_CLI /E
robocopy KeepDrivingModTool\KeepDrivingModCLI\bin\Release\app.publish %BUILD_FOLDER% /E
copy music.kdr %BUILD_FOLDER%\ /y
copy README.md %BUILD_FOLDER%\ /y
copy LICENSE.txt %BUILD_FOLDER%\ /y
copy dirmaker.bat %BUILD_FOLDER%\ /y
del /q %BUILD_FOLDER%\Launcher.exe
del /q %BUILD_FOLDER%\KeepDrivingModCLI.xml
echo Complete!