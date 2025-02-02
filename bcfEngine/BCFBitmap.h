#pragma once

#include "BCFObject.h"
#include "XMLPoint.h"
struct BCFViewPoint;


struct BCFBitmap : public BCFObject
{
public:
    BCFBitmap(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

public:
    BCFBitmapFormat GetFormat();
    const char* GetReference() { return m_Reference.c_str(); }
    bool        GetLocation(BCFPoint& pt) { return m_Location.GetPoint(pt); }
    bool        GetNormal(BCFPoint& pt) { return m_Normal.GetPoint(pt); }
    bool        GetUp(BCFPoint& pt) { return m_Up.GetPoint(pt); }
    double      GetHeight() { return atof(m_Height.c_str()); }

    bool SetFormat(BCFBitmapFormat val);
    bool SetReference(const char* val);
    bool SetLocation(BCFPoint* pt) { return m_Location.SetPoint(pt); }
    bool SetNormal(BCFPoint* pt) { return m_Normal.SetPoint(pt); }
    bool SetUp(BCFPoint* pt) { return m_Up.SetPoint(pt); }
    bool SetHeight(double val) { return RealToStr(val, m_Height); }

private:
    std::string m_Format;
    std::string m_Reference;
    XMLPoint    m_Location;
    XMLPoint    m_Normal;
    XMLPoint    m_Up;
    std::string m_Height;
};

