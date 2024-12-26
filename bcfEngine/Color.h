#pragma once

#include "Component.h"

class Color
{
public:
    Color(BCFProject& project) : m_project(project) {}

    void Read(_xml::_element & elem, const std::string& folder) {}

private:
    BCFProject&             m_project;

    std::string             m_Color;
    std::vector<Component>  m_Components;
};

