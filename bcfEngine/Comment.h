#pragma once

#include "GUIDable.h"

struct BCFProject;

class Comment
{
public:
    Comment(BCFProject& project, const char* guid = NULL) : m_project(project), m_Guid(project, guid), m_Viewpoint(project) {}

    void Read(_xml::_element& elem, const std::string& folder);

private:
    BCFProject&                m_project;

private:
    BCFGuid                    m_Guid;
    std::string                m_Date;
    std::string                m_Author;
    std::string                m_Comment;
    std::string                m_ModifiedDate;
    std::string                m_ModifiedAuthor;
    GuidReference              m_Viewpoint;
};

