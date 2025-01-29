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
    void UpgradeReadVersion();

public:
    const char* GetGuid() { return m_Guid.c_str(); }
    const char* GetUrlPath();
    const char* GetDescription() { return m_Description.c_str(); }

    bool SetUrlPath(const char* val);
    bool SetDescription(const char* val) { UNNULL; m_Description.assign(val); return true; }

private:
    void Write_DocumentReference(_xml_writer& writer, const std::string& folder);

private:
    BCFTopic&                  m_topic;

    GuidStr                    m_Guid;
    std::string                m_DocumentGuid;
    std::string                m_Url;
    std::string                m_Description;
};

