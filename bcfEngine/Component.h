#pragma once

struct BCFProject;


class Component
{
public:
    Component(BCFProject& project) : m_project(project) {}

    void Read(_xml::_element & elem, const std::string& folder) {}

private:
    BCFProject& m_project;

    std::string m_IfcGuid;
};

