#pragma once

struct BCFProject;
class ViewPoint;

class Component
{
public:
    Component(ViewPoint& viewPoint);

    void Read(_xml::_element& elem, const std::string& folder);

    BCFProject& Project();

private:
    ViewPoint& m_viewPoint;

    std::string m_IfcGuid;
    std::string m_OriginatingSystem;
    std::string m_AuthoringToolId;
};

