#include "pch.h"
#include "BCFDocumentReference.h"
#include "BCFProject.h"
#include "BCFTopic.h"
#include "FileSystem.h"

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
    ATTRS_START
        ATTR_GET(Guid)
        ATTR_GET(isExternal) //v2.1
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(DocumentGuid)
        CHILD_GET_CONTENT(Url)
        CHILD_GET_CONTENT(Description)
        CHILD_GET_CONTENT(ReferencedDocument) //v2.1
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
const char* BCFDocumentReference::GetFilePath()
{
    if (!m_Url.empty()) {
        return m_Url.c_str();
    }
    else if (!m_DocumentGuid.empty()) {
        return Project().GetDocuments().GetFilePath(m_DocumentGuid.c_str());
    }
    else {
        return "";
    }
}

/// <summary>
/// 
/// </summary>
bool BCFDocumentReference::SetFilePath(const char* filePath, bool isExternal)
{
    if (!filePath || !*filePath) {
        m_DocumentGuid.clear();
        m_Url.clear();
        return true;
    }
    else if (isExternal) {
        m_DocumentGuid.clear();
        m_Url.assign(filePath);
        return true;
    }
    else {
        m_DocumentGuid = Project().GetDocuments().Add (filePath);
        m_Url.clear();
        return !m_DocumentGuid.empty();
    }
}

/// <summary>
/// 
/// </summary>
void BCFDocumentReference::UpgradeReadVersion(const std::string& folder)
{
    if (Project().GetVersion() < BCFVer_3_0) {
        
        if (m_Guid.IsEmpty()) {
            m_Guid.AssignNew();
        }

        if (StrToBool(m_isExternal)) {
            SetFilePath(m_ReferencedDocument.c_str(), true);
        }
        else {
            std::string path(folder);
            FileSystem::AddPath(path, m_ReferencedDocument.c_str());
            SetFilePath(path.c_str(), false);
        }
    }
}
