#pragma once

#include "bcfTypes.h"

struct XMLPoint
{
public:
    XMLPoint(BCFProject& project) : m_project(project) {}

    void Read(_xml::_element& elem, const std::string& folder);

    BCFProject& Project() { return m_project; }

private:
    BCFProject& m_project;

    std::string m_X;
    std::string m_Y;
    std::string m_Z;
};
