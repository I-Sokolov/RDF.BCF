#include "pch.h"
#include "Comment.h"
#include "Topic.h"
#include "Project.h"
#include "ViewPoint.h"

/// <summary>
/// 
/// </summary>
Comment::Comment(Topic& topic, ListOfBCFObjects* parentList, const char* guid)
    : BCFObject(topic.Project_(), parentList)
    , m_topic(topic)
    , m_Guid(topic.Project_(), guid)
    , m_Viewpoint(*this, NULL)
    , m_readFromFile(false)
{
}


/// <summary>
/// 
/// </summary>
void Comment::Read(_xml::_element& elem, const std::string& folder)
{
    m_readFromFile = true;

    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)

    std::string ignore; //v2.0
    std::string verbalStatus;
    std::string status;

    CHILDREN_START
        CHILD_GET_CONTENT(Date)
        CHILD_GET_CONTENT(Author)
        CHILD_GET_CONTENT(Comment)
        CHILD_READ_MEMBER(Viewpoint)
        CHILD_GET_CONTENT(ModifiedDate)
        CHILD_GET_CONTENT(ModifiedAuthor)
        //v2.0
        CHILD_GET_CONTENT_STR(Topic, ignore) 
        CHILD_GET_CONTENT_STR(VerbalStatus, verbalStatus)
        CHILD_GET_CONTENT_STR(Status, status)
        CHILD_GET_CONTENT_STR(ReplyToComment, ignore)
    CHILDREN_END;

    //v2.0
    if (!status.empty()) { 
        if (!m_Comment.empty()) {
            m_Comment.push_back('\n');
        }
        m_Comment.append(status);
    }

    if (!verbalStatus.empty()) {
        if (!m_Comment.empty()) {
            m_Comment.push_back('\n');
        }
        m_Comment.append(verbalStatus);
    }
}

/// <summary>
/// 
/// </summary>
bool Comment::Validate(bool fix)
{
    bool valid = true;

    if (!*m_Viewpoint.GetGuid()) {
        REQUIRED_PROP(Comment);
    }

    if (!valid && fix) {
        Remove();
        return true;
    }

    return valid;
}

/// <summary>
/// 
/// </summary>
void Comment::Write(_xml_writer& writer, const std::string& folder, const char* /*tag*/)
{
    XMLFile::Attributes attr;
    ATTR_ADD(Guid);

    WRITE_ELEM(Comment);
}

/// <summary>
/// 
/// </summary>
void Comment::Write_Comment(_xml_writer& writer, const std::string& folder)
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
BCFViewPoint* Comment::GetViewPoint()
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
bool Comment::SetViewPoint(BCFViewPoint* viewPoint)
{
    bool ok = true;
    const char* guid = NULL;

    if (viewPoint) {
        guid = viewPoint->GetGuid();
      
        if (!m_topic.ViewPointByGuid(guid)) {
            Log_().add(Log::Level::error, "Invalid viewpoint", "Viewpoint %s is not from this topic %s", guid, m_topic.GetTitle());
            ok = false;
            guid = NULL;
        }
    }

    return m_Viewpoint.SetGuid(guid);
}

/// <summary>
/// 
/// </summary>
bool Comment::SetText(const char* val)
{
    return SetPropertyString(val, m_Comment);
}

/// <summary>
/// 
/// </summary>
bool Comment::SetEditorAndDate()
{
    return BCFObject::SetEditorAndDate(m_readFromFile ? m_ModifiedAuthor : m_Author, m_readFromFile ? m_ModifiedDate : m_Date);
}

/// <summary>
/// 
/// </summary>
void Comment::AfterRead(const std::string&)
{
    if (Project_().GetVersion() < BCFVer_3_0) {
        if (!*m_Viewpoint.GetGuid()) { //empty comment appeared from a file
            if (m_Comment.empty()) {
                m_Comment.assign("empty");
            }
        }
    }
}

/// <summary>
/// 
/// </summary>
BCFTopic& Comment::GetTopic()
{ 
    return m_topic; 
}
