#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"
#include "XMLPoint.h"

struct ViewPoint;

struct ClippingPlane : public BCFObject, public BCFClippingPlane
{
public:
    ClippingPlane(ViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

public:
    //BCFClippingPlane
    virtual bool GetLocation(BCFPoint& pt) override { return m_Location.GetPoint(pt); }
    virtual bool GetDirection(BCFPoint& pt) override { return m_Direction.GetPoint(pt); }

    virtual bool SetLocation(BCFPoint* pt) override { return m_Location.SetPoint(pt); }
    virtual bool SetDirection(BCFPoint* pt) override { return m_Direction.SetPoint(pt); }

    virtual BCFViewPoint& GetViewPoint() override;
    virtual bool Remove() override { return RemoveImpl(); }

private:
    ViewPoint& m_viewPoint;

    XMLPoint m_Location;
    XMLPoint m_Direction;
};

