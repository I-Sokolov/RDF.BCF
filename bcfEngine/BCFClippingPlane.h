#pragma once

#include "BCFObject.h"
struct BCFViewPoint;


struct BCFClippingPlane : public BCFObject
{
public:
    BCFClippingPlane(BCFViewPoint& viewPoint) : BCFObject(viewPoint.Project()) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}
};

