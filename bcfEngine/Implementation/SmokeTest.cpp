
#include "pch.h"
#include "bcfAPI.h"
#include "SmokeTest.h"
#include "FileSystem.h"

#include <filesystem>


#ifdef SMOKE_TEST

#define ASSERT assert

/// <summary>
/// 
/// </summary>
extern void SmokeTest_ValidateXSD(const char* xsdName, const char* xmlFilePath)
{
    std::string schemaFolder("..");
    FileSystem::AddPath(schemaFolder, "bcfEngine");
    FileSystem::AddPath(schemaFolder, "Schemas");

    std::string xsdFilePath(schemaFolder);
    FileSystem::AddPath(xsdFilePath, xsdName);

    std::string exeFilePath(schemaFolder);
    FileSystem::AddPath(exeFilePath, "xml.exe");

    char cmdLine[1024];
    sprintf_s(cmdLine, "%s val -e -s %s %s", exeFilePath.c_str(), xsdFilePath.c_str(), xmlFilePath);

    STARTUPINFOA si;
    PROCESS_INFORMATION pi;
    ZeroMemory(&si, sizeof(si));
    ZeroMemory(&pi, sizeof(pi));
    si.cb = sizeof(si);

    if (!CreateProcessA(NULL, cmdLine, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi)) {
        std::cerr << "Failed to create process. Error code: " << GetLastError() << std::endl;
        exit(13);
    }

    WaitForSingleObject(pi.hProcess, INFINITE);

    DWORD exitCode;
    GetExitCodeProcess(pi.hProcess, &exitCode);

    CloseHandle(pi.hProcess);
    CloseHandle(pi.hThread);

    if (exitCode != 0) {
        std::cerr << "XML file mismatch schema " << xmlFilePath << std::endl;
        exit(13);
    }
}

/// <summary>
/// 
/// </summary>
static void TestFromDataSet(const char* filepath)
{
    printf("\n\nTEST FILE %s\n", filepath);

    auto bcf = BCFProject::Create();

    bcf->SetOptions(NULL, false, true);

    auto ok = bcf->ReadFile(filepath, true);
    ASSERT(ok);

    ok = bcf->WriteFile("Test.bcf", BCFVer_3_0);
    ASSERT(ok);

    bcf->Delete();    
}

/// <summary>
/// 
/// </summary>
RDFBCF_EXPORT void SmokeTest_DataSet(const char* folder)
{
    for (const auto& entry : std::filesystem::directory_iterator(folder)) {
        if (entry.is_directory()) {
            if (entry.path().filename() != "unzipped") {
                SmokeTest_DataSet(entry.path().string().c_str());
            }
        }
        else {
            auto ext = entry.path().extension();
            if (ext.string() == ".bcf" || ext.string() == ".bcfzip") {
                TestFromDataSet(entry.path().string().c_str());
            }
        }
    }
}

#endif //SMOKE_TEST