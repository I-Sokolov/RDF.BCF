#pragma once

#include "BCFObject.h"
#include "GuidReference.h"
#include "GuidStr.h"

struct BCFViewPoint;


struct BCFComment : public BCFObject
{
public:
    BCFComment(BCFTopic& topic, ListOfBCFObjects* parentList, const char* guid = NULL);

public:
    const char* GetGuid() { return m_Guid.c_str(); }
    const char* GetDate() { return m_Date.c_str(); }
    const char* GetAuthor() { return m_Author.c_str(); }
    const char* GetModifiedDate() { return m_ModifiedDate.c_str(); }
    const char* GetModifiedAuthor() { return m_ModifiedAuthor.c_str(); }
    const char* GetText() { return m_Comment.c_str(); }
    BCFViewPoint* GetViewPoint();

    bool SetText(const char* value);
    bool SetViewPoint(BCFViewPoint* viewPoint);

private:
    bool UpdateAuthor();

public:
    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void UpgradeReadVersion(const std::string& folder);

private:
    void Write_Comment(_xml_writer& writer, const std::string& folder);

private:
    BCFTopic&                  m_topic;
    bool                       m_readFromFile;

private:
    GuidStr                    m_Guid;
    std::string                m_Date;
    std::string                m_Author;
    std::string                m_Comment;
    std::string                m_ModifiedDate;
    std::string                m_ModifiedAuthor;
    GuidReference              m_Viewpoint;
};

