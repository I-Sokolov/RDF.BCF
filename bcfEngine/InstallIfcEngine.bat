
echo Check IFC engine includes

set ProjectDir=%~1
set Platform=%2
set OutputPath=%~3

if .%RDF_IFC_ENGINE_INCLUDE%.==.. goto IncUpdated
echo echo Update IFC engine includes 
xcopy %RDF_IFC_ENGINE_INCLUDE% %ProjectDir%IfcEngine\include /F /Y
:IncUpdated

echo Check IFC engine binaries
if .%RDF_IFC_ENGINE_LIB%.==.. goto LibUpdated
echo Update lib and dll
xcopy %RDF_IFC_ENGINE_LIB%Release\ifcengine.lib %ProjectDir%ifcengine\lib /F /Y
xcopy %RDF_IFC_ENGINE_LIB%Release\ifcengine.dll %ProjectDir%ifcengine\lib /F /Y
:LibUpdated

echo Copy dll to output
xcopy %ProjectDir%ifcengine\lib\ifcengine.dll %OutputPath% /F /Y
