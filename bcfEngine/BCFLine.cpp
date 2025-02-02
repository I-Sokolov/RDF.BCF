#include "pch.h"

#include "BCFLine.h"
#include "BCFViewPoint.h"
#include "BCFProject.h"

/// <summary>
/// 
/// </summary>
BCFLine::BCFLine(BCFViewPoint& viewPoint, ListOfBCFObjects* parentList)
    : BCFObject(viewPoint.Project(), parentList)
    , m_StartPoint(viewPoint.Project())
    , m_EndPoint(viewPoint.Project())
{ 
}

/// <summary>
/// 
/// </summary>
void BCFLine::Read(_xml::_element& elem, const std::string& folder)
{
    CHILDREN_START
        CHILD_READ_MEMBER(StartPoint)
        CHILD_READ_MEMBER(EndPoint)
    CHILDREN_END
}

/// <summary>
/// 
/// </summary>
bool BCFLine::Validate(bool fix)
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
void BCFLine::Write(_xml_writer& writer, const std::string& folder, const char* tag)
{
    XMLFile::Attributes attr;
    XMLFile::ElemTag _(writer, tag, attr);

    WRITE_MEMBER(StartPoint);
    WRITE_MEMBER(EndPoint);
}


