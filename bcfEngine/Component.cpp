#include "pch.h"
#include "Point.h"
#include "XMLFile.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
void Component::Read(_xml::_element& elem, const std::string&)
{
    ATTRS_START
        ATTR_GET(IfcGuid)
    ATTRS_END(UnknownNames::NotAllowed)


    CHILDREN_START
        CHILD_GET_CONTENT(OriginatingSystem)
        CHILD_GET_CONTENT(AuthoringToolId)
    CHILDREN_END
}
