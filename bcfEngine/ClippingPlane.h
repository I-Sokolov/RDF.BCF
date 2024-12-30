#pragma once

class ViewPoint;


class ClippingPlane
{
public:
    ClippingPlane(ViewPoint&) { assert(!"TODO"); }

    void Read(_xml::_element & elem, const std::string& folder) {}
};

