#include "pch.h"
#include "bcfTypes.h"
#include "BCFTopic.h"
#include "BCFProject.h"
#include "BCFViewPoint.h"
#include "BCFComment.h"
#include "BCFFile.h"
#include "BCFDocumentReference.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
BCFTopic::BCFTopic(BCFProject& project, const char* guid)
    : XMLFile(project)
    , m_Guid(project, guid)
    , m_BimSnippet(project)
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
void BCFTopic::Read_Header(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Files, File)
    CHILDREN_END
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
        CHILD_GET_LIST(DocumentReferences, BCFDocumentReference)
        CHILD_GET_LIST(RelatedTopics, RelatedTopic)
        CHILD_GET_LIST(Comments, BCFComment)
        CHILD_GET_LIST(Viewpoints, BCFViewPoint)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::SetServerAssignedId(const char* val) 
{
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
    return __super::UpdateAuthor(m_bReadFromFile ? m_ModifiedAuthor : m_CreationAuthor, m_bReadFromFile ? m_ModifiedAuthor : m_CreationDate);
}

/// <summary>
/// 
/// </summary>
bool BCFTopic::Remove(void)
{
    return m_project.TopicRemove(this);
}

/// <summary>
/// 
/// </summary>
BCFViewPoint* BCFTopic::ViewPointCreate(const char* guid)
{
    auto viewPoint = new BCFViewPoint(*this, guid ? guid : "");

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
bool BCFTopic::ViewPointRemove(BCFViewPoint* viewPoint)
{
    return m_Viewpoints.Remove(viewPoint);
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
BCFComment* BCFTopic::CommentCreate(const char* guid)
{
    auto comment = new BCFComment(*this, guid ? guid : "");

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
bool        BCFTopic::CommentRemove(BCFComment* comment)
{
    return m_Comments.Remove(comment);
}