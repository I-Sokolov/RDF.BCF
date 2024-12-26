#include "pch.h"
#include "Color.h"
#include "XMLFile.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
void Color::Read(_xml::_element& elem, const std::string&)
{
    ATTRS_START
        ATTR_GET(Color)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_LIST(Components, Component)
    CHILDREN_END

}
