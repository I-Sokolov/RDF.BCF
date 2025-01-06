#include "pch.h"
#include "BCFComment.h"
#include "BCFTopic.h"
#include "BCFProject.h"
#include "BCFViewPoint.h"

/// <summary>
/// 
/// </summary>
BCFComment::BCFComment(BCFTopic& topic, const char* guid)
    : BCFObject(topic.Project())
    , m_topic(topic)
    , m_Guid(topic.Project(), guid)
    , m_Viewpoint(topic)
    , m_readFromFile(false)
{
}


/// <summary>
/// 
/// </summary>
void BCFComment::Read(_xml::_element& elem, const std::string& folder)
{
    m_readFromFile = true;

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
BCFViewPoint* BCFComment::GetViewPoint()
{
    if (*m_Viewpoint.GetGuid()) {
        return m_topic.ViewPointByGuid(m_Viewpoint.GetGuid());
    }
    else {
        return NULL;
    }
}

/// <summary>
/// 
/// </summary>
bool BCFComment::SetViewPoint(BCFViewPoint* viewPoint)
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
bool BCFComment::SetText(const char* value)
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
bool BCFComment::UpdateAuthor()
{
    return __super::UpdateAuthor(m_readFromFile ? m_ModifiedAuthor : m_Author, m_readFromFile ? m_ModifiedDate : m_Date);
}

/// <summary>
/// 
/// </summary>
bool BCFComment::Remove()
{
    return m_topic.CommentRemove(this);
}
