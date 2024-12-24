#include "pch.h"
#include "XMLFile.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
bool XMLFile::Read(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, FileName());

    try {
        _xml::_document doc(nullptr);
        doc.load(path.c_str());

        if (auto root = doc.getRoot()) {
            ReadRoot(*root);
            ok = true;
        }
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Read file error", "Failed to read %s file. %s", FileName(), ex.what());
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
bool XMLFile::Write(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, FileName());

    try {
        //_xml::_document doc(nullptr);
        //doc.
        ok = true;
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Write file error", "Failed to write %s file. %s", FileName(), ex.what());
    }

    return ok;
}
