
#include "pch.h"

#include "ClippingPlane.h"
#include "ViewPoint.h"
#include "Project.h"

/// <summary>
/// 
/// </summary>
ClippingPlane::ClippingPlane(ViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project_(), parentList)
    , m_viewPoint(viewPoint)
    , m_Location(*this)
    , m_Direction(*this)
{
}

/// <summary>
/// 
/// </summary>
void ClippingPlane::Read(_xml::_element & elem, const std::string& folder) 
{
    CHILDREN_START
        CHILD_READ_MEMBER(Location)
        CHILD_READ_MEMBER(Direction)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool ClippingPlane::Validate(bool fix)
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
void ClippingPlane::Write(_xml_writer& writer, const std::string& folder, const char* tag) 
{
    XMLFile::Attributes attr;
    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_MEMBER(Location);
    WRITE_MEMBER(Direction);
}

/// <summary>
/// 
/// </summary>
BCFViewPoint& ClippingPlane::GetViewPoint() 
{ 
    return m_viewPoint; 
}

/// <summary>
/// 
/// </summary>
Topic* ClippingPlane::Topic_()
{
    return m_viewPoint.Topic_();
}
