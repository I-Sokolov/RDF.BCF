#include "pch.h"
#include "Comment.h"
#include "Topic.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
Comment::Comment(Topic& topic, const char* guid)
    : BCFObject(topic.Project())
    , m_topic(topic)
    , m_Guid(topic.Project(), guid)
    , m_Viewpoint(topic)
    , m_bReadFromFile(false)
{
}


/// <summary>
/// 
/// </summary>
void Comment::Read(_xml::_element& elem, const std::string& folder)
{
    m_bReadFromFile = true;

    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Author)
        CHILD_GET_CONTENT(Comment)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(ModifiedAuthor)
        CHILD_READ_MEMBER(Viewpoint)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
ViewPoint* Comment::GetViewPoint()
{
    if (!m_Guid.empty()) {
        return m_topic.ViewPointByGuid(m_Guid.c_str());
    }
    return NULL;
}

/// <summary>
/// 
/// </summary>
bool Comment::SetViewPoint(ViewPoint* viewPoint)
{
    if (!UpdateAuthor()) {
        return false;
    }

    bool ok = true;
    const char* guid = NULL;

    if (viewPoint) {
        guid = viewPoint->GetGuid();
      
        if (!m_topic.ViewPointByGuid(guid)) {
            ok = false;
            guid = NULL;
        }
    }

    m_Viewpoint.SetGuid(guid);
    return ok;
}

/// <summary>
/// 
/// </summary>
bool Comment::SetComment(const char* value)
{
    if (!UpdateAuthor()) {
        return false;
    }

    m_Comment.assign(value);
    return true;
}

/// <summary>
/// 
/// </summary>
bool Comment::UpdateAuthor()
{
    return __super::UpdateAuthor(m_bReadFromFile ? m_ModifiedAuthor : m_Author, m_bReadFromFile ? m_ModifiedAuthor : m_Date);
}
