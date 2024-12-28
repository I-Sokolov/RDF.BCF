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
    const char* editor = Project().GetEditor();
    if (!*editor) {
        Project().log().add(Log::Level::error, "Configuration", "Current editor is not set");
        return false;
    }

    if (!Project().GetExtensions().CheckElement(BCFUsers, editor)) {
        return false;
    }

    if (!m_bReadFromFile) {
        m_CreationAuthor.assign(editor);
        m_CreationDate.assign (GetCurrentDate());
    }
    else {
        m_ModifiedAuthor.assign(editor);
        m_ModifiedDate.assign (GetCurrentDate());
    }

    return true;
}