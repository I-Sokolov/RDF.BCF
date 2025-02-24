#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"

struct Topic;

struct BimSnippet : public BCFObject, public BCFBimSnippet
{
public:
    BimSnippet(Topic& topic, ListOfBCFObjects* parentList);

public:
    //BCFBimSnippet
    virtual const char* GetSnippetType() override { return m_SnippetType.c_str(); }
    virtual bool        GetIsExternal()override { return GetPropertyBool(m_IsExternal); }
    virtual const char* GetReference()override { return m_Reference.c_str(); }
    virtual const char* GetReferenceSchema()override { return m_ReferenceSchema.c_str(); }

    virtual bool SetSnippetType(const char* val) override;
    virtual bool SetIsExternal(bool val) override { return SetPropertyBool(val, m_IsExternal); }
    virtual bool SetReference(const char* val) override;
    virtual bool SetReferenceSchema(const char* val) override;

    virtual BCFTopic& GetTopic() override;
    virtual bool Remove() override { return RemoveImpl(); }

    virtual Topic* Topic_() override { return &m_topic; }
    virtual Comment* Comment_() override { return NULL; }

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

private:
    Topic&      m_topic;

    std::string m_SnippetType;
    std::string m_IsExternal;
    std::string m_Reference;
    std::string m_ReferenceSchema;
};

