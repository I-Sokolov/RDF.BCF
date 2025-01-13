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

/// <summary>
/// 
/// </summary>
void BCFComponent::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{ 
    if (m_AuthoringToolId.empty()) {
        REQUIRED_PROP(IfcGuid);
    }

    XMLFile::Attributes attr;
    ATTR_ADD(IfcGuid);

    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_CONTENT(OriginatingSystem);
    WRITE_CONTENT(AuthoringToolId);
}

