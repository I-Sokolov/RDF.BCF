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

private:
    std::string m_Format;
    std::string m_Reference;
    XMLPoint    m_Location;
    XMLPoint    m_Normal;
    XMLPoint    m_Up;
    std::string m_Height;
};

