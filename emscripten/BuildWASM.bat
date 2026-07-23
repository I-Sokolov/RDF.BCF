mkdir Build.Wasm.Obj
pushd Build.Wasm.Obj

REM ------------------ CHECK ENV ----------------------

if not defined EMSDK (
    echo
    echo !!! EMSDK is not installed or emsdk_env.bat was not called
    exit /b 13
)

set errorlevel=0

REM ------------------ COMPILER OPTIONS ----------------------
set BCF_SRC=../../bcfEngine/

set EMCC_OPTS=-Wno-deprecated-declarations -I%BCF_SRC% -I%BCF_SRC%ifcEngine/include/ -DNDEBUG -DMAPPING_CIS2_DISABLED -DMAPPING_AP242_DISABLED -DEMBEDDED_SCHEMAS -O2

if .%1.==.link. goto LINK

REM ------------------ IFC -------------------------------------
call ..\CompileIFCEngine.bat
if %errorlevel% neq 0 exit /b %errorlevel%

REM ------------------ kubazip - 1 chunk(s) ----------------------

REM Compile chunk 1
call emcc %EMCC_OPTS% -c %BCF_SRC%kubazip/zip.c
if %errorlevel% neq 0 exit /b %errorlevel%

REM Combine chunk 1
call emcc zip.o -r -o kubazip_0.o
if %errorlevel% neq 0 exit /b %errorlevel%

REM ------------------ XMLParser - 1 chunk(s) ----------------------

REM Compile chunk 1
call emcc %EMCC_OPTS% -c %BCF_SRC%XMLParser/_net.cpp %BCF_SRC%XMLParser/_reader.cpp %BCF_SRC%XMLParser/_serialization.cpp %BCF_SRC%XMLParser/_xml.cpp
if %errorlevel% neq 0 exit /b %errorlevel%

REM Combine chunk 1
call emcc _net.o _reader.o _serialization.o _xml.o -r -o XMLParser_0.o
if %errorlevel% neq 0 exit /b %errorlevel%

REM ------------------ bcfengine - 1 chunk(s) ----------------------

REM Compile chunk 1
call emcc %EMCC_OPTS% -c %BCF_SRC%Implementation/Archivator.cpp %BCF_SRC%Implementation/bcfEngine.cpp %BCF_SRC%Implementation/BCFObject.cpp %BCF_SRC%Implementation/BimFile.cpp %BCF_SRC%Implementation/BimSnippet.cpp %BCF_SRC%Implementation/Bitmap.cpp %BCF_SRC%Implementation/ClippingPlane.cpp %BCF_SRC%Implementation/Coloring.cpp %BCF_SRC%Implementation/Comment.cpp %BCF_SRC%Implementation/Component.cpp %BCF_SRC%Implementation/DocumentReference.cpp %BCF_SRC%Implementation/Documents.cpp %BCF_SRC%Implementation/Extensions.cpp %BCF_SRC%Implementation/FileSystem.cpp %BCF_SRC%Implementation/GuidReference.cpp %BCF_SRC%Implementation/GuidStr.cpp %BCF_SRC%Implementation/Line.cpp %BCF_SRC%Implementation/ListOf.cpp %BCF_SRC%Implementation/Log.cpp %BCF_SRC%Implementation/pch.cpp %BCF_SRC%Implementation/Project.cpp %BCF_SRC%Implementation/ProjectInfo.cpp %BCF_SRC%Implementation/SmokeTest.cpp %BCF_SRC%Implementation/Topic.cpp %BCF_SRC%Implementation/Version.cpp %BCF_SRC%Implementation/ViewPoint.cpp %BCF_SRC%Implementation/XMLFile.cpp %BCF_SRC%Implementation/XMLPoint.cpp
if %errorlevel% neq 0 exit /b %errorlevel%

REM Combine chunk 1
call emcc Archivator.o bcfEngine.o BCFObject.o BimFile.o BimSnippet.o Bitmap.o ClippingPlane.o Coloring.o Comment.o Component.o DocumentReference.o Documents.o Extensions.o FileSystem.o GuidReference.o GuidStr.o Line.o ListOf.o Log.o pch.o Project.o ProjectInfo.o SmokeTest.o Topic.o Version.o ViewPoint.o XMLFile.o XMLPoint.o -r -o bcfengine_0.o
if %errorlevel% neq 0 exit /b %errorlevel%

REM ------------------ ASSEMBLY LIBRARY ----------------------
:LINK

call emar rcs bcfEngine.a Common_0.o GeometryKernel_0.o GeometryKernel_1.o GeometryKernel_2.o GeometryKernel_3.o GeometryKernel_4.o IFC2x3_0.o IFC2x3_1.o IFC2x3_2.o IFC4_0.o IFC4_1.o IFC4_2.o IFC4_3.o Repo_0.o ToolBox_0.o ToolBoxEx_0.o kubazip_0.o XMLParser_0.o bcfengine_0.o
if %errorlevel% neq 0 exit /b %errorlevel%

call emcc --bind %EMCC_OPTS% bcfEngine.a ../emEngine.cpp -s ALLOW_MEMORY_GROWTH=1 -s GROWABLE_ARRAYBUFFERS=0 -s STACK_SIZE=5242880 -s FORCE_FILESYSTEM=1 -s EXPORTED_RUNTIME_METHODS="['FS']" -s EXPORTED_FUNCTIONS="['_malloc','_free','stringToUTF8','UTF8ToString']" -s ASSERTIONS=2 -s MODULARIZE=1 -s EXPORT_ES6=1 -o ../RDF.BCF.html
if %errorlevel% neq 0 exit /b %errorlevel%

popd
