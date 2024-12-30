#pragma once

#include "GUIDable.h"

struct BCFProject;
class Topic;
class ViewPoint;

class Comment : public BCFObject
{
public:
    Comment(Topic& topic, const char* guid = NULL);

    void Read(_xml::_element& elem, const std::string& folder);

public:
    const char* GetGuid() { return m_Guid.c_str(); }
    const char* GetDate() { return m_Date.c_str(); }
    const char* GetAuthor() { return m_Author.c_str(); }
    const char* GetModifiedDate() { return m_ModifiedDate.c_str(); }
    const char* GetModifiedAuthor() { return m_ModifiedAuthor.c_str(); }
    const char* GetComment() { return m_Comment.c_str(); }
    ViewPoint* GetViewPoint();

    bool SetComment(const char* value);
    bool SetViewPoint(ViewPoint* viewPoint);
    
private:
    bool UpdateAuthor();

private:
    Topic&                     m_topic;
    bool                       m_bReadFromFile;

private:
    BCFGuid                    m_Guid;
    std::string                m_Date;
    std::string                m_Author;
    std::string                m_Comment;
    std::string                m_ModifiedDate;
    std::string                m_ModifiedAuthor;
    GuidReference              m_Viewpoint;
};

