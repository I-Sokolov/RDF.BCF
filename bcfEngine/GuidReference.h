#pragma once

#include "BCFObject.h"

struct BCFTopic;

/// <summary>
/// 
/// </summary>
struct GuidReference : public BCFObject
{
public:
    GuidReference(BCFTopic& topic);

    void Read(_xml::_element& elem, const std::string&);

    const char* GetGuid() { return m_Guid.c_str(); }
    void SetGuid(const char* guid) { if (guid && *guid) m_Guid.assign(guid); else m_Guid.clear(); }

private:
    std::string    m_Guid;
};
