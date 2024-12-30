#pragma once

#include "Component.h"

class Color
{
public:
    Color(ViewPoint& viewPoint) {}

    void Read(_xml::_element & elem, const std::string& folder) {}

private:
    std::string             m_Color;
    std::vector<Component>  m_Components;
};

