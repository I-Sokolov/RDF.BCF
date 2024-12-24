#include "pch.h"
#include "XMLFile.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
bool XMLFile::Read(const std::string& bcfFolder)
{
    bool ok = false;

    std::string relPath;
    GetRelativePathName(relPath);

    std::string path(bcfFolder);
    FileSystem::AddPath(path, relPath.c_str());

    try {
        _xml::_document doc(nullptr);
        doc.load(path.c_str());

        if (auto root = doc.getRoot()) {
            ReadRoot(*root);
            ok = true;
        }
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Read file error", "Failed to read %s file. %s", path.c_str(), ex.what());
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool XMLFile::Write(const std::string& bcfFolder)
{
    bool ok = false;

    std::string relPath;
    GetRelativePathName(relPath);

    std::string path(bcfFolder);
    FileSystem::AddPath(path, relPath.c_str());

    try {
        //_xml::_document doc(nullptr);
        //doc.
        ok = true;
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Write file error", "Failed to write %s file. %s", path.c_str(), ex.what());
    }

    return ok;
}
