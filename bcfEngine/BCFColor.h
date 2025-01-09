#pragma once

#include "BCFObject.h"
struct BCFComponent;

struct BCFColor : public BCFObject
{
public:
    BCFColor(BCFViewPoint& viewPoint) : BCFObject(viewPoint.Project()) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}
    void Write(_xml_writer& writer, const std::string& folder, const char* tag) { assert(!"TODO"); }

private:
    std::string                m_Color;
    //std::vector<BCFComponent>  m_Components;
};

