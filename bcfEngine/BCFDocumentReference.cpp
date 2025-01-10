#include "pch.h"
#include "BCFDocumentReference.h"
#include "BCFProject.h"
#include "BCFTopic.h"

/// <summary>
/// 
/// </summary>
BCFDocumentReference::BCFDocumentReference(BCFTopic& topic, const char* guid)
        : BCFObject(topic.Project()) 
        , m_Guid(topic.Project(), guid)
{
}

/// <summary>
/// 
/// </summary>
void BCFDocumentReference::Read(_xml::_element& elem, const std::string& folder) 
{ 
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(DocumentGuid)
        CHILD_GET_CONTENT(Url)
        CHILD_GET_CONTENT(Description)
    CHILDREN_END
}


/// <summary>
/// 
/// </summary>
void BCFDocumentReference::Write(_xml_writer& writer, const std::string& folder, const char* tag) 
{ 
    assert(0 == strcmp(tag, "DocumentReference"));

    if (m_DocumentGuid.empty()) {
        REQUIRED_PROP(Url);
    }
    else {
        REQUIRED(Url, m_Url.empty());
    }

    XMLFile::Attributes attr;
    ATTR_ADD(Guid);

    WRITE_ELEM(DocumentReference);    
}

/// <summary>
/// 
/// </summary>
void BCFDocumentReference::Write_DocumentReference(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(DocumentGuid);
    WRITE_CONTENT(Url);
    WRITE_CONTENT(Description);
}


