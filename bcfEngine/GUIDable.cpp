#include "pch.h"
#include "GUIDable.h"
#include "XMLFile.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
void BCFGuid::CreateNew()
{
    assert(empty());

    std::ostringstream oss;
    for (int i = 0; i < 4; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 2; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 2; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 2; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }
    oss << "-";
    for (int i = 0; i < 6; i++) {
        oss << std::hex << std::setw(2) << std::setfill('0') << rand() % 256;
    }

    assign(oss.str());
}

/// <summary>
/// 
/// </summary>
void BCFGuid::assign(const std::string& s)
{ 
    if (empty()) {
        __super::assign(s);
    }
    else if (*this!=s) {
        m_project.log().add(Log::Level::warning, "Inconsitent GUIDs",  "%s is aloso referenced as %s", c_str(), s.c_str());
    }
}


/// <summary>
/// 
/// </summary>
void GuidReference::Read(_xml::_element& elem, const std::string& /*folder*/)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)
}
