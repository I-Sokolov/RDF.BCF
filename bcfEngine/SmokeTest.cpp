
#include "pch.h"
#include "SmokeTest.h"
#include "FileSystem.h"

#ifdef SMOKE_TEST

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

    STARTUPINFO si;
    PROCESS_INFORMATION pi;
    ZeroMemory(&si, sizeof(si));
    ZeroMemory(&pi, sizeof(pi));
    si.cb = sizeof(si);

    if (!CreateProcess(NULL, cmdLine, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi)) {
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

#endif //SMOKE_TEST