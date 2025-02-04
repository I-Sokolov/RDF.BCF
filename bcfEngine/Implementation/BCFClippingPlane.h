#pragma once

#include "BCFObject.h"
#include "XMLPoint.h"
struct BCFViewPoint;


struct BCFClippingPlane : public BCFObject
{
public:
    BCFClippingPlane(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

public:
    bool GetLocation(BCFPoint& pt) { return m_Location.GetPoint(pt); }
    bool GetDirection(BCFPoint& pt) { return m_Direction.GetPoint(pt); }

    bool SetLocation(BCFPoint* pt) { return m_Location.SetPoint(pt); }
    bool SetDirection(BCFPoint* pt) { return m_Direction.SetPoint(pt); }

private:
    XMLPoint m_Location;
    XMLPoint m_Direction;
};

