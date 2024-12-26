#pragma once

#include "GUIDable.h"

struct BCFProject;

class Comment : public GUIDable
{
public:
    Comment(BCFProject& project) : GUIDable(NULL), m_project(project) {}

    void Read(_xml::_element& elem, const std::string& folder);

private:
    BCFProject&                m_project;

private:
    std::string                m_Date;
    std::string                m_Author;
    std::string                m_Comment;
    std::string                m_ModifiedDate;
    std::string                m_ModifiedAuthor;
    std::vector<GuidReference> m_Viewpoints;
};

