#pragma once

#include "BCFObject.h"
struct BCFTopic;

struct BCFBimSnippet : public BCFObject
{
public:
    BCFBimSnippet(BCFTopic& project, ListOfBCFObjects* parentList);

public:
    const char* GetSnippetType() { return m_SnippetType.c_str(); }
    bool        GetIsExternal() { return StrToBool(m_IsExternal); }
    const char* GetReference() { return m_Reference.c_str(); }
    const char* GetReferenceSchema() { return m_ReferenceSchema.c_str(); }

    bool SetSnippetType(const char* val);
    bool SetIsExternal(bool val) { return BoolToStr(val, m_IsExternal); }
    bool SetReference(const char* val);
    bool SetReferenceSchema(const char* val);

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

private:
    std::string m_SnippetType;
    std::string m_IsExternal;
    std::string m_Reference;
    std::string m_ReferenceSchema;
};

