rem two arguments
rem arg1 (required): name of 7z file to create
rem arg2 (optional): use "--no-self-contained" to suppress self contained publish

set name=%1
set this_mode=%2

set cmd_7z=C:\Program Files\7-Zip\7z.exe
set dropdir=%HOMEDRIVE%%HOMEPATH%\Desktop\Deploy

set dropfile=%dropdir%\%fnprefix%%name%.7z

del /F/S/Q bin\publish


@echo self contained publish
@echo on
rem original full command
dotnet publish -c Release -f net6.0 -r win81-x64 --self-contained true -o bin\Publish -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true
rem dotnet publish -c Release -f net6.0 -r win81-x64 --self-contained true -o bin\Publish -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true


@if %errorlevel% NEQ 0 (
   @echo Failure Reason Given is %errorlevel%
   @echo on
   del %dropfile%
   exit /b %errorlevel%
)

del %dropfile%

cd bin\Publish
"%cmd_7z%" a -mx9 "%dropfile%" .
