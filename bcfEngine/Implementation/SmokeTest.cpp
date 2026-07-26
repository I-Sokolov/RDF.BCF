
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
extern void SmokeTest_ValidateXSD(const char* xsdName, const char* xmlFilePath, BCFVersion version)
{
    std::string schemaFolder("..");
    FileSystem::AddPath(schemaFolder, "bcfEngine");
    FileSystem::AddPath(schemaFolder, "Schemas");
    
    std::string exeFilePath(schemaFolder);
    FileSystem::AddPath(exeFilePath, "xml.exe");

    std::string xsdFilePath(schemaFolder);
    switch (version) {
        case BCFVer_2_1:
            FileSystem::AddPath(xsdFilePath, "2.1");
            break;
        case BCFVer_3_0:
            FileSystem::AddPath(xsdFilePath, "3.0");
            break;
        default:
            std::cerr << "Not supported version: " << version << std::endl;
            exit(13);
    }
    FileSystem::AddPath(xsdFilePath, xsdName);


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

    ok = bcf->WriteFile("Test.bcf", BCFVer_2_1);
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


std::string SmokeTest_UTF8toCodes(const char* utf8)
{
    if (!utf8)
        return {};

    std::string result;

    const unsigned char* p =
        reinterpret_cast<const unsigned char*>(utf8);

    auto isContinuation = [](unsigned char c) -> bool
        {
            return (c & 0xC0) == 0x80;
        };

    auto LEN = strlen(utf8);
    while ((const char*)p-utf8 < (int)LEN)
    {
        unsigned char c = *p;
        if (c < 0x80)
        {
            // ASCII as is
            result += static_cast<char>(c);
            ++p;
            continue;
        }

        uint32_t codePoint = 0;
        int length = 0;

        if ((c & 0xE0) == 0xC0)
        {
            // 2-byte UTF-8
            if (!p[1] || !isContinuation(p[1]))
            {
                result += "<Invalid 2-byte>";
            }

            codePoint =
                ((c & 0x1F) << 6) |
                (p[1] & 0x3F);

            length = 2;

            // 2-byte sequence must represent >= U+0080
            if (codePoint < 0x80)
            {
                result += "<Overlong 2-byte encoding>";
            }
        }
        else if ((c & 0xF0) == 0xE0)
        {
            // 3-byte UTF-8
            if (!p[1] ||
                !p[2] ||
                !isContinuation(p[1]) ||
                !isContinuation(p[2]))
            {
                result += "<Invalid 3-byte>";
            }

            codePoint =
                ((c & 0x0F) << 12) |
                ((p[1] & 0x3F) << 6) |
                (p[2] & 0x3F);

            length = 3;

            if (codePoint < 0x800)
            {
                result += "<Overlong 3-byte encoding>";
            }

            if (codePoint >= 0xD800 &&
                codePoint <= 0xDFFF)
            {
                result += "<UTF-16 surrogate range is not valid UTF-8>";
            }
        }
        else if ((c & 0xF8) == 0xF0)
        {
            // 4-byte UTF-8
            if (!p[1] ||
                !p[2] ||
                !p[3] ||
                !isContinuation(p[1]) ||
                !isContinuation(p[2]) ||
                !isContinuation(p[3]))
            {
                result += "<Invalid 4-byte>";
            }

            codePoint =
                ((c & 0x07) << 18) |
                ((p[1] & 0x3F) << 12) |
                ((p[2] & 0x3F) << 6) |
                (p[3] & 0x3F);

            length = 4;

            // Overlong encoding
            if (codePoint < 0x10000)
            {
                result += "<Overlong 4-byte encoding>";
            }

            if (codePoint > 0x10FFFF)
            {
                result += "Exceeds Unicode maximum";
            }
        }
        else
        {
            result += "<Invalid UTF-8 leading byte>";
            codePoint = c;
        }

        // Unicode code point
        std::ostringstream ss;

        if (codePoint <= 0xFFFF)
        {
            ss << "\\u"
                << std::uppercase
                << std::hex
                << std::setw(4)
                << std::setfill('0')
                << codePoint;
        }
        else
        {
            // Convert Unicode code point > U+FFFF
            // to UTF-16 surrogate pair.

            uint32_t value =
                codePoint - 0x10000;

            uint16_t high =
                static_cast<uint16_t>(
                    0xD800 + (value >> 10)
                    );

            uint16_t low =
                static_cast<uint16_t>(
                    0xDC00 + (value & 0x3FF)
                    );

            ss << "\\u"
                << std::uppercase
                << std::hex
                << std::setw(4)
                << std::setfill('0')
                << high
                << "\\u"
                << std::setw(4)
                << low;
        }

        result += ss.str();

        p += length;
    }

    return result;
}


#endif //SMOKE_TEST

