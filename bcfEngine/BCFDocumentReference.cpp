#include "pch.h"
#include "BCFDocumentReference.h"
#include "BCFProject.h"
#include "BCFTopic.h"

/// <summary>
/// 
/// </summary>
BCFDocumentReference::BCFDocumentReference(BCFTopic& topic, ListOfBCFObjects* parentList, const char* guid)
        : BCFObject(topic.Project(), parentList) 
        , m_topic(topic)
        , m_Guid(topic.Project(), guid)
{
}

/// <summary>
/// 
/// </summary>
void BCFDocumentReference::Read(_xml::_element& elem, const std::string& folder) 
{ 
    std::string isExternal; //v2.1

    ATTRS_START
        ATTR_GET(Guid)
        ATTR_GET_STR(isExternal, isExternal)
    ATTRS_END(UnknownNames::NotAllowed)

    assert(isExternal != "false"); //TODO: implement embedded doucments

    CHILDREN_START
        CHILD_GET_CONTENT(DocumentGuid)
        CHILD_GET_CONTENT(Url)
        CHILD_GET_CONTENT(Description)

        CHILD_GET_CONTENT_STR(ReferencedDocument, m_Url)

    CHILDREN_END
}


/// <summary>
/// 
/// </summary>
void BCFDocumentReference::Write(_xml_writer& writer, const std::string& folder, const char* tag) 
{ 
    assert(0 == strcmp(tag, "DocumentReference"));

    if (m_DocumentGuid.empty()) { //mutually exclusive
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


/// <summary>
/// 
/// </summary>
const char* BCFDocumentReference::GetUrlPath()
{
    if (!m_Url.empty()) {
        return m_Url.c_str();
    }
    else if (!m_DocumentGuid.empty()) {
        assert(!"TODO - local document");
        return "";
    }
    else {
        return "";
    }
}

/// <summary>
/// 
/// </summary>
bool BCFDocumentReference::SetUrlPath(const char* val)
{
    UNNULL;

    if (!*val) {
        m_DocumentGuid.clear();
        m_Url.clear();
        return true;
    }
    else if (IsURL(val)) {
        m_DocumentGuid.clear();
        m_Url.assign(val);
        return true;
    }
    else {
        assert(!"TODO - local document");
        return false;
    }
}

/// <summary>
/// 
/// </summary>
void BCFDocumentReference::UpgradeReadVersion()
{
    if (Project().GetVersion() < BCFVer_3_0) {
        if (m_Guid.IsEmpty()) {
            m_Guid.AssignNew();
        }
    }
}
