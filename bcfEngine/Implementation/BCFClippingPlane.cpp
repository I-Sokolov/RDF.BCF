
#include "pch.h"

#include "BCFClippingPlane.h"
#include "BCFViewPoint.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
BCFClippingPlane::BCFClippingPlane(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project(), parentList)
    , m_Location(viewPoint.Project())
    , m_Direction(viewPoint.Project())
{
}

/// <summary>
/// 
/// </summary>
void BCFClippingPlane::Read(_xml::_element & elem, const std::string& folder) 
{
    CHILDREN_START
        CHILD_READ_MEMBER(Location)
        CHILD_READ_MEMBER(Direction)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool BCFClippingPlane::Validate(bool fix)
{
    if (fix) {
        if (!m_Direction.IsSet()) {
            return Remove();
        }
        if (!m_Location.IsSet()) {
            return Remove();
        }
    }

    bool valid = true;
    REQUIRED(Direction, m_Direction.IsSet());
    REQUIRED(Location, m_Direction.IsSet());
    return valid;
}

/// <summary>
/// 
/// </summary>
void BCFClippingPlane::Write(_xml_writer& writer, const std::string& folder, const char* tag) 
{
    XMLFile::Attributes attr;
    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_MEMBER(Location);
    WRITE_MEMBER(Direction);
}

