
set ProjectDir=%~1
set Platform=%2
set OutputPath=%~3

set line=copy "%ProjectDir%libzip\libzip.redist.1.1.2.7\build\native\bin\%Platform%\v140\Release\zip.dll" "%OutputPath%" /y
echo %line%
%line%

set line=copy "%ProjectDir%libzip\zlib-msvc-x64.1.2.11.8900\build\native\bin_release\zlib.dll" "%OutputPath%" /y
echo %line%
%line%