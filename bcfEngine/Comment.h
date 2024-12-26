#pragma once

#include "GUIDable.h"

class Comment : public GUIDable
{
public:
    Comment(Log& log) : GUIDable(NULL), m_log(log) {}

    void Read(_xml::_element& elem);

private:
    std::string                m_Date;
    std::string                m_Author;
    std::string                m_Comment;
    std::string                m_ModifiedDate;
    std::string                m_ModifiedAuthor;
    std::vector<GuidReference> m_lstViewpoint;

private:
    Log& m_log;
};

