#include "pch.h"
#include "BCFComment.h"
#include "BCFTopic.h"
#include "BCFProject.h"
#include "BCFViewPoint.h"

/// <summary>
/// 
/// </summary>
BCFComment::BCFComment(BCFTopic& topic, ListOfBCFObjects* parentList, const char* guid)
    : BCFObject(topic.Project(), parentList)
    , m_topic(topic)
    , m_Guid(topic.Project(), guid)
    , m_Viewpoint(topic, NULL)
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
        CHILD_READ_MEMBER(Viewpoint)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(ModifiedAuthor)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
void BCFComment::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
{
    if (!*m_Viewpoint.GetGuid()) {
        REQUIRED_PROP(Comment);
    }

    XMLFile::Attributes attr;
    ATTR_ADD(Guid);

    WRITE_ELEM(Comment);
}

/// <summary>
/// 
/// </summary>
void BCFComment::Write_Comment(_xml_writer& writer, const std::string& folder)
{
    WRITE_CONTENT(Date);
    WRITE_CONTENT(Author);
    WRITE_CONTENT(Comment);
    WRITE_MEMBER(Viewpoint);
    WRITE_CONTENT(ModifiedDate);
    WRITE_CONTENT(ModifiedAuthor);
}

/// <summary>
/// 
/// </summary>
BCFViewPoint* BCFComment::GetViewPoint()
{
    if (*m_Viewpoint.GetGuid()) {
        if (auto vp = m_topic.ViewPointByGuid(m_Viewpoint.GetGuid())) {
            return vp;
        }
        Log().add(Log::Level::error, "ViewPoint for comment '%s' referencec but does not exist", m_Comment.empty() ? m_Guid.c_str() : m_Comment.c_str());
    }
    return NULL;
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
            log().add(Log::Level::error, "Invalid viewpoint", "Viewpoint %s is not from this topic %s", guid, m_topic.GetTitle());
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
bool BCFComment::SetText(const char* val)
{
    UNNULL;

    if (!UpdateAuthor()) {
        return false;
    }

    m_Comment.assign(val);
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
void BCFComment::UpgradeReadVersion()
{
    if (Project().GetVersion() < BCFVer_3_0) {
        if (!*m_Viewpoint.GetGuid()) { //empty comment appeared from a file
            if (m_Comment.empty()) {
                m_Comment.assign("empty");
            }
        }
    }
}
