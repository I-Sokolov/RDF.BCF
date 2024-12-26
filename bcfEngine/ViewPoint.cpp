#include "pch.h"
#include "ViewPoint.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
void ViewPoint::Read(_xml::_element& elem, const std::string& folder)
{
    ATTRS_START
        ATTR_GET(Guid)
    ATTRS_END(UnknownNames::NotAllowed)

    CHILDREN_START
        CHILD_GET_CONTENT(Viewpoint)
        CHILD_GET_CONTENT(Snapshot)
    CHILDREN_END

    if (!ReadFile(folder)) {
        throw std::exception();
    }
}

/// <summary>
/// 
/// </summary>
void ViewPoint::ReadRoot(_xml::_element& elem, const std::string& folder)
{

}
