#include "pch.h"
#include "Version.h"
#include "BCFProject.h"


/// <summary>
/// 
/// </summary>
void Version::ReadRoot(_xml::_element& elem, const std::string& /*folder*/)
{
    ATTRS_START
        ATTR_GET(VersionId)
    ATTRS_END(UnknownNames::Allowed)
}

/// <summary>
/// 
/// </summary>
void Version::WriteRootElem(_xml_writer& writer, const std::string& folder, Attributes& attr)
{
    ATTR_ADD(VersionId);

    const char* rootName = RootElemName();
    writer.writeTag(rootName, attr, "");
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
    if (m_VersionId == "2.0") {
        return BCFVer_2_0;
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
