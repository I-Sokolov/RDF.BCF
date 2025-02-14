#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"
#include "XMLPoint.h"

struct ViewPoint;

struct Line : public BCFObject, public BCFLine
{
public:
    Line(ViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

public:
    //BCFLine
    virtual bool GetStartPoint(BCFPoint& pt) override { return m_StartPoint.GetPoint(pt); }
    virtual bool GetEndPoint(BCFPoint& pt) override { return m_EndPoint.GetPoint(pt); }

    virtual bool SetStartPoint(BCFPoint* pt) override { return m_StartPoint.SetPoint(pt); }
    virtual bool SetEndPoint(BCFPoint* pt) override { return m_EndPoint.SetPoint(pt); }

    virtual BCFViewPoint& GetViewPoint() override;
    virtual bool Remove() override { return RemoveImpl(); }

private:
    ViewPoint&  m_viewPoint;

    XMLPoint    m_StartPoint;
    XMLPoint    m_EndPoint;
};

