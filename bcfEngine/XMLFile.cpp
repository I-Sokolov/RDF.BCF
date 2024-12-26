#include "pch.h"
#include "BCFProject.h"
#include "XMLFile.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
bool XMLFile::ReadFile(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, XMLFileName());

    try {
        _xml::_document doc(nullptr);
        doc.load(path.c_str());

        if (auto root = doc.getRoot()) {
            ReadRoot(*root, bcfFolder);
            ok = true;
        }
    }
    catch (std::exception& ex) {
        m_project.log().add(Log::Level::error, "Read file error", "Failed to read %s file. %s", path.c_str(), ex.what());
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool XMLFile::WriteFile(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, XMLFileName());

    try {
        //_xml::_document doc(nullptr);
        //doc.
        ok = true;
    }
    catch (std::exception& ex) {
        m_project.log().add(Log::Level::error, "Write file error", "Failed to write %s file. %s", path.c_str(), ex.what());
    }

    return ok;
}
