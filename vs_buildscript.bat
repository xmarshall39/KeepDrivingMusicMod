@echo off
echo Building UndertaleModTool...
rmdir /s /q build
call msbuild /p:Configuration=Release
echo Building KeepDrivingModTools...
call msbuild KeepDrivingModTool/KeepDrivingModTool.sln /p:Configuration=Release
echo Migrating package files...
robocopy UndertaleModCli/bin/Release/net8.0/ build/UMT_CLI /E
robocopy KeepDrivingModTool/KeepDrivingModCLI/bin/Release build /E
echo Complete!