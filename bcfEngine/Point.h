#pragma once

struct BCFProject;


class Point
{
public:
    Point(BCFProject& project) : m_project(project) {}

    void Read(_xml::_element& elem, const std::string& folder);

private:
    BCFProject& m_project;

    std::string m_X;
    std::string m_Y;
    std::string m_Z;
};

