#pragma once

#include "BCFObject.h"
struct BCFViewPoint;


struct BCFLine : public BCFObject
{
public:
    BCFLine(BCFViewPoint& viewPoint) : BCFObject(viewPoint.Project()) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}
};

