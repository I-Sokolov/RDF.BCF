#include "pch.h"
#include "bcfTypes.h"
#include "BCFTopic.h"
#include "BCFProject.h"
#include "BCFViewPoint.h"
#include "BCFComment.h"
#include "BCFFile.h"
#include "BCFDocumentReference.h"
#include "BCFBimSnippet.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFTopic::BCFTopic(BCFProject& project, ListOfBCFObjects* parentList, const char* guid)
    : XMLFile(project, parentList)
    , m_Guid(project, guid)
    , m_BimSnippets(project)
    , m_bReadFromFile (false)
    , m_Files(project)
    , m_ReferenceLinks(project)
    , m_Labels(project)
    , m_DocumentReferences(project)
    , m_RelatedTopics(project)
    , m_Comments(project)
    , m_Viewpoints(project)
{
}


/// <summary>
/// 
/// </summary>
void BCFTopic::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ(Header)
        CHILD_READ(Topic)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void BCFTopic::WriteRootContent(_xml_writer& writer, const std::string& folder)
{
    REQUIRED_PROP(TopicType);
    REQUIRED_PROP(TopicStatus);
    REQUIRED_PROP(Title);

    Attributes attr;

    WRITE_ELEM(Header);

    ATTR_ADD(Guid);
    ATTR_ADD(ServerAssignedId);
    ATTR_ADD(TopicStatus);
    ATTR_ADD(TopicType);
    WRITE_ELEM(Topic);
}

/// <summary>
/// 
/// </summary>
void BCFTopic::Read_Header(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Files, File)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void BCFTopic::Write_Header(_xml_writer& writer, const std::string& folder)
{
    WRITE_LIST(File)
}

/// <summary>
/// 
/// </summary>
void BCFTopic::Read_Topic(_xml::_element& elem, const std::string& folder)
{
    m_bReadFromFile = true;

    ATTRS_START
        ATTR_GET(Guid)
        ATTR_GET(ServerAssignedId)
        ATTR_GET(TopicStatus)
        ATTR_GET(TopicType)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Title)
        CHILD_GET_LIST(ReferenceLinks, ReferenceLink)
        CHILD_GET_CONTENT(Priority)
        CHILD_GET_CONTENT(Index)
        CHILD_GET_LIST(Labels, Label)
        CHILD_GET_CONTENT(CreationDate)
        CHILD_GET_CONTENT(CreationAuthor)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(ModifiedAuthor)
        CHILD_GET_CONTENT(DueDate)
        CHILD_GET_CONTENT(AssignedTo)
        CHILD_GET_CONTENT(Description)
        CHILD_GET_CONTENT(Stage)
        CHILD_GET_LIST(DocumentReferences, DocumentReference)
        CHILD_GET_LIST(RelatedTopics, RelatedTopic)
        CHILD_GET_LIST(Comments, Comment)
        CHILD_GET_LIST(Viewpoints, ViewPoint)
        CHILD_GET_LIST(BimSnippets, BimSnippet)
    CHILDREN_END
}

