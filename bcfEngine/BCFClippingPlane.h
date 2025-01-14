#pragma once

#include "BCFObject.h"
struct BCFViewPoint;


struct BCFClippingPlane : public BCFObject
{
public:
    BCFClippingPlane(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList) : BCFObject(viewPoint.Project(), parentList) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}
    void Write(_xml_writer& writer, const std::string& folder, const char* tag) { assert(!"TODO"); }
};

