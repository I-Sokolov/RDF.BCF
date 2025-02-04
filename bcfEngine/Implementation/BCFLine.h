#pragma once

#include "BCFObject.h"
#include "XMLPoint.h"
struct BCFViewPoint;


struct BCFLine : public BCFObject
{
public:
    BCFLine(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    bool Validate(bool fix);

public:
    bool GetStartPoint(BCFPoint& pt) { return m_StartPoint.GetPoint(pt); }
    bool GetEndPoint(BCFPoint& pt) { return m_EndPoint.GetPoint(pt); }

    bool SetStartPoint(BCFPoint* pt) { return m_StartPoint.SetPoint(pt); }
    bool SetEndPoint(BCFPoint* pt) { return m_EndPoint.SetPoint(pt); }

private:
    XMLPoint    m_StartPoint;
    XMLPoint    m_EndPoint;
};

