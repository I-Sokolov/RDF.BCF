#pragma once

struct BCFProject;

class BIMFile
{
public:
    BIMFile (BCFProject& project) : m_project(project) {}

    void Read(_xml::_element& elem, const std::string& folder);

private:
    BCFProject& m_project;

    std::string m_IsExternal;
    std::string m_Filename;
    std::string m_Date;
    std::string m_Reference;
};

