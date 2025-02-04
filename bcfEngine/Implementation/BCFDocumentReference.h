#pragma once

#include "BCFObject.h"
#include "GuidStr.h"

struct BCFTopic;

struct BCFDocumentReference : public BCFObject
{
public:
    BCFDocumentReference(BCFTopic& topic, ListOfBCFObjects* parentList, const char* guid = NULL);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

public:
    const char* GetGuid() { return m_Guid.c_str(); }
    const char* GetFilePath();
    const char* GetDescription() { return m_Description.c_str(); }

    bool SetFilePath(const char* filePath, bool isExternal);
    bool SetDescription(const char* val) { UNNULL; m_Description.assign(val); return true; }

    const char* GetDocumentGuid() { return m_DocumentGuid.c_str(); }

private:
    void Write_DocumentReference(_xml_writer& writer, const std::string& folder);

private:
    BCFTopic&                  m_topic;

    GuidStr                    m_Guid;
    std::string                m_DocumentGuid;
    std::string                m_Url;
    std::string                m_Description;
    //v2.1
    std::string                m_isExternal;  
    std::string                m_ReferencedDocument;
};

