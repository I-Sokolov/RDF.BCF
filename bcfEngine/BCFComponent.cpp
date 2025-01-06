#include "pch.h"
#include "BCFComponent.h"
#include "BCFViewPoint.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
BCFComponent::BCFComponent(BCFViewPoint& viewPoint)
    : BCFObject(viewPoint.Project())
    , m_viewPoint (viewPoint)
{
}

/// <summary>
/// 
/// </summary>
void BCFComponent::Read(_xml::_element& elem, const std::string&)
{
    ATTRS_START
        ATTR_GET(IfcGuid)
    ATTRS_END(UnknownNames::NotAllowed)


    CHILDREN_START
        CHILD_GET_CONTENT(OriginatingSystem)
        CHILD_GET_CONTENT(AuthoringToolId)
    CHILDREN_END
}
