#pragma once

#include "BCFObject.h"
#include "GuidStr.h"

struct BCFTopic;

struct BCFDocumentReference : public BCFObject
{
public:
    BCFDocumentReference(BCFTopic& topic, const char* guid = NULL);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

private:
    void Write_DocumentReference(_xml_writer& writer, const std::string& folder);

private:
    GuidStr                    m_Guid;
    std::string                m_DocumentGuid;
    std::string                m_Url;
    std::string                m_Description;
};

