#pragma once

#include "bcfTypes.h"

struct XMLPoint
{
public:
    XMLPoint(BCFProject& project) : m_project(project), m_X(XYZ[0]), m_Y(XYZ[1]), m_Z(XYZ[2]) {}

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);

    BCFProject& Project() { return m_project; }

    bool IsSet() { return !m_X.empty() && !m_Y.empty() && !m_Z.empty(); }

    std::string XYZ[3];

private:
    BCFProject& m_project;

    std::string& m_X;
    std::string& m_Y;
    std::string& m_Z;
};

