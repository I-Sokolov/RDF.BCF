#pragma once

#include "BCFObject.h"

struct Topic;
struct Comment;

/// <summary>
/// 
/// </summary>
struct GuidReference : public BCFObject
{
public:
    GuidReference(Topic& topic, ListOfBCFObjects* parentList);
    GuidReference(Comment& topic, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string&);
    void Write(_xml_writer& writer, const std::string&, const char* tag);
    bool Validate(bool fix);

    const char* GetGuid() { return m_Guid.c_str(); }
    bool SetGuid(const char* val) { return SetPropertyString(val, m_Guid); }

    bool Remove() { return RemoveImpl(); }

    virtual Topic* Topic_() override { return &m_topic; }
    virtual Comment* Comment_() override { return m_comment; }

private:
    Topic&          m_topic;
    Comment*        m_comment;

    std::string     m_Guid;
};

