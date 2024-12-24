#include "pch.h"
#include "GUIDable.h"

/// <summary>
/// 
/// </summary>
GUIDable::GUIDable(const char* guid)
{
    if (guid) {
        m_guid.assign(guid);
    }
    else {
        m_guid = CreateNewGUID();
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
