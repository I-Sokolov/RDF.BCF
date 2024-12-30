#include "pch.h"
#include "bcfTypes.h"
#include "Topic.h"
#include "BCFProject.h"
#include "FileSystem.h"

/// <summary>
/// 
/// </summary>
Topic::Topic(BCFProject& project, const char* guid)
    : XMLFile(project)
    , m_Guid(project, guid)
    , m_BimSnippet(project)
    , m_bReadFromFile (false)
{
}


/// <summary>
/// 
/// </summary>
void Topic::ReadRoot(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ(Header)
        CHILD_READ(Topic)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Header(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_GET_LIST(Files, File)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void Topic::Read_Topic(_xml::_element& elem, const std::string& folder)
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
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool Topic::SetServerAssignedId(const char* val) 
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
bool Topic::SetTopicStatus(const char* val)
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
bool Topic::SetTopicType(const char* val)
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
bool Topic::SetTitle(const char* val)
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
bool Topic::SetPriority(const char* val)
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
bool Topic::SetIndex(int val)
{
    if (UpdateAuthor()) {
        return SetIntVal(m_Index, val);
    }
    return false;
}

/// <summary>
/// 
/// </summary>
bool Topic::SetDueDate(const char* val)
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
bool Topic::SetAssignedTo(const char* val)
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
bool Topic::SetDescription(const char* val)
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
bool Topic::SetStage(const char* val)
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
bool Topic::UpdateAuthor()
{
    return __super::UpdateAuthor(m_bReadFromFile ? m_ModifiedAuthor : m_CreationAuthor, m_bReadFromFile ? m_ModifiedAuthor : m_CreationDate);
}

/// <summary>
/// 
/// </summary>
BCFIndex Topic::ViewPointCreate(const char* guid)
{
    auto viewPoint = new ViewPoint(*this, guid ? guid : "");

    if (viewPoint) {
        /*
        bool ok = topic->SetTopicType(type);
        ok = ok && topic->SetTitle(title);
        ok = ok && topic->SetTopicStatus(status);

        if (!ok) {
            delete topic;
            topic = NULL;
        }
        */
    }

    if (viewPoint) {
        m_Viewpoints.push_back(viewPoint);
        return (BCFIndex)m_Comments.size() - 1;
    }
    else {
        return BCFIndex_ERROR;
    }
}

/// <summary>
/// 
/// </summary>
ViewPoint* Topic::ViewPointByGuid(const char* guid)
{
    if (guid && *guid) {
        for (auto vp : m_Viewpoints) {
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
BCFIndex Topic::CommentCreate(const char* guid)
{
    auto topic = new Comment(*this, guid ? guid : "");

    if (topic) {
        /*
        bool ok = topic->SetTopicType(type);
        ok = ok && topic->SetTitle(title);
        ok = ok && topic->SetTopicStatus(status);

        if (!ok) {
            delete topic;
            topic = NULL;
        }
        */
    }

    if (topic) {
        m_Comments.push_back(topic);
        return (BCFIndex)m_Comments.size() - 1;
    }
    else {
        return BCFIndex_ERROR;
    }
}