void BCFTopic::Write_Topic(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Title);
    WRITE_LIST(ReferenceLink);
    WRITE_CONTENT(Priority);
    WRITE_CONTENT(Index);
    WRITE_LIST(Label);
    WRITE_CONTENT(CreationDate);
    WRITE_CONTENT(CreationAuthor);
    WRITE_CONTENT(ModifiedDate);
    WRITE_CONTENT(ModifiedAuthor);
    WRITE_CONTENT(DueDate);
    WRITE_CONTENT(AssignedTo);
    WRITE_CONTENT(Stage);
    WRITE_CONTENT(Description);
    if (!m_BimSnippets.Items().empty()) {
        m_BimSnippets.Items().front()->Write(writer, folder, "BimSnippet");
    }
    WRITE_LIST(DocumentReference);
    WRITE_LIST(RelatedTopic);
    WRITE_LIST(Comment);
    WRITE_LIST(Viewpoint);

}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetServerAssignedId(const char* val) 
{
    UNNULL;

    if (UpdateAuthor()) {
        m_ServerAssignedId.assign(val);
        return true;
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetTopicStatus(const char* val)
{
    UNNULL;

    if (Project().GetExtensions().CheckElement(BCFTopicStatuses, val)) {
        if (UpdateAuthor()) {
            m_TopicStatus.assign(val);
            return true;
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetTopicType(const char* val)
{
    UNNULL;

    if (Project().GetExtensions().CheckElement(BCFTopicTypes, val)) {
        if (UpdateAuthor()) {
            m_TopicType.assign(val);
            return true;
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetTitle(const char* val)
{
    UNNULL;

    if (UpdateAuthor()) {
        m_Title.assign(val);
        return true;
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetPriority(const char* val)
{
    UNNULL;

    if (Project().GetExtensions().CheckElement(BCFPriorities, val)) {
        if (UpdateAuthor()) {
            m_Priority.assign(val);
            return true;
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetIndex(int val)
{
    if (UpdateAuthor()) {
        return IntToStr(val, m_Index);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetDueDate(const char* val)
{
    UNNULL;
    VALIDATE(DueDate, DateTime);

    if (UpdateAuthor()) {
        m_DueDate.assign(val);
        return true;
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetAssignedTo(const char* val)
{
    UNNULL;

    if (Project().GetExtensions().CheckElement(BCFUsers, val)) {
        if (UpdateAuthor()) {
            m_AssignedTo.assign(val);
            return true;
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetDescription(const char* val)
{
    UNNULL;

    if (UpdateAuthor()) {
        m_Description.assign(val);
        return true;
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetStage(const char* val)
{
    UNNULL;

    if (Project().GetExtensions().CheckElement(BCFStages, val)) {
        if (UpdateAuthor()) {
            m_Stage.assign(val);
            return true;
        }
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::UpdateAuthor()
{
    return __super::UpdateAuthor(m_bReadFromFile ? m_ModifiedAuthor : m_CreationAuthor, m_bReadFromFile ? m_ModifiedDate : m_CreationDate);
}

/// <summary>
/// 
/// </summary>
BCFViewPoint* BCFTopic::ViewPointAdd(const char* guid)
{
    auto viewPoint = new BCFViewPoint(*this, &m_Viewpoints, guid ? guid : "");

    if (viewPoint) {
        m_Viewpoints.Add(viewPoint);
        return viewPoint;
    }
    else {
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFViewPoint* BCFTopic::ViewPointIterate(BCFViewPoint* prev)
{
    return m_Viewpoints.GetNext(prev);
}


/// <summary>
/// 
/// </summary>
BCFFile* BCFTopic::FileAdd(const char* filePath, bool isExternal)
{
    auto file = new BCFFile(*this, &m_Files);

    bool ok = true;

    if (file && filePath && *filePath) {
        ok = ok && file->SetIsExternal(isExternal);
        ok = ok && file->SetReference(filePath);
    }

    if (!ok) {
        delete file;
        file = NULL;
    }

    if (file) {
        m_Files.Add(file);
        return file;
    }
    else {
        return NULL;
    }

}

/// <summary>
/// 
/// </summary>
BCFFile* BCFTopic::FileIterate(BCFFile* prev)
{
    return m_Files.GetNext(prev);
}


/// <summary>
/// 
/// </summary>
BCFViewPoint* BCFTopic::ViewPointByGuid(const char* guid)
{
    if (guid && *guid) {
        for (auto vp : m_Viewpoints.Items()) {
            if (vp && 0 == strcmp(vp->GetGuid(), guid)) {
                return vp;
            }
        }

        Log().add(Log::Level::error, "Invalid reference", "Viewpoint with GUID %s not found in topic %s", guid, GetGuid());
    }
    else {
        Log().add(Log::Level::error, "NULL GUID", "Viewpoingt with NULL GUID is requested from topic %s", GetGuid());
    }

    return NULL;
}

/// <summary>
/// 
/// </summary>
BCFComment* BCFTopic::CommentAdd(const char* guid)
{
    auto comment = new BCFComment(*this, &m_Comments, guid ? guid : "");//"" forces generate guid

    if (comment) {
        m_Comments.Add(comment);
        return comment;
    }
    else {
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
BCFComment* BCFTopic::CommentIterate(BCFComment* prev)
{
    return m_Comments.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFDocumentReference* BCFTopic::DocumentReferenceAdd(const char* urlPath, const char* guid)
{
    auto ref = new BCFDocumentReference(*this, &m_DocumentReferences, guid ? guid : "");

    if (!ref->SetUrlPath(urlPath)){
        delete ref;
        ref = NULL;
    }

    if (ref) {
        m_DocumentReferences.Add(ref);
        return ref;
    }
    else {
        return NULL;
    }

}

/// <summary>
/// 
/// </summary>
BCFDocumentReference* BCFTopic::DocumentReferenceIterate(BCFDocumentReference* prev)
{
    return m_DocumentReferences.GetNext(prev);
}

/// <summary>
/// 
/// </summary>
BCFBimSnippet* BCFTopic::GetBimSnippet(bool forceCreate)
{
    if (forceCreate && m_BimSnippets.Items().empty()) {
        auto snippet = new BCFBimSnippet(*this, &m_BimSnippets);
        m_BimSnippets.Add(snippet);
    }

    if (m_BimSnippets.Items().empty()) {
        return NULL;
    }
    else {
        return m_BimSnippets.Items().front();
    }
}
