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
    virtual bool        GetIsExternal()override { return StrToBool(m_IsExternal); }
    virtual const char* GetReference()override { return m_Reference.c_str(); }
    virtual const char* GetReferenceSchema()override { return m_ReferenceSchema.c_str(); }

    virtual bool SetSnippetType(const char* val) override;
    virtual bool SetIsExternal(bool val) override { return BoolToStr(val, m_IsExternal); }
    virtual bool SetReference(const char* val) override;
    virtual bool SetReferenceSchema(const char* val) override;

    virtual BCFTopic& GetTopic() override;

    virtual bool Remove() override { return RemoveImpl(); }

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

