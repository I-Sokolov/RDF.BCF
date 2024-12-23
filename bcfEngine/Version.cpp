#include "pch.h"
#include "Version.h"
#include "FileSystem.h"
#include "XMLMacro.h"

#define FILE_NAME "bcf.version"

/// <summary>
/// 
/// </summary>
bool Version::Read(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, FILE_NAME);

    try {
        _xml::_document doc(nullptr);
        doc.load(path.c_str());

        if (auto root = doc.getRoot()) {
            ReadRoot(*root);
            ok = true;
        }
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Read file error", "Failed to read %s file. %s", FILE_NAME, ex.what());
    }

    return ok;

}


/// <summary>
/// 
/// </summary>
bool Version::Write(const std::string& bcfFolder)
{
    bool ok = false;

    std::string path(bcfFolder);
    FileSystem::AddPath(path, FILE_NAME);

    try {
        //_xml::_document doc(nullptr);
        //doc.
        ok = true;
    }
    catch (std::exception& ex) {
        m_log.add(Log::Level::error, "Write file error", "Failed to write %s file. %s", FILE_NAME, ex.what());
    }

    return ok;
}

/// <summary>
/// 
/// </summary>
void Version::ReadRoot(_xml::_element& elem)
{
    GET_ATTR(VersionId)
    END_ATTR(UnknownNames::Allowed)
}

/// <summary>
/// 
/// </summary>
BCFVersion Version::Get()
{
    if (m_VersionId == "3.0") {
        return BCFVer_3_0;
    }
    if (m_VersionId == "2.1") {
        return BCFVer_2_1;
    }
    else {
        assert(false);
        return BCFVerNotSupported;
    }
}

/// <summary>
/// 
/// </summary>
void Version::Set(BCFVersion version)
{
    switch (version) {
        case BCFVer_2_1:
            m_VersionId = "2.1";
            break;
        default:
            m_VersionId = "3.0";
            break;
    }
}
