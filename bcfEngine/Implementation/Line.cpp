#include "pch.h"

#include "Line.h"
#include "ViewPoint.h"
#include "Project.h"

/// <summary>
/// 
/// </summary>
Line::Line(ViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project_(), parentList)
    , m_viewPoint(viewPoint)
    , m_StartPoint(*this)
    , m_EndPoint(*this)
{ 
}

/// <summary>
/// 
/// </summary>
void Line::Read(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ_MEMBER(StartPoint)
        CHILD_READ_MEMBER(EndPoint)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool Line::Validate(bool fix)
{
    if (fix) {
        if (!m_StartPoint.IsSet()) {
            return Remove();
        }
        if (!m_EndPoint.IsSet()) {
            return Remove();
        }
    }

    bool valid = true;
    REQUIRED(StartPoint, m_StartPoint.IsSet());
    REQUIRED(EndPoint, m_EndPoint.IsSet());
    return valid;
}

/// <summary>
/// 
/// </summary>
void Line::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{
    XMLFile::Attributes attr;
    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_MEMBER(StartPoint);
    WRITE_MEMBER(EndPoint);
}


/// <summary>
/// 
/// </summary>
BCFViewPoint& Line::GetViewPoint() 
{ 
    return m_viewPoint; 
}

/// <summary>
/// 
/// </summary>
Topic* Line::Topic_()
{
    return m_viewPoint.Topic_();
}
