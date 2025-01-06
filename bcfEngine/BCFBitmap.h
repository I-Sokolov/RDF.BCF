#pragma once

#include "BCFObject.h"
struct BCFViewPoint;


struct BCFBitmap : public BCFObject
{
public:
    BCFBitmap(BCFViewPoint& viewPoint) : BCFObject(viewPoint.Project()) {}

    void Read(_xml::_element & elem, const std::string& folder) {}
};

