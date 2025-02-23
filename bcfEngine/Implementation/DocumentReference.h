#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"
#include "GuidStr.h"

struct Topic;

struct DocumentReference : public BCFObject, public BCFDocumentReference
{
public:
    DocumentReference(Topic& topic, ListOfBCFObjects* parentList, const char* guid = NULL);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

public:
    //BCF API
    virtual const char* GetGuid() override { return m_Guid.c_str(); }
    virtual const char* GetFilePath() override;
    virtual const char* GetDocumentGuid() override { return m_DocumentGuid.c_str(); }
    virtual const char* GetDescription() override { return m_Description.c_str(); }

    virtual bool SetFilePath(const char* filePath, bool isExternal) override;
    virtual bool SetDescription(const char* val) override { return SetPropertyString (val, m_Description); }

    virtual BCFTopic& GetTopic() override;
    virtual bool Remove() override { return RemoveImpl(); }

private:
    void Write_DocumentReference(_xml_writer& writer, const std::string& folder);

private:
    Topic&                     m_topic;

    GuidStr                    m_Guid;
    std::string                m_DocumentGuid;
    std::string                m_Url;
    std::string                m_Description;
    //v2.1
    std::string                m_isExternal;  
    std::string                m_ReferencedDocument;
};

