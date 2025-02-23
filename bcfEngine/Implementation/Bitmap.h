#pragma once

#include "bcfAPI.h"
#include "BCFObject.h"
#include "XMLPoint.h"

struct ViewPoint;

struct Bitmap : public BCFObject, public BCFBitmap
{
public:
    Bitmap(ViewPoint& viewPoint, ListOfBCFObjects* parentList);

    void Read(_xml::_element& elem, const std::string& folder);
    void Write(_xml_writer& writer, const std::string& folder, const char* tag);
    void AfterRead(const std::string& folder);
    bool Validate(bool fix);

public:
    //BCFBitmap
    virtual BCFBitmapFormat GetFormat() override;
    virtual const char* GetReference() override { return m_Reference.c_str(); }
    virtual bool        GetLocation(BCFPoint& pt) override { return m_Location.GetPoint(pt); }
    virtual bool        GetNormal(BCFPoint& pt) override { return m_Normal.GetPoint(pt); }
    virtual bool        GetUp(BCFPoint& pt) override { return m_Up.GetPoint(pt); }
    virtual double      GetHeight() override { return atof(m_Height.c_str()); }

    virtual bool SetFormat(BCFBitmapFormat val) override;
    virtual bool SetReference(const char* val) override;
    virtual bool SetLocation(BCFPoint* pt) override { return m_Location.SetPoint(pt); }
    virtual bool SetNormal(BCFPoint* pt) override { return m_Normal.SetPoint(pt); }
    virtual bool SetUp(BCFPoint* pt) override { return m_Up.SetPoint(pt); }
    virtual bool SetHeight(double val) override { return SetPropertyReal(val, m_Height); }
    
    virtual BCFViewPoint& GetViewPoint() override;
    virtual bool Remove() override { return RemoveImpl(); }

private:
    ViewPoint& m_viewPoint;

    std::string m_Format;
    std::string m_Reference;
    XMLPoint    m_Location;
    XMLPoint    m_Normal;
    XMLPoint    m_Up;
    std::string m_Height;
};

