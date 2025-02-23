#pragma once

#include "BCFObject.h"

struct Topic;

/// <summary>
/// 
/// </summary>
struct GuidReference : public BCFObject
{
public:
    GuidReference(Topic& topic, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string&);
    void Write(_xml_writer& writer, const std::string&, const char* tag);
    bool Validate(bool fix);

    const char* GetGuid() { return m_Guid.c_str(); }
    bool SetGuid(const char* val) { return SetPropertyString(val, m_Guid); }

    bool Remove() { return RemoveImpl(); }
private:
    std::string    m_Guid;
};

