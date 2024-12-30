#pragma once

struct BCFProject;
class  Topic;

class BIMFile
{
public:
    BIMFile (Topic& topic) : m_topic(topic) {}

    void Read(_xml::_element& elem, const std::string& folder);

    BCFProject& Project();

private:
    Topic& m_topic;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
};

