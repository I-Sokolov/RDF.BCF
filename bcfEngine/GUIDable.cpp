#include "pch.h"
#include "GUIDable.h"
#include "XMLFile.h"

/// <summary>
/// 
/// </summary>
GUIDable::GUIDable(const char* guid)
{
    if (guid) {
        m_Guid.assign(guid);
    }
    else {
        m_Guid = CreateNewGUID();
    }
}

/// <summary>
/// 
/// </summary>
std::string GUIDable::CreateNewGUID()
{
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

    return oss.str();
}

/// <summary>
/// 
/// </summary>
void GuidReference::Read(_xml::_element& elem)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)
}
