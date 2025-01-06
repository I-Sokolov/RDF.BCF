#pragma once

#include "BCFObject.h"
struct BCFComponent;

struct BCFColor : public BCFObject
{
public:
    BCFColor(BCFViewPoint& viewPoint) : BCFObject(viewPoint.Project()) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}

private:
    std::string                m_Color;
    //std::vector<BCFComponent>  m_Components;
};

