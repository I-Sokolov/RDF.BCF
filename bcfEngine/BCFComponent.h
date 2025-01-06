#pragma once

#include "BCFObject.h"
struct BCFViewPoint;

struct BCFComponent : public BCFObject
{
public:
    BCFComponent(BCFViewPoint& viewPoint);

    void Read(_xml::_element& elem, const std::string& folder);

private:
    BCFViewPoint& m_viewPoint;

    std::string m_IfcGuid;
    std::string m_OriginatingSystem;
    std::string m_AuthoringToolId;
};

