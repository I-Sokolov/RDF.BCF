#pragma once

#include "BCFObject.h"
struct BCFProject;
struct BCFTopic;

struct BCFFile : public BCFObject
{
public:
    BCFFile(BCFTopic& topic);

    void Read(_xml::_element& elem, const std::string& folder);

private:
    BCFTopic& m_topic;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
};

