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
    void UpgradeReadVersion();

public:
    BCFBitmapFormat GetFormat();
    const char* GetReference() { return m_Reference.c_str(); }
    bool        GetLocation(BCFPoint& pt) { return GetPoint(m_Location, pt); }
    bool        GetNormal(BCFPoint& pt) { return GetPoint(m_Normal, pt); }
    bool        GetUp(BCFPoint& pt) { return GetPoint(m_Up, pt); }
    double      GetHeight() { return atof(m_Height.c_str()); }

    bool SetFormat(BCFBitmapFormat val);
    bool SetReference(const char* val);
    bool SetLocation(BCFPoint* pt) { return SetPoint(pt, m_Location); }
    bool SetNormal(BCFPoint* pt) { return SetPoint(pt, m_Normal); }
    bool SetUp(BCFPoint* pt) { return SetPoint(pt, m_Up); }
    bool SetHeight(double val) { return RealToStr(val, m_Height); }

private:
    std::string m_Format;
    std::string m_Reference;
    XMLPoint    m_Location;
    XMLPoint    m_Normal;
    XMLPoint    m_Up;
    std::string m_Height;
};

